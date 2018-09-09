using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.Common.DataAccess;
using Com.JinYiWei.Common.Extensions;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.FlyDogAPIBLL
{
    public class BillService : BaseService, IBillService
    {
        /// <summary>
        /// 查询可开发票项目
        /// </summary>
        /// <param name="customerID">顾客ID</param>
        /// <param name="hospitalID">医院ID</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<CanBillCharges>>> GetCanBillCharges(long customerID, long hospitalID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<CanBillCharges>>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            await TryExecuteAsync(async () =>
            {
                result.Data = await _connection.QueryAsync<CanBillCharges>(
                    @"select a.ID,f.PaidTime,e.Name as ChargeName,a.Num,isnull(sum(b.Num),0) as RebateNum,a.FinalPrice,sum(c.CouponAmount) as Coupon,
                    sum(c.CommissionAmount) as Commission,isnull(sum(d.CashCardAmount+d.DepositAmount),0) as RebateAmount, a.FinalPrice-sum(c.CouponAmount)-sum(c.CommissionAmount) as RealAmount
                    from SmartOrderDetail a
                    left join SmartCharge e on a.ChargeID=e.ID
                    inner join SmartOrder f on a.OrderID=f.ID and f.PaidStatus!=@PaidStatus and f.CustomerID=@CustomerID and f.HospitalID=@HospitalID
                    left join SmartBackOrderDetail b on a.ID=b.DetailID and b.OrderID in (select ID from SmartBackOrder where PaidStatus!=@PaidStatus)
                    left join SmartCashierCharge c on a.ID=c.ReferID and c.OrderType in (1,2)
                    left join SmartCashierCharge d on a.ID=d.ReferID and d.OrderType=4 
                    where a.ID not in (select OrderDetailID from SmartBillDetail)
                    group by a.ID,e.Name,e.Size,a.Num,a.RestNum,b.Num, a.FinalPrice,f.PaidTime",
                    new { PaidStatus = PaidStatus.NotPaid, CustomerID = customerID, HospitalID = hospitalID });
            });

            return result;
        }

        /// <summary>
        /// 添加发票
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Add(BillAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (!dto.Name.IsNullOrEmpty() && dto.Name.Length >= 50)
            {
                result.Message = "发票抬头不能超过50字";
                return result;
            }
            if (dto.Code.IsNullOrEmpty())
            {
                result.Message = "发票号不能为空！";
                return result;
            }
            else if (!dto.Code.IsNullOrEmpty() && dto.Code.Length >= 50)
            {
                result.Message = "发票号不能超过50字！";
                return result;
            }
            if (dto.Remark.IsNullOrEmpty())
            {
                dto.Remark = " ";
            }
            else if (!dto.Remark.IsNullOrEmpty() && dto.Remark.Length >= 50)
            {
                result.Message = "备注不能超过50字！";
                return result;
            }


            if (dto.OrderDetailID.Count() == 0)
            {
                result.Message = "请选择开发票的项目！";
                return result;
            }

            if (string.IsNullOrWhiteSpace(dto.CreateDate.ToString()) || dto.CreateDate.ToString() == "0001/1/1 0:00:00")
            {
                result.Message = "发票日期不能为空！";
                return result;
            }

            await TryTransactionAsync(async () =>
            {

                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();
                var task1 = _connection.ExecuteAsync(
                    @"insert into [SmartBill]([ID],[HospitalID],[CustomerID],[Name],[Code],[CreateTime],[CreateUserID],[Remark],[Amount],[CreateDate]) 
                    values(@ID,@HospitalID,@CustomerID,@Name,@Code,@CreateTime,@CreateUserID,@Remark,@Amount,@CreateDate)",
                    new
                    {
                        ID = id,
                        HospitalID = dto.HospitalID,
                        CustomerID = dto.CustomerID,
                        Name = dto.Name,
                        Code = dto.Code,
                        CreateTime = DateTime.Now,
                        CreateUserID = dto.CreateUserID,
                        Remark = dto.Remark,
                        Amount = dto.Amount,
                        CreateDate = dto.CreateDate.Date
                    }, _transaction);

                var task2 = _connection.ExecuteAsync(
                    @"insert into [SmartBillDetail]([ID],[BillID],[OrderDetailID]) values(@ID,@BillID,@OrderDetailID)",
                    dto.OrderDetailID.Select(u => new { ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(), BillID = id, OrderDetailID = u }), _transaction, 30, CommandType.Text);

                await Task.WhenAll(task1, task2);
                result.Message = "添加成功！";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 删除发票
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Delete(BillDelete dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Success;
            result.Message = "删除成功！";

            await TryTransactionAsync(async () =>
            {
                int count = await _connection.ExecuteAsync(
                    @"delete from [SmartBill] where ID=@ID and [HospitalID]=@HospitalID", dto, _transaction);

                if (count == 0)
                {
                    result.Message = "发票不存在或者该发票不属于您的医院！";
                    result.ResultType = IFlyDogResultType.Failed;
                    return false;
                }
                await _connection.ExecuteAsync(
                    @"delete from [SmartBillDetail] where [OrderDetailID]=@ID", dto, _transaction);

                return true;
            });

            return result;
        }

        /// <summary>
        /// 获取今日发票记录
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<Bill>>> GetBillToday(long hospitalID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<Bill>>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            await TryExecuteAsync(async () =>
            {
                result.Data = await _connection.QueryAsync<Bill>(
                    @"select a.ID,a.CreateTime,a.CustomerID,c.Name as CustomerName,b.Name as CreateUserName,a.Amount,
                    a.Code,a.Name,a.Remark,a.CreateDate
                    from SmartBill a
                    inner join SmartUser b on a.CreateUserID=b.ID
                    inner join SmartCustomer c on a.CustomerID=c.ID
                    where a.CreateTime between @StartTime and @EndTime and a.HospitalID=@HospitalID",
                    new { StartTime = DateTime.Today, EndTime = DateTime.Today.AddDays(1), HospitalID = hospitalID });
            });

            return result;
        }
    }
}
