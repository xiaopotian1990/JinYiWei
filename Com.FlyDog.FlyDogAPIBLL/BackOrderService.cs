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
    public class BackOrderService : BaseService, IBackOrderService
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Add(BackOrderAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            dto.Details=dto.Details.Where(u => u.Num > 0);

            if (dto.Details == null || dto.Details.Count() == 0)
            {
                result.Message = "请选择项目！";
                return result;
            }

            if (!dto.Remark.IsNullOrEmpty() && dto.Remark.Length >= 50)
            {
                result.Message = "备注不能超过50字！";
                return result;
            }

            await TryTransactionAsync(async () =>
            {
                if (!await HasCustomerOAuthTransactionAsync(dto.CreateUserID, dto.CustomerID))
                {
                    result.Message = "对不起，您无权操作该用户！";
                    result.ResultType = IFlyDogResultType.NoAuth;
                    return false;
                }
                if (!await IsCome(dto.CustomerID, dto.CreateUserID))
                {
                    result.Message = "今日未上门，不能操作，请在前台先分诊 ！";
                    return false;
                }


                var charges = await _connection.QueryAsync<BackOrderDetailAdd>(
                    string.Format(@"select a.ID as DetailID,a.ChargeID,a.RestNum as Num from SmartOrderDetail a inner join SmartOrder b on a.OrderID=b.ID and b.PaidStatus in (@PaidStatus,@PaidStatus2) and a.RestNum>0 
                    and b.CustomerID=@CustomerID and b.HospitalID=@HospitalID"), new { PaidStatus = PaidStatus.Paid, PaidStatus2=PaidStatus.Debt, CustomerID = dto.CustomerID, HospitalID = dto.HospitalID }, _transaction);

                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();
                List<BackOrderDetailTemp> detailList = new List<BackOrderDetailTemp>();
                decimal amount = 0;
                foreach (var u in dto.Details)
                {
                    if (u.Num <= 0)
                    {
                        result.Message = "退项目数量不能小于0！";
                        return false;
                    }
                    foreach (var n in charges)
                    {
                        if (u.ChargeID == n.ChargeID && u.DetailID == n.DetailID)
                        {
                            if (u.Num > n.Num)
                            {
                                result.Message = "退项目数量不能大于剩余数量！";
                                return false;
                            }
                            detailList.Add(new BackOrderDetailTemp()
                            {
                                ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                                DetailID = u.DetailID,
                                Amount = u.Amount,
                                Num = u.Num,
                                ChargeID = u.ChargeID,
                                OrderID = id
                            });
                            amount += u.Amount;
                            continue;
                        }
                    }
                }

                if (detailList.Count == 0)
                {
                    result.Message = "请选择项目！";
                    return false;
                }

                AuditType auditType = await IsAudit(amount, dto.HospitalID);

                Task task1 = _connection.ExecuteAsync(
                    @"insert into [SmartBackOrder]([ID],[HospitalID],[CustomerID],[CreateUserID],[CreateTime],[Amount],[Point],[PaidStatus],[Remark],[AuditStatus]) 
                        values(@ID,@HospitalID,@CustomerID,@CreateUserID,@CreateTime,@Amount,@Point,@PaidStatus,@Remark,@AuditStatus)",
                    new
                    {
                        ID = id,
                        HospitalID = dto.HospitalID,
                        CustomerID = dto.CustomerID,
                        CreateUserID = dto.CreateUserID,
                        CreateTime = DateTime.Now,
                        Amount = amount,
                        Point = dto.Point,
                        AuditStatus = auditType,
                        Remark = dto.Remark,
                        PaidStatus = PaidStatus.NotPaid
                    }, _transaction);

                Task task2 = _connection.ExecuteAsync(
                    @"insert into [SmartBackOrderDetail]([ID],[OrderID],[ChargeID],[Num],[Amount],[DetailID]) 
                    values(@ID,@OrderID,@ChargeID,@Num,@Amount,@DetailID)", detailList, _transaction, 30, CommandType.Text);

                await Task.WhenAll(task1, task2);

                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Delete(DepositOrderDelete dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;


            await TryTransactionAsync(async () =>
            {
                if (!await HasCustomerOAuthTransactionAsync(dto.CreateUserID, dto.CustomerID))
                {
                    result.Message = "对不起，您无权操作该用户！";
                    result.ResultType = IFlyDogResultType.NoAuth;
                    return false;
                }

                var temp = (await _connection.QueryAsync(
                    @"select [PaidStatus],HospitalID from [SmartBackOrder] where [ID]=@ID", new { ID = dto.OrderID }, _transaction)).FirstOrDefault();

                if (temp == null)
                {
                    result.Message = "订单不存在！";
                    return false;
                }

                if ((PaidStatus)temp.PaidStatus == PaidStatus.Paid)
                {
                    result.Message = "已付款订单不允许删除！";
                    return false;
                }
                if ((long)temp.HospitalID != dto.HospitalID)
                {
                    result.Message = "对不起，您无权操作其他家医院的订单！";
                    return false;
                }

                Task task1 = _connection.ExecuteAsync(
                    @"delete from [SmartBackOrder] where ID=@ID", new { ID = dto.OrderID }, _transaction);
                Task task2 = _connection.ExecuteAsync(
                    @"delete from [SmartBackOrderDetail] where [OrderID]=@ID", new { ID = dto.OrderID }, _transaction);
                Task task3 = _connection.ExecuteAsync(
                    @"delete from SmartAudit where OrderID=@OrderID and OrderType=@OrderType", new { OrderID = dto.OrderID, OrderType = OrderType.Back }, _transaction);

                await Task.WhenAll(task1, task2, task3);

                result.Message = "删除成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 查询详细
        /// </summary>
        /// <param name="userID">操作用户ID</param>
        /// <param name="customerID">顾客ID</param>
        /// <param name="orderID">订单ID</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, BackOrder>> GetDetail(long userID, long customerID, long orderID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, BackOrder>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;
            await TryExecuteAsync(async () =>
            {
                if (!await HasCustomerOAuthTransactionAsync(userID, customerID))
                {
                    result.Message = "对不起，您无权操作该用户！";
                    result.ResultType = IFlyDogResultType.NoAuth;
                    return;
                }

                var list = new Dictionary<string, BackOrder>();
                await _connection.QueryAsync<BackOrder, BackOrderDetial, BackOrder>(
                    @"select a.ID as OrderID,a.HospitalID,d.Name as HospitalName,a.CreateTime,e.Name as CreateUserName,
                    a.Amount,a.Point,a.PaidStatus,a.PaidTime,a.Remark,a.AuditStatus,b.ChargeID,c.Name as ChargeName,b.Amount,b.Num
                    from SmartBackOrder a
                    inner join SmartBackOrderDetail b on a.ID=b.OrderID
                    inner join SmartCharge c on b.ChargeID=c.ID
                    inner join SmartHospital d on a.HospitalID=d.ID
                    inner join SmartUser e on a.CreateUserID=e.ID where a.ID=@ID and a.CustomerID=@CustomerID",
                    (order, detail) =>
                    {
                        BackOrder temp = new BackOrder();
                        if (!list.TryGetValue(order.OrderID, out temp))
                        {
                            list.Add(order.OrderID, temp = order);
                        }
                        if (detail != null)
                            temp.Details.Add(detail);
                        return order;
                    }, new { CustomerID = customerID, ID = orderID }, _transaction, true, splitOn: "ChargeID");

                result.Data = list.Values.FirstOrDefault();
            });

            return result;
        }

        /// <summary>
        /// 判断是否需要审核
        /// </summary>
        /// <param name="totalPrice"></param>
        /// <param name="finalPrice"></param>
        /// <param name="detailList"></param>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        private async Task<AuditType> IsAudit(decimal amount, long hospitalID)
        {
            var discountAmount = (await _connection.QueryAsync<decimal?>(@"select [Amount] from [SmartAuditRule] where [Type]=@Type and [HospitalID]=@HospitalID and Status=@Status",
                    new { Type = RuleType.BackOrder, HospitalID = hospitalID, Status = CommonStatus.Use }, _transaction)).FirstOrDefault();


            AuditType auditStatus = AuditType.NoApprove;
            if (amount > 0)
            {
                if (discountAmount != null)
                {
                    if (amount > discountAmount)
                    {
                        auditStatus = AuditType.Pending;
                    }
                }
            }

            return auditStatus;
        }
    }
}
