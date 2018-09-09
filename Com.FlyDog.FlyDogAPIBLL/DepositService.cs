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
    public class DepositService : BaseService, IDepositService
    {
        /// <summary>
        /// 添加预收款
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> AddOrder(DepositOrderAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.Details == null || dto.Details.Count() == 0)
            {
                result.Message = "请选择预收款项目！";
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

                string sql_where = " where [Status]=@Status";
                int count = 0;
                foreach (var u in dto.Details)
                {
                    if (count == 0)
                        sql_where += " and [ID]=" + u.ChargeID;
                    else
                        sql_where += " or ID=" + u.ChargeID;

                    count++;
                }
                var depositCharges = await _connection.QueryAsync<DepositOrderDetailAdd>(
                    string.Format(@"select distinct [ID] as ChargeID,[Price] from [SmartDepositCharge] {0}", sql_where), new { Status = CommonStatus.Use }, _transaction);


                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();
                List<DepositOrderDetailTemp> detailList = new List<DepositOrderDetailTemp>();
                decimal amount = 0;
                foreach (var u in dto.Details)
                {
                    if (u.Num <= 0)
                    {
                        result.Message = "预收款数量不能小于0！";
                        return false;
                    }
                    foreach (var n in depositCharges)
                    {
                        if (u.ChargeID == n.ChargeID)
                        {
                            detailList.Add(new DepositOrderDetailTemp()
                            {
                                ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                                OrderID = id,
                                Num = u.Num,
                                ChargeID = u.ChargeID,
                                Price = n.Price,
                                Total = n.Price * u.Num
                            });

                            amount += n.Price * u.Num;
                            continue;
                        }
                    }
                }

                Task task1 = _connection.ExecuteAsync(
                    @"insert into [SmartDepositOrder]([ID],[HospitalID],[CustomerID],[CreateUserID],[CreateTime],[Amount],[Remark],[PaidStatus]) 
                        values(@ID,@HospitalID,@CustomerID,@CreateUserID,@CreateTime,@Amount,@Remark,@PaidStatus)",
                    new
                    {
                        ID = id,
                        HospitalID = dto.HospitalID,
                        CustomerID = dto.CustomerID,
                        CreateUserID = dto.CreateUserID,
                        CreateTime = DateTime.Now,
                        Amount = amount,
                        Remark = dto.Remark,
                        PaidStatus = PaidStatus.NotPaid
                    }, _transaction);

                Task task2 = _connection.ExecuteAsync(
                    @"insert into [SmartDepositOrderDetail]([ID],[OrderID],[ChargeID],[Price],[Num],[Total]) 
                    values(@ID,@OrderID,@ChargeID,@Price,@Num,@Total)", detailList, _transaction, 30, CommandType.Text);

                await Task.WhenAll(task1, task2);

                result.Message = "添加预收款成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 预收款删除
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
                    @"select [PaidStatus],HospitalID from [SmartDepositOrder] where [ID]=@ID", new { ID = dto.OrderID }, _transaction)).FirstOrDefault();

                if (temp == null)
                {
                    result.Message = "预收款订单不存在！";
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
                    @"delete from [SmartDepositOrder] where ID=@ID", new { ID = dto.OrderID }, _transaction);
                Task task2 = _connection.ExecuteAsync(
                    @"delete from [SmartDepositOrderDetail] where [OrderID]=@ID", new { ID = dto.OrderID }, _transaction);

                await Task.WhenAll(task1, task2);

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
        public async Task<IFlyDogResult<IFlyDogResultType, DepositOrder>> GetDetail(long orderID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, DepositOrder>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;
            await TryExecuteAsync(async () =>
            {
                var list = new Dictionary<string, DepositOrder>();
                await _connection.QueryAsync<DepositOrder, DepositOrderDetial, DepositOrder>(
                    @"select a.ID as OrderID,a.CustomerID,a.HospitalID,d.Name as HospitalName,a.CreateTime,e.Name as CreateUserName,
                    a.Amount,a.PaidStatus,a.PaidTime,a.Remark,b.ChargeID,c.Name as ChargeName,b.Price,b.Num,b.Total 
                    from [SmartDepositOrder] a
                    inner join SmartDepositOrderDetail b on a.ID=b.OrderID
                    inner join SmartDepositCharge c on b.ChargeID=c.ID
                    inner join SmartHospital d on a.HospitalID=d.ID
                    inner join SmartUser e on a.CreateUserID=e.ID where a.ID=@ID",
                    (order, detail) =>
                    {
                        DepositOrder temp = new DepositOrder();
                        if (!list.TryGetValue(order.OrderID, out temp))
                        {
                            list.Add(order.OrderID, temp = order);
                        }
                        if (detail != null)
                            temp.Details.Add(detail);
                        return order;
                    }, new { ID = orderID }, _transaction, true, splitOn: "ChargeID");

                result.Data = list.Values.FirstOrDefault();
            });

            return result;
        }

        /// <summary>
        /// 查询剩余预收款
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<NoDoneDeposits>>> GetNoDoneOrders(long hospitalID, long customerID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<NoDoneDeposits>>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            await TryExecuteAsync(async () =>
            {
                result.Data = await _connection.QueryAsync<NoDoneDeposits>(
                    @"select a.ID as DepositID ,b.Name as DepositChargeName,a.Amount,a.Rest
                    from SmartDeposit a 
                    inner join SmartDepositCharge b on a.ChargeID=b.ID
                    where a.CustomerID=@CustomerID and a.HospitalID=@HospitalID and a.Rest>0", new { CustomerID = customerID, HospitalID = hospitalID });
            });

            return result;
        }

        /// <summary>
        /// 查询剩余预收款
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<NoDoneDeposits>>> GetCashierDeposits(long customerID, long orderID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<NoDoneDeposits>>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            await TryExecuteAsync(async () =>
            {
                result.Data = await _connection.QueryAsync<NoDoneDeposits>(
                    @"select * from SmartDeposit a,SmartDepositCharge b,SmartDepositChargeHospital c 
  where b.ScopeLimit=1 and b.ID=c.DepositChargeID and c.HospitalID=1 and a.ChargeID=b.ID and a.Rest>0 and a.CustomerID=1", new { CustomerID = customerID, HospitalID = 1 });
            });

            return result;
        }

        /// <summary>
        /// 添加预收款界面获取可购买的预收款
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<DepositChargeHospitalUse>>> GetAllDeposit(long hospitalID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<DepositChargeHospitalUse>>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;
            await TryExecuteAsync(async () =>
            {
                var temp = await _connection.QueryAsync<DepositChargeHospitalUseTemp>(
                    @"select a.ID,a.Name,a.Price,a.ScopeLimit,d.Name as ChargeName, e.Name as ChargeCategoryName,HasCoupon,f.Name as CouponCategoryName,a.CouponAmount
                    from SmartDepositCharge a
                    left join SmartDepositChargeCharge b on a.ID=b.DepositChargeID and a.ScopeLimit=3
                    left join SmartDepositChargeChargeCategory c on a.ID=c.DepositChargeID and a.ScopeLimit=2
                    left join SmartCharge d on b.ChargeID=d.ID
                    left join SmartChargeCategory e on c.ChargeCategoryID=e.ID
                    left join SmartCouponCategory f on a.CouponCategoryID=f.ID
                    where a.Status=@Status and a.ID in (select DepositChargeID from SmartDepositChargeHospital where HospitalID=@HospitalID)",
                    new
                    {
                        Status = CommonStatus.Use,
                        HospitalID = hospitalID
                    });

                var dic = new Dictionary<string, DepositChargeHospitalUse>();
                foreach (var u in temp)
                {
                    DepositChargeHospitalUse deposit = new DepositChargeHospitalUse();
                    if (!dic.TryGetValue(u.ID, out deposit))
                    {
                        dic.Add(u.ID, new DepositChargeHospitalUse()
                        {
                            ID = u.ID,
                            Name = u.Name,
                            CouponAmount = u.CouponAmount,
                            HasCoupon = u.HasCoupon == 0 ? "无" : u.CouponCategoryName + ":" + u.CouponAmount,
                            Price = u.Price,
                            ScopeLimit = u.ScopeLimit == 1 ? "无限制" : u.ScopeLimit == 2 ? "指定项目分类:" + u.ChargeCategoryName : "指定项目:" + u.ChargeName
                        });
                    }
                    else
                    {
                        if (u.ScopeLimit == 2)
                        {
                            deposit.ScopeLimit += "," + u.ChargeCategoryName;
                        }else if (u.ScopeLimit == 3)
                        {
                            deposit.ScopeLimit += "," + u.ChargeName;
                        }
                    }
                }

                result.Data = dic.Values;
            });

            return result;
        }
    }
}
