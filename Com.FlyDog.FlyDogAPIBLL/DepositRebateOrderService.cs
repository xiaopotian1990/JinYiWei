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
    public class DepositRebateOrderService : BaseService, IDepositRebateOrderService
    {
        /// <summary>
        /// 获取可退代金券跟预收款
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, CanRebate>> GetCanRebate(long hospitalID, long customerID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, CanRebate>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;
            await TryExecuteAsync(async () =>
            {
                var multi = await _connection.QueryMultipleAsync(
                   @"select a.ID as DepositID ,b.Name as DepositChargeName,a.Amount,a.Rest
                    from SmartDeposit a 
                    inner join SmartDepositCharge b on a.ChargeID=b.ID
                    where a.CustomerID=@CustomerID and a.HospitalID=@HospitalID and a.Rest>0 
                    select a.ID as CouponID ,b.Name as CouponCategoryName,a.Amount,a.Rest
                    from SmartCoupon a 
                    inner join SmartCouponCategory b on a.CategoryID=b.ID
                    where a.CustomerID=@CustomerID and a.HospitalID=@HospitalID and a.Status=@Status and a.Rest>0"
                    , new { CustomerID = customerID, HospitalID = hospitalID, Status = CouponStatus.Effective });

                result.Data = new CanRebate();
                result.Data.Deposits = await multi.ReadAsync<NoDoneDeposits>();
                result.Data.Coupons = await multi.ReadAsync<NoDoneCoupons>();

            });

            return result;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Add(DepositRebateOrderAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            dto.CouponDetails = dto.CouponDetails.Where(u => u.Amount > 0);
            dto.Details = dto.Details.Where(u => u.Amount > 0);

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

            if (dto.Amount < 0)
            {
                result.Message = "退款金额不能小于0！";
                return result;
            }
            if (dto.Point < 0)
            {
                result.Message = "扣减积分不能小于0！";
                return result;
            }


            if (dto.Remark.IsNullOrEmpty())
            {
                dto.Remark = " ";
            }
            else if (!dto.Remark.IsNullOrEmpty() && dto.Remark.Length > 50)
            {
                result.Message = "备注最多50个字符！";
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



                var deposits = await _connection.QueryAsync<DepositRebateOrderDetailAdd>(
                    string.Format(@"select [ID] as DepositID,[Rest] as [Amount] from [SmartDeposit] 
                    where [HospitalID]=@HospitalID and CustomerID=@CustomerID and Rest>0"), new { CustomerID = dto.CustomerID, HospitalID = dto.HospitalID }, _transaction);

                var coupons = await _connection.QueryAsync<DepositRebateOrderCouponDetailAdd>(
                    string.Format(@"select [ID] as CouponID,[Rest] as [Amount] from [SmartCoupon] 
                    where [HospitalID]=@HospitalID and CustomerID=@CustomerID and Status=@Status"), new { CustomerID = dto.CustomerID, HospitalID = dto.HospitalID, Status = CouponStatus.Effective }, _transaction);

                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();
                List<DepositRebateOrderDetailTemp> detailList = new List<DepositRebateOrderDetailTemp>();
                List<DepositRebateOrderCouponDetailTemp> couponDetailList = new List<DepositRebateOrderCouponDetailTemp>();
                decimal deposit = 0;
                decimal coupon = 0;
                foreach (var u in dto.Details)
                {
                    if (u.Amount <= 0)
                    {
                        result.Message = "预收款退款金额不能为0！";
                        return false;
                    }
                    foreach (var n in deposits)
                    {
                        if (u.DepositID == n.DepositID)
                        {
                            if (u.Amount > n.Amount)
                            {
                                result.Message = "对不起，预收款剩余金额不足！";
                                return false;
                            }
                            detailList.Add(new DepositRebateOrderDetailTemp()
                            {
                                ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                                OrderID = id,
                                Amount = u.Amount,
                                DepositID = u.DepositID
                            });

                            deposit += u.Amount;
                            continue;
                        }
                    }
                }

                foreach (var u in dto.CouponDetails)
                {
                    if (u.Amount <= 0)
                    {
                        result.Message = "券退款金额不能小于0！";
                        return false;
                    }
                    foreach (var n in coupons)
                    {
                        if (u.CouponID == n.CouponID)
                        {
                            if (u.Amount > n.Amount)
                            {
                                result.Message = "对不起，代金券剩余金额不足！";
                                return false;
                            }
                            couponDetailList.Add(new DepositRebateOrderCouponDetailTemp()
                            {
                                ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                                OrderID = id,
                                Amount = u.Amount,
                                CouponID = u.CouponID
                            });

                            coupon += u.Amount;
                            continue;
                        }
                    }
                }

                if (detailList.Count == 0 && couponDetailList.Count==0)
                {
                    result.Message = "请选择预收款项目或者券项目！";
                    return false;
                }

                AuditType auditType = await IsAudit(dto.Amount, dto.HospitalID);

                Task task1 = _connection.ExecuteAsync(
                    @"insert into [SmartDepositRebateOrder]([ID],[HospitalID],[CustomerID],[CreateTime],[CreateUserID],[Deposit],[Amount],[AuditStatus],[PaidStatus],[Remark],Coupon,Point) 
                        values(@ID,@HospitalID,@CustomerID,@CreateTime,@CreateUserID,@Deposit,@Amount,@AuditStatus,@PaidStatus,@Remark,@Coupon,@Point)",
                    new
                    {
                        ID = id,
                        HospitalID = dto.HospitalID,
                        CustomerID = dto.CustomerID,
                        CreateUserID = dto.CreateUserID,
                        CreateTime = DateTime.Now,
                        Point = dto.Point,
                        Amount = dto.Amount,
                        Coupon = coupon,
                        Deposit = deposit,
                        AuditStatus = auditType,
                        Remark = dto.Remark,
                        PaidStatus = PaidStatus.NotPaid
                    }, _transaction);

                Task task2 = _connection.ExecuteAsync(
                    @"insert into [SmartDepositRebateOrderDetail]([ID],[OrderID],[DepositID],[Amount]) 
                    values(@ID,@OrderID,@DepositID,@Amount)", detailList, _transaction, 30, CommandType.Text);
                Task task3 = _connection.ExecuteAsync(
                    @"insert into [SmartDepositRebateCouponOrderDetail]([ID],[OrderID],[CouponID],[Amount]) 
                    values(@ID,@OrderID,@CouponID,@Amount)", couponDetailList, _transaction, 30, CommandType.Text);

                await Task.WhenAll(task1, task2, task3);

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
                    @"select [PaidStatus],HospitalID from [SmartDepositRebateOrder] where [ID]=@ID", new { ID = dto.OrderID }, _transaction)).FirstOrDefault();

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
                    @"delete from [SmartDepositRebateOrder] where ID=@ID", new { ID = dto.OrderID }, _transaction);
                Task task2 = _connection.ExecuteAsync(
                    @"delete from [SmartDepositRebateOrderDetail] where [OrderID]=@ID", new { ID = dto.OrderID }, _transaction);
                Task task4 = _connection.ExecuteAsync(
                    @"delete from [SmartDepositRebateCouponOrderDetail] where [OrderID]=@ID", new { ID = dto.OrderID }, _transaction);
                Task task3 = _connection.ExecuteAsync(
                    @"delete from SmartAudit where OrderID=@OrderID and OrderType=@OrderType", new { OrderID = dto.OrderID, OrderType = OrderType.Refund }, _transaction);

                await Task.WhenAll(task1, task2, task3, task4);

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
        public async Task<IFlyDogResult<IFlyDogResultType, DepositRebateOrder>> GetDetail(long userID, long customerID, long orderID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, DepositRebateOrder>();
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

                var list = new Dictionary<string, DepositRebateOrder>();
                await _connection.QueryAsync<DepositRebateOrder, DepositRebateOrderDetial, DepositRebateCouponOrderDetial, DepositRebateOrder>(
                    @"select a.ID as OrderID,a.HospitalID,d.Name as HospitalName,a.CreateTime,e.Name as CreateUserName,
                    a.Amount,a.Deposit,a.Point,a.Coupon,a.PaidStatus,a.PaidTime,a.Remark,a.AuditStatus,b.DepositID,f.Name as DepositName,b.Amount,
					g.CouponID,i.Name as CouponName,g.Amount
                    from [SmartDepositRebateOrder] a
                    left join [SmartDepositRebateOrderDetail] b on a.ID=b.OrderID
                    left join SmartDeposit c on b.DepositID=c.ID
					inner join SmartDepositCharge f on c.ChargeID=f.ID
                    inner join SmartHospital d on a.HospitalID=d.ID
                    inner join SmartUser e on a.CreateUserID=e.ID 
					left join SmartDepositRebateCouponOrderDetail g on a.ID=g.OrderID
					left join SmartCoupon h on g.CouponID=h.ID
					left join SmartCouponCategory i on h.CategoryID=i.ID
					where a.ID=@ID and a.CustomerID=@CustomerID",
                    (order, detail, couponDetail) =>
                    {
                        DepositRebateOrder temp = new DepositRebateOrder();
                        if (!list.TryGetValue(order.OrderID, out temp))
                        {
                            list.Add(order.OrderID, temp = order);
                        }
                        if (detail != null)
                        {
                            if (!temp.Details.Any(u => u.DepositID == detail.DepositID))
                            {
                                temp.Details.Add(detail);
                            }
                        }
                        if (couponDetail != null)
                        {
                            if (!temp.CouponDetails.Any(u => u.CouponID == couponDetail.CouponID))
                            {
                                temp.CouponDetails.Add(couponDetail);
                            }
                        }
                        return order;
                    }, new { CustomerID = customerID, ID = orderID }, _transaction, true, splitOn: "DepositID,CouponID");

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
                    new { Type = RuleType.DepositRebate, HospitalID = hospitalID, Status = CommonStatus.Use }, _transaction)).FirstOrDefault();


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
