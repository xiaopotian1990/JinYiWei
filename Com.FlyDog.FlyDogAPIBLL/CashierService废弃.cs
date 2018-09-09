using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.Common;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.Cache;
using Com.JinYiWei.Common.DataAccess;
using Com.JinYiWei.Common.Extensions;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.FlyDogAPIBLL
{
    public class CashierService : BaseService
    {
        //private IDepositService _depositService;
        private IOptionService _optionService;
        private RedisStackExchangeHelper _redis = new RedisStackExchangeHelper();
        private IOrderService _orderService;
        private IDepositRebateOrderService _depositRebateOrder;
        public CashierService(IOptionService optionService, IDepositRebateOrderService depositRebateOrder, IOrderService orderService)
        {
            //_depositService = depositService;
            _optionService = optionService;
            _orderService = orderService;
            _depositRebateOrder = depositRebateOrder;
        }


        /// <summary>
        /// 获取支付代金券跟预收款
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, CanCashier>> GetCanCashier(long hospitalID, long customerID, long orderID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, CanCashier>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            //var order=await _orderService.getd
            await TryExecuteAsync(async () =>
            {
                var multi = await _connection.QueryMultipleAsync(
                   @"select a.ID as DepositID,a.HospitalID,c.Name as HospitalName,a.ChargeID as DepositChargeID,b.Name as DepositChargeName,a.Rest,b.ScopeLimit 
                   from SmartDeposit a,SmartDepositCharge b,SmartHospital c
                   where  b.ID in (select distinct DepositChargeID from [SmartDepositChargeHospital] where HospitalID=@HospitalID) and a.ChargeID=b.ID and a.Rest>0 and a.CustomerID=@CustomerID and a.HospitalID=c.ID
                   select a.ID as CouponID,a.HospitalID,c.Name as HospitalName,a.CategoryID as CouponChargeID,b.Name as CouponChargeName,a.Rest,b.ScopeLimit 
                   from SmartCoupon a,SmartCouponCategory b,SmartHospital c
                   where  b.ID in (select distinct CouponCategoryID from SmartCouponCategoryHospital where HospitalID=@HospitalID) and a.CategoryID=b.ID and a.Rest>0 and a.CustomerID=@CustomerID and a.HospitalID=c.ID and a.Status=@Status"
                   , new { CustomerID = customerID, HospitalID = hospitalID, Status = CouponStatus.Effective });

                var depositTemp = await multi.ReadAsync<CanCashierDeposits>();
                var couponTemp = await multi.ReadAsync<CanCashierCoupons>();

                var depositList = new List<CanCashierDeposits>();
                var couponList = new List<CanCashierCoupons>();

                var depositDic = new Dictionary<string, CanCashierDeposits>();
                var couponDic = new Dictionary<string, CanCashierCoupons>();

                foreach (var u in depositTemp)
                {
                    if (depositDic.ContainsKey(u.DepositChargeID))
                    {
                        depositList.Add(u);
                        //depositDic.Add(u.DepositChargeID, u);
                        continue;
                    }

                    if (u.ScopeLimit == 1)
                    {
                        depositList.Add(u);
                        depositDic.Add(u.DepositChargeID, u);
                        continue;
                    }
                    else if (u.ScopeLimit == 2)
                    {
                        var count = (await _connection.QueryAsync<int>(
                            @"with tree as
                            (
                               select ChargeCategoryID from SmartDepositChargeChargeCategory where DepositChargeID=@DepositChargeID 
                               union all
                               select a.ID from SmartChargeCategory a,tree b where a.ParentID=b.ChargeCategoryID
                            )
                            select count(ID) from SmartOrderDetail 
                            where ChargeID not in(select a.ID from SmartCharge a,tree b where a.CategoryID=b.ChargeCategoryID) and OrderID=@OrderID",
                            new { DepositChargeID = u.DepositChargeID, OrderID = orderID })).FirstOrDefault();
                        if (count == 0)
                        {
                            depositList.Add(u);
                            depositDic.Add(u.DepositChargeID, u);
                            continue;
                        }
                    }
                    else if (u.ScopeLimit == 3)
                    {
                        var count = (await _connection.QueryAsync<int>(
                            @"select count(ID) from SmartOrderDetail 
                            where ChargeID not in(select ChargeID from SmartDepositChargeCharge where DepositChargeID=@DepositChargeID) and OrderID=@OrderID",
                            new { DepositChargeID = u.DepositChargeID, OrderID = orderID })).FirstOrDefault();
                        if (count == 0)
                        {
                            depositList.Add(u);
                            depositDic.Add(u.DepositChargeID, u);
                            continue;
                        }
                    }
                }

                foreach (var u in couponTemp)
                {
                    if (couponDic.ContainsKey(u.CouponChargeID))
                    {
                        couponList.Add(u);
                        //couponDic.Add(u.CouponChargeID, u);
                        continue;
                    }

                    if (u.ScopeLimit == 1)
                    {
                        couponList.Add(u);
                        couponDic.Add(u.CouponChargeID, u);
                        continue;
                    }
                    else if (u.ScopeLimit == 2)
                    {
                        var count = (await _connection.QueryAsync<int>(
                            @"with tree as
                            (
                               select ChargeCategoryID from SmartCouponCategoryChargeCategory where [CouponCategoryID]=@CouponCategoryID 
                               union all
                               select a.ID from SmartChargeCategory a,tree b where a.ParentID=b.ChargeCategoryID
                            )
                            select count(ID) from SmartOrderDetail 
                            where ChargeID not in(select a.ID from SmartCharge a,tree b where a.CategoryID=b.ChargeCategoryID) and OrderID=@OrderID",
                            new { CouponCategoryID = u.CouponChargeID, OrderID = orderID })).FirstOrDefault();
                        if (count == 0)
                        {
                            couponList.Add(u);
                            couponDic.Add(u.CouponChargeID, u);
                            continue;
                        }
                    }
                    else if (u.ScopeLimit == 3)
                    {
                        var count = (await _connection.QueryAsync<int>(
                            @"select count(ID) from SmartOrderDetail 
                            where ChargeID not in(select ChargeID from SmartCouponCategoryCharge where CouponCategoryID=@CouponCategoryID) and OrderID=@OrderID",
                            new { CouponCategoryID = u.CouponChargeID, OrderID = orderID })).FirstOrDefault();
                        if (count == 0)
                        {
                            couponList.Add(u);
                            couponDic.Add(u.CouponChargeID, u);
                            continue;
                        }
                    }
                }

                result.Data = new CanCashier();
                result.Data.Deposits = depositList;
                result.Data.Coupons = couponList;

            });

            return result;
        }

        /// <summary>
        /// 待收费列表
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<NoPaidOrders>>> GetNoPaidOrders(long hospitalID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<NoPaidOrders>>();
            result.ResultType = IFlyDogResultType.Success;
            result.Message = "查询成功";

            await TryExecuteAsync(async () =>
            {
                result.Data = await _connection.QueryAsync<NoPaidOrders>(
                    @"select a.ID as OrderID,1 as OrderType,a.CustomerID,c.Name as CustomerName,b.Name as CreateUserName,a.CreateTime,a.AuditStatus,a.FinalPrice as Amount 
                    from SmartOrder a,SmartUser b,SmartCustomer c where a.CustomerID=c.ID and a.CreateUserID=b.ID and a.PaidStatus=@PaidStatus and a.HospitalID=@HospitalID and a.InpatientID is null
                    union all
                    select a.ID as OrderID,2 as OrderType,a.CustomerID,c.Name as CustomerName,b.Name as CreateUserName,a.CreateTime,a.AuditStatus,a.FinalPrice as Amount 
                    from SmartOrder a,SmartUser b,SmartCustomer c where a.CustomerID=c.ID and a.CreateUserID=b.ID and a.PaidStatus=@PaidStatus and a.HospitalID=@HospitalID and a.InpatientID is not null
                    union all
                    select a.ID as OrderID,3 as OrderType,a.CustomerID,c.Name as CustomerName,b.Name as CreateUserName,a.CreateTime,2,a.Amount 
                    from SmartDepositOrder a,SmartUser b,SmartCustomer c where a.CustomerID=c.ID and a.CreateUserID=b.ID and a.PaidStatus=@PaidStatus and a.HospitalID=@HospitalID  
                    union all
                    select a.ID as OrderID,4 as OrderType,a.CustomerID,c.Name as CustomerName,b.Name as CreateUserName,a.CreateTime,a.AuditStatus,a.Amount 
                    from SmartBackOrder a,SmartUser b,SmartCustomer c where a.CustomerID=c.ID and a.CreateUserID=b.ID and a.PaidStatus=@PaidStatus and a.HospitalID=@HospitalID  
                    union all
                    select a.ID as OrderID,5 as OrderType,a.CustomerID,c.Name as CustomerName,b.Name as CreateUserName,a.CreateTime,a.AuditStatus,a.Amount 
                    from SmartDepositRebateOrder a,SmartUser b,SmartCustomer c where a.CustomerID=c.ID and a.CreateUserID=b.ID and a.PaidStatus=@PaidStatus and a.HospitalID=@HospitalID 
                    order by CreateTime desc",
                    new { PaidStatus = PaidStatus.NotPaid, HospitalID = hospitalID });
            });

            return result;
        }

        /// <summary>
        /// 预收款收银
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> DepositOrderCashier(DepositCashierAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.CardList != null)
            {
                if (dto.CardList.Count() > dto.CardList.DistinctBy(u => u.CardCategoryID).Count())
                {
                    result.Message = "同一订单银行卡支付方式不能相通！";
                    return result;
                }
            }

            var options = _optionService.Get().Data;
            DateTime now = DateTime.Now;

            await TryTransactionAsync(async () =>
            {
                List<Task> tasks = new List<Task>();
                var deposits = await _connection.QueryAsync<DepositOrderTemp>(
                    @"select a.ID as OrderID,a.CustomerID,a.HospitalID,
                    a.Amount,a.PaidStatus,b.ChargeID,b.Total,b.Num,c.HasCoupon,c.CouponCategoryID,c.CouponAmount
                    from [SmartDepositOrder] a
                    inner join SmartDepositOrderDetail b on a.ID=b.OrderID
                    inner join SmartDepositCharge c on b.ChargeID=c.ID
                    where a.ID=@ID and a.CustomerID=@CustomerID", new { ID = dto.OrderID, CustomerID = dto.CustomerID }, _transaction);



                if (deposits == null || deposits.Count() == 0)
                {
                    result.Message = "订单不存在或者不属于该顾客！";
                    return false;
                }

                var depositFirst = deposits.FirstOrDefault();

                if (depositFirst.PaidStatus == PaidStatus.Paid || depositFirst.PaidStatus == PaidStatus.Debt)
                {
                    result.Message = "订单已经支付，无需重新支付！";
                    return false;
                }
                if (depositFirst.HospitalID != dto.HospitalID)
                {
                    result.Message = "对不起，该订单不属于您的医院！";
                    return false;
                }

                decimal coupon = 0;
                foreach (var u in deposits)
                {
                    tasks.Add(_connection.ExecuteAsync(
                        @"insert into [SmartDeposit]([ID],[HospitalID],[CustomerID],[CreateUserID],[CreateTime] ,[Access],[ChargeID],[Amount],[Rest],[Remark]) 
                        values(@ID,@HospitalID,@CustomerID,@CreateUserID,@CreateTime,@Access,@ChargeID,@Amount,@Rest,@Remark)",
                        new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            HospitalID = dto.HospitalID,
                            CustomerID = dto.CustomerID,
                            CreateUserID = dto.CreateUserID,
                            CreateTime = now,
                            Access = DepositType.Deposit,
                            ChargeID = u.ChargeID,
                            Amount = u.Total,
                            Rest = u.Total,
                            Remark = DepositType.Deposit.ToDescription() + "：" + dto.OrderID
                        }, _transaction));

                    if (u.HasCoupon == 1)
                    {
                        tasks.Add(_connection.ExecuteAsync(
                                  @"insert into [SmartCoupon]([ID],[HospitalID],[CustomerID],[CreateUserID],[CreateTime],[Access],[CategoryID],[Amount],[Rest],[Remark],Status) 
                                  values(@ID,@HospitalID,@CustomerID,@CreateUserID,@CreateTime,@Access,@CategoryID,@Amount,@Rest,@Remark,@Status)",
                                  new
                                  {
                                      ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                                      HospitalID = dto.HospitalID,
                                      CustomerID = dto.CustomerID,
                                      CreateUserID = dto.CreateUserID,
                                      CreateTime = now,
                                      Access = CouponType.DepositSend,
                                      CategoryID = u.CouponCategoryID,
                                      Amount = u.CouponAmount * u.Num,
                                      Rest = u.CouponAmount * u.Num,
                                      Remark = CouponType.DepositSend.ToDescription() + "：" + dto.OrderID,
                                      Status = CouponStatus.Effective
                                  }, _transaction));
                        coupon += (decimal)u.CouponAmount * u.Num;
                    }
                }

                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();

                decimal card = 0;
                foreach (var u in dto.CardList)
                {
                    tasks.Add(_connection.ExecuteAsync(
                        @"insert into [SmartCashierCardCatogoryDetail]([ID],[CashierID],[CardCategoryID],[Card]) 
                        values(@ID,@CashierID,@CardCategoryID,@Card)",
                        new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            CashierID = id,
                            CardCategoryID = u.CardCategoryID,
                            Card = u.Amount
                        }, _transaction));
                    card += u.Amount;
                }

                if (card + dto.Cash != depositFirst.Amount)
                {
                    result.Message = "金额不匹配，无法支付";
                    return false;
                }


                tasks.Add(_connection.ExecuteAsync(
                    @"update [SmartDepositOrder] set [PaidStatus]=@PaidStatus,PaidTime=@PaidTime where ID=@ID",
                    new { ID = dto.OrderID, PaidTime = now, PaidStatus = PaidStatus.Paid }, _transaction));


                decimal point = depositFirst.Amount * Convert.ToDecimal(options.IntegralNumValue);

                var customer = (await _connection.QueryAsync(
                   @"select MemberCategoryID,CashCardTotalAmount,FirstDealTime from SmartCustomer where ID=@ID", new { ID = depositFirst.CustomerID }, _transaction)).FirstOrDefault();
                var newMemberCategoryID = (await _connection.QueryAsync<long>(
                    @"select top 1 ID from [SmartMemberCategory] where Amount<@Amount order by Level desc", new { Amount = (decimal)customer.CashCardTotalAmount + depositFirst.Amount }, _transaction)).FirstOrDefault();

                string sql_where = "";
                if (customer.FirstDealTime == null)
                {
                    if (depositFirst.Amount > 0)
                    {
                        if (options.AdvanceSettingsValue == "1")
                        {
                            sql_where += ",FirstDealTime=@FirstDealTime";
                        }
                    }
                }
                if (newMemberCategoryID != (long)customer.MemberCategoryID)
                {
                    sql_where += ",MemberCategoryID=@MemberCategoryID";
                    tasks.Add(_connection.ExecuteAsync(
                        @"insert into [SmartMember]([ID],[CustomerID],[CreateTime],[CategoryID],[Remark],[CreateUserID]) 
                        values(@ID,@CustomerID,@CreateTime,@CategoryID,@Remark,@CreateUserID)", new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            CustomerID = dto.CustomerID,
                            CreateTime = DateTime.Now,
                            CategoryID = newMemberCategoryID,
                            Remark = "消费自动升级",
                            CreateUserID = 1
                        }, _transaction));
                }

                tasks.Add(_connection.ExecuteAsync(
                    string.Format(@"update [SmartCustomer] set Deposit=Deposit+@Deposit,Coupon=Coupon+@Coupon,Point=Point+@Point,CashCardTotalAmount=CashCardTotalAmount+@CashCardTotalAmount {0} where ID=@ID", sql_where)
                    , new
                    {
                        ID = dto.CustomerID,
                        Deposit = depositFirst.Amount,
                        Coupon = coupon,
                        Point = point,
                        CashCardTotalAmount = depositFirst.Amount,
                        FirstDealTime = now,
                        MemberCategoryID = newMemberCategoryID
                    }, _transaction));

                var cashierNo = GetCashierNo();
                tasks.Add(_connection.ExecuteAsync(
                    @"insert into [SmartCashier]([ID],[HospitalID],[CustomerID],[No],[OrderType],[OrderID],[CreateUserID],[CreateTime],[Amount],[Cash],[Card],[Deposit],[Coupon],[Debt],[Status],[Remark]) 
                    values(@ID,@HospitalID,@CustomerID,@No,@OrderType,@OrderID,@CreateUserID,@CreateTime,@Amount,@Cash,@Card,@Deposit,@Coupon,@Debt,@Status,@Remark)",
                    new
                    {
                        ID = id,
                        HospitalID = dto.HospitalID,
                        CustomerID = dto.CustomerID,
                        No = cashierNo,
                        OrderType = OrderType.Deposit,
                        OrderID = dto.OrderID,
                        CreateUserID = dto.CreateUserID,
                        CreateTime = now,
                        Amount = depositFirst.Amount,
                        Cash = dto.Cash,
                        Card = card,
                        Deposit = 0,
                        Coupon = 0,
                        Debt = 0,
                        Status = CashierStatus.No,
                        Remark = dto.Remark
                    }, _transaction));

                await Task.WhenAll(tasks);
                result.ResultType = IFlyDogResultType.Success;
                result.Message = "收银成功";
                return true;
            });
            return result;
        }

        /// <summary>
        /// 订单收银
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> OrderCashier(OrderCashierAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.CardList != null)
            {
                if (dto.CardList.Count() > dto.CardList.DistinctBy(u => u.CardCategoryID).Count())
                {
                    result.Message = "同一订单银行卡支付方式不能相通！";
                    return result;
                }
            }

            var options = _optionService.Get().Data;
            DateTime now = DateTime.Now;

            var canUse = (await GetCanCashier(dto.HospitalID, dto.CustomerID, dto.CreateUserID)).Data;

            await TryTransactionAsync(async () =>
            {
                List<Task> tasks = new List<Task>();
                var orders = (await _connection.QueryAsync<OrderTemp>(
                    @"select a.ID as OrderID,a.HospitalID,a.CustomerID,a.FinalPrice,a.PaidStatus,a.AuditStatus,b.ID as DetailID,b.ChargeID,b.FinalPrice as DetailFinalPrice,b.Num
                    from SmartOrder a,SmartOrderDetail b where a.ID=@ID and a.ID=b.OrderID order by b.FinalPrice", new { ID = dto.OrderID }, _transaction)).ToList();

                var orderFirst = orders.FirstOrDefault();

                if (orderFirst == null)
                {
                    result.Message = "订单不存在";
                    return false;
                }

                if (orderFirst.PaidStatus == PaidStatus.Paid || orderFirst.PaidStatus == PaidStatus.Debt)
                {
                    result.Message = "订单已经支付，无需重新支付！";
                    return false;
                }

                if (orderFirst.AuditStatus == AuditType.Pending)
                {
                    result.Message = "订单尚未审核，请先审核！";
                    return false;
                }
                else if (orderFirst.AuditStatus == AuditType.UnApprove)
                {
                    result.Message = "订单未通过审核，无法支付！";
                    return false;
                }

                if (orderFirst.HospitalID != dto.HospitalID)
                {
                    result.Message = "对不起，该订单不属于您的医院！";
                    return false;
                }



                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();

                var customer = (await _connection.QueryAsync(
                  @"select MemberCategoryID,CashCardTotalAmount,FirstDealTime,[Commission] from SmartCustomer where ID=@ID", new { ID = dto.CustomerID }, _transaction)).FirstOrDefault();
                if (dto.Commission > 0)
                {
                    if (dto.Commission > (decimal)customer.Commission)
                    {
                        result.Message = "对不起，佣金余额不足！";
                        return false;
                    }
                }

                decimal coupon = 0;
                foreach (var u in dto.CouponUseList)
                {
                    if (u.Amount <= 0)
                    {
                        result.Message = "券使用额度不能小于0！";
                        return false;
                    }
                    if (!canUse.Coupons.Any(n => n.CouponID.Equals(u.CardCategoryID.ToString()) && u.Amount <= n.Rest))
                    {
                        result.Message = "代金券" + u.CardCategoryID + "不能使用或者余额不足！";
                        return false;
                    }
                    coupon += u.Amount;
                    tasks.Add(_connection.ExecuteAsync(
                        @"update [SmartCoupon] set [Rest]=[Rest]-@Rest where [ID]=@ID",
                        new
                        {
                            ID = u.CardCategoryID,
                            Rest = u.Amount,
                            //Status = CouponStatus.Effective
                        }, _transaction));
                    tasks.Add(_connection.ExecuteAsync(
                        @"insert into [SmartCouponUsage]([ID],[CashierID],[CouponID],[Amount],[Type],[Remark])
                        values(@ID,@CashierID,@CouponID,@Amount,@Type,@Remark)",
                        new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            CashierID = id,
                            CouponID = u.CardCategoryID,
                            Amount = u.Amount,
                            Remark = CouponUsageType.OrderUse.ToDescription(),
                            Type = CouponUsageType.OrderUse
                        }, _transaction));
                }

                decimal deposit = 0;
                foreach (var u in dto.DepositUseList)
                {
                    if (u.Amount <= 0)
                    {
                        result.Message = "预收款使用额度不能小于0！";
                        return false;
                    }
                    if (!canUse.Deposits.Any(n => n.DepositID.Equals(u.CardCategoryID.ToString()) && u.Amount <= n.Rest))
                    {
                        result.Message = "预收款" + u.CardCategoryID + "不能使用或者余额不足！";
                        return false;
                    }
                    deposit += u.Amount;

                    tasks.Add(_connection.ExecuteAsync(
                        @"update [SmartDeposit] set [Rest]=[Rest]-@Rest where [ID]=@ID",
                        new
                        {
                            ID = u.CardCategoryID,
                            Rest = u.Amount
                        }, _transaction));
                    tasks.Add(_connection.ExecuteAsync(
                        @"insert into [SmartDepositUsage]([ID],[CashierID],[DepositID],[Amount])
                        values(@ID,@CashierID,@DepositID,@Amount)",
                        new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            CashierID = id,
                            DepositID = u.CardCategoryID,
                            Amount = u.Amount
                        }, _transaction));
                }


                var cashierNo = GetCashierNo();
                decimal card = 0;
                foreach (var u in dto.CardList)
                {
                    tasks.Add(_connection.ExecuteAsync(
                        @"insert into [SmartCashierCardCatogoryDetail]([ID],[CashierID],[CardCategoryID],[Card]) 
                        values(@ID,@CashierID,@CardCategoryID,@Card)",
                        new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            CashierID = id,
                            CardCategoryID = u.CardCategoryID,
                            Card = u.Amount
                        }, _transaction));
                    card += u.Amount;
                }

                decimal debt = 0;
                if (dto.IsDebt == 0)
                {
                    if (card + dto.Cash + deposit + coupon + dto.Commission != orderFirst.FinalPrice)
                    {
                        result.Message = "金额不匹配，无法支付";
                        return false;
                    }
                }
                else
                {
                    if (card + dto.Cash + deposit + coupon + dto.Commission > orderFirst.FinalPrice)
                    {
                        result.Message = "金额不匹配，无法支付";
                        return false;
                    }
                    debt = orderFirst.FinalPrice - card - dto.Cash - deposit - coupon - dto.Commission;
                }


                tasks.Add(_connection.ExecuteAsync(
                    @"insert into [SmartCashier]([ID],[HospitalID],[CustomerID],[No],[OrderType],[OrderID],[CreateUserID],[CreateTime],[Amount],[Cash],[Card],[Deposit],[Coupon],[Debt],[Status],[Remark],Commission) 
                    values(@ID,@HospitalID,@CustomerID,@No,@OrderType,@OrderID,@CreateUserID,@CreateTime,@Amount,@Cash,@Card,@Deposit,@Coupon,@Debt,@Status,@Remark,@Commission)",
                    new
                    {
                        ID = id,
                        HospitalID = dto.HospitalID,
                        CustomerID = orderFirst.CustomerID,
                        No = cashierNo,
                        OrderType = OrderType.Order,
                        OrderID = dto.OrderID,
                        CreateUserID = dto.CreateUserID,
                        CreateTime = now,
                        Amount = orderFirst.FinalPrice,
                        Cash = dto.Cash,
                        Card = card,
                        Deposit = deposit,
                        Coupon = coupon,
                        Debt = debt,
                        Status = CashierStatus.No,
                        Remark = dto.Remark,
                        Commission = dto.Commission
                    }, _transaction));


                decimal cashTemp = 0;
                decimal cardTemp = 0;
                decimal depositTemp = 0;
                decimal couponTemp = 0;
                decimal DebtTemp = 0;
                decimal commissionTemp = 0;

                for (int i = 1; i <= orders.Count(); i++)
                {
                    decimal a = 0; decimal b = 0; decimal c = 0; decimal d = 0; decimal e = 0; decimal f = 0;
                    if (orderFirst.FinalPrice > 0)
                    {
                        if (i == orders.Count())
                        {
                            a = dto.Cash - cashTemp;
                            b = card - cardTemp;
                            c = deposit - depositTemp;
                            d = coupon - couponTemp;
                            e = debt - DebtTemp;
                            f = dto.Commission - commissionTemp;
                        }
                        else
                        {
                            a = Math.Round(orders[i - 1].DetailFinalPrice * dto.Cash / orders[i - 1].FinalPrice, 0);
                            b = Math.Round(orders[i - 1].DetailFinalPrice * card / orders[i - 1].FinalPrice, 0);
                            c = Math.Round(orders[i - 1].DetailFinalPrice * deposit / orders[i - 1].FinalPrice, 0);
                            d = Math.Round(orders[i - 1].DetailFinalPrice * coupon / orders[i - 1].FinalPrice, 0);
                            e = Math.Round(orders[i - 1].DetailFinalPrice * debt / orders[i - 1].FinalPrice, 0);
                            f = Math.Round(orders[i - 1].DetailFinalPrice * dto.Commission / orders[i - 1].FinalPrice, 0);
                            cashTemp += a;
                            cardTemp += b;
                            depositTemp += c;
                            couponTemp += d;
                            DebtTemp += e;
                            commissionTemp += f;
                        }
                    }

                    tasks.Add(_connection.ExecuteAsync(
                        @"insert into [SmartCashierCharge]([ID],[CashierID],[ReferID],[CashAmount],[CardAmount],[DepositAmount],[CouponAmount],[DebtAmount],[Amount],[HospitalID],[CommissionAmount]) 
                        values(@ID,@CashierID,@ReferID,@CashAmount,@CardAmount,@DepositAmount,@CouponAmount,@DebtAmount,@Amount,@HospitalID,@CommissionAmount)",
                        new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            CashierID = id,
                            ReferID = orders[i - 1].ChargeID,
                            CashAmount = a,
                            CardAmount = b,
                            DepositAmount = c,
                            CouponAmount = d,
                            DebtAmount = e,
                            CommissionAmount = f,
                            Amount = orders[i - 1].DetailFinalPrice,
                            HospitalID = dto.HospitalID
                        }, _transaction));
                }

                tasks.Add(_connection.ExecuteAsync(
                    @"update [SmartOrder] set [PaidStatus]=@PaidStatus,PaidTime=@PaidTime,[DebtAmount]=@DebtAmount where ID=@ID",
                    new { ID = dto.OrderID, PaidTime = now, PaidStatus = debt > 0 ? PaidStatus.Debt : PaidStatus.Paid, DebtAmount = debt }, _transaction));


                decimal point = (orderFirst.FinalPrice) * Convert.ToDecimal(options.IntegralNumValue);



                var newMemberCategoryID = (await _connection.QueryAsync<long>(
                    @"select top 1 ID from [SmartMemberCategory] where Amount<@Amount order by Level desc", new { Amount = (decimal)customer.CashCardTotalAmount + orderFirst.FinalPrice }, _transaction)).FirstOrDefault();


                string sql_where = "";
                if (customer.FirstDealTime == null)
                {
                    if (orderFirst.FinalPrice > 0)
                    {
                        sql_where += ",FirstDealTime=@FirstDealTime";
                    }
                }
                if (newMemberCategoryID != (long)customer.MemberCategoryID)
                {
                    sql_where += ",MemberCategoryID=@MemberCategoryID";
                    tasks.Add(_connection.ExecuteAsync(
                        @"insert into [SmartMember]([ID],[CustomerID],[CreateTime],[CategoryID],[Remark],[CreateUserID]) 
                        values(@ID,@CustomerID,@CreateTime,@CategoryID,@Remark,@CreateUserID)", new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            CustomerID = dto.CustomerID,
                            CreateTime = DateTime.Now,
                            CategoryID = newMemberCategoryID,
                            Remark = "消费自动升级",
                            CreateUserID = 1
                        }, _transaction));
                }


                tasks.Add(_connection.ExecuteAsync(
                    string.Format(@"update [SmartCustomer] set Deposit=Deposit+@Deposit,Coupon=Coupon+@Coupon,Point=Point+@Point,Commission=@Commission,CashCardTotalAmount=CashCardTotalAmount+@CashCardTotalAmount {0} where ID=@ID", sql_where)
                    , new
                    {
                        ID = dto.CustomerID,
                        Deposit = deposit * -1,
                        Coupon = coupon * -1,
                        Point = point,
                        CashCardTotalAmount = orderFirst.FinalPrice,
                        FirstDealTime = now,
                        MemberCategoryID = newMemberCategoryID,
                        Commission = dto.Commission * -1
                    }, _transaction));

                await Task.WhenAll(tasks);
                result.ResultType = IFlyDogResultType.Success;
                result.Message = "收银成功";
                return true;
            });
            return result;
        }

        /// <summary>
        /// 欠款收银
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> DebtCashier(DebtCashierAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;
            DateTime now = DateTime.Now;

            if (dto.CardList != null)
            {
                if (dto.CardList.Count() > dto.CardList.DistinctBy(u => u.CardCategoryID).Count())
                {
                    result.Message = "同一订单银行卡支付方式不能相通！";
                    return result;
                }
            }

            await TryTransactionAsync(async () =>
            {
                List<Task> tasks = new List<Task>();
                var orders = (await _connection.QueryAsync<OrderTemp>(
                    @"select a.ID as OrderID,a.HospitalID,a.CustomerID,a.FinalPrice,a.DebtAmount,a.PaidStatus,a.AuditStatus,b.ID as DetailID,b.ChargeID,b.FinalPrice as DetailFinalPrice,b.Num
                    from SmartOrder a,SmartOrderDetail b where a.ID=@ID and a.ID=b.OrderID order by b.FinalPrice", new { ID = dto.OrderID }, _transaction)).ToList();

                var orderFirst = orders.FirstOrDefault();

                if (orderFirst == null)
                {
                    result.Message = "订单不存在";
                    return false;
                }

                if (orderFirst.PaidStatus != PaidStatus.Debt)
                {
                    result.Message = "对不起，该订单不是欠款状态！";
                    return false;
                }

                if (orderFirst.AuditStatus == AuditType.Pending)
                {
                    result.Message = "订单尚未审核，请先审核！";
                    return false;
                }
                else if (orderFirst.AuditStatus == AuditType.UnApprove)
                {
                    result.Message = "订单未通过审核，无法支付！";
                    return false;
                }

                if (orderFirst.HospitalID != dto.HospitalID)
                {
                    result.Message = "对不起，该订单不属于您的医院！";
                    return false;
                }
                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();

                //var customer = (await _connection.QueryAsync(
                //  @"select MemberCategoryID,CashCardTotalAmount,FirstDealTime,[Commission] from SmartCustomer where ID=@ID", new { ID = dto.CustomerID }, _transaction)).FirstOrDefault();

                var cashierNo = GetCashierNo();
                decimal card = 0;

                foreach (var u in dto.CardList)
                {
                    tasks.Add(_connection.ExecuteAsync(
                        @"insert into [SmartCashierCardCatogoryDetail]([ID],[CashierID],[CardCategoryID],[Card]) 
                        values(@ID,@CashierID,@CardCategoryID,@Card)",
                        new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            CashierID = id,
                            CardCategoryID = u.CardCategoryID,
                            Card = u.Amount
                        }, _transaction));
                    card += u.Amount;
                }

                decimal debt = 0;
                if (dto.IsDebt == 0)
                {
                    if (card + dto.Cash != orderFirst.DebtAmount)
                    {
                        result.Message = "金额不匹配，无法支付";
                        return false;
                    }
                }
                else
                {
                    if (card + dto.Cash >= orderFirst.DebtAmount)
                    {
                        result.Message = "金额不匹配，无法支付";
                        return false;
                    }
                    debt = orderFirst.DebtAmount - card - dto.Cash;
                }


                tasks.Add(_connection.ExecuteAsync(
                    @"insert into [SmartCashier]([ID],[HospitalID],[CustomerID],[No],[OrderType],[OrderID],[CreateUserID],[CreateTime],[Amount],[Cash],[Card],[Deposit],[Coupon],[Debt],[Status],[Remark],Commission) 
                    values(@ID,@HospitalID,@CustomerID,@No,@OrderType,@OrderID,@CreateUserID,@CreateTime,@Amount,@Cash,@Card,@Deposit,@Coupon,@Debt,@Status,@Remark,@Commission)",
                    new
                    {
                        ID = id,
                        HospitalID = dto.HospitalID,
                        CustomerID = orderFirst.CustomerID,
                        No = cashierNo,
                        OrderType = OrderType.Debt,
                        OrderID = dto.OrderID,
                        CreateUserID = dto.CreateUserID,
                        CreateTime = now,
                        Amount = orderFirst.DebtAmount,
                        Cash = dto.Cash,
                        Card = card,
                        Deposit = 0,
                        Coupon = 0,
                        Debt = debt,
                        Status = CashierStatus.No,
                        Remark = dto.Remark,
                        Commission = 0
                    }, _transaction));


                decimal cashTemp = 0;
                decimal cardTemp = 0;
                //decimal depositTemp = 0;
                //decimal couponTemp = 0;
                decimal DebtTemp = 0;
                //decimal commissionTemp = 0;
                decimal detailTemp = 0;

                for (int i = 1; i <= orders.Count(); i++)
                {
                    decimal a = 0; decimal b = 0; decimal c = 0; decimal d = 0; decimal e = 0; decimal f = 0; decimal g = 0;
                    if (orderFirst.FinalPrice > 0)
                    {
                        if (i == orders.Count())
                        {
                            a = dto.Cash - cashTemp;
                            b = card - cardTemp;
                            //c = deposit - depositTemp;
                            //d = coupon - couponTemp;
                            e = debt - DebtTemp;
                            g = orders[i - 1].DebtAmount - detailTemp;
                            //f = dto.Commission - commissionTemp;
                        }
                        else
                        {
                            a = Math.Round(orders[i - 1].DetailFinalPrice * dto.Cash / orders[i - 1].FinalPrice, 0);
                            b = Math.Round(orders[i - 1].DetailFinalPrice * card / orders[i - 1].FinalPrice, 0);
                            //c = Math.Round(orders[i - 1].DetailFinalPrice * deposit / orders[i - 1].FinalPrice, 0);
                            //d = Math.Round(orders[i - 1].DetailFinalPrice * coupon / orders[i - 1].FinalPrice, 0);
                            e = Math.Round(orders[i - 1].DetailFinalPrice * debt / orders[i - 1].FinalPrice, 0);
                            //f = Math.Round(orders[i - 1].DetailFinalPrice * dto.Commission / orders[i - 1].FinalPrice, 0);
                            g = Math.Round(orders[i - 1].DetailFinalPrice * orders[i - 1].DebtAmount / orders[i - 1].FinalPrice, 0);
                            cashTemp += a;
                            cardTemp += b;
                            //depositTemp += c;
                            //couponTemp += d;
                            DebtTemp += e;
                            detailTemp += g;
                            //commissionTemp += f;
                        }
                    }

                    tasks.Add(_connection.ExecuteAsync(
                        @"insert into [SmartCashierCharge]([ID],[CashierID],[ReferID],[CashAmount],[CardAmount],[DepositAmount],[CouponAmount],[DebtAmount],[Amount],[HospitalID],[CommissionAmount]) 
                        values(@ID,@CashierID,@ReferID,@CashAmount,@CardAmount,@DepositAmount,@CouponAmount,@DebtAmount,@Amount,@HospitalID,@CommissionAmount)",
                        new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            CashierID = id,
                            ReferID = orders[i - 1].ChargeID,
                            CashAmount = a,
                            CardAmount = b,
                            DepositAmount = c,
                            CouponAmount = d,
                            DebtAmount = e,
                            CommissionAmount = f,
                            Amount = detailTemp,
                            HospitalID = dto.HospitalID
                        }, _transaction));
                }

                tasks.Add(_connection.ExecuteAsync(
                    @"update [SmartOrder] set [PaidStatus]=@PaidStatus,PaidTime=@PaidTime,[DebtAmount]=@DebtAmount where ID=@ID",
                    new { ID = dto.OrderID, PaidTime = now, PaidStatus = debt > 0 ? PaidStatus.Debt : PaidStatus.Paid, DebtAmount = debt }, _transaction));


                await Task.WhenAll(tasks);
                result.ResultType = IFlyDogResultType.Success;
                result.Message = "收银成功";
                return true;
            });
            return result;
        }

        /// <summary>
        /// 退款收银
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> DepositRebateOrderCashier(DepositRebateCashierAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            var options = _optionService.Get().Data;
            DateTime now = DateTime.Now;
            var depositRebateOrder = (await _depositRebateOrder.GetDetail(dto.CreateUserID, dto.CustomerID, dto.OrderID)).Data;
            var CanRebate = (await _depositRebateOrder.GetCanRebate(dto.HospitalID, dto.CustomerID)).Data;


            await TryTransactionAsync(async () =>
            {
                List<Task> tasks = new List<Task>();


                if (depositRebateOrder == null)
                {
                    result.Message = "订单不存在或者不属于该顾客！";
                    return false;
                }

                if (depositRebateOrder.PaidStatus == PaidStatus.Paid)
                {
                    result.Message = "订单已经支付，无需重新支付！";
                    return false;
                }
                if (depositRebateOrder.HospitalID != dto.HospitalID.ToString())
                {
                    result.Message = "对不起，该订单不属于您的医院！";
                    return false;
                }
                if (depositRebateOrder.AuditStatus == AuditType.Pending)
                {
                    result.Message = "订单尚未审核，请先审核！";
                    return false;
                }
                else if (depositRebateOrder.AuditStatus == AuditType.UnApprove)
                {
                    result.Message = "订单未通过审核，无法支付！";
                    return false;
                }

                if (dto.CardCategoryID != null && dto.CardCategoryID > 0)
                {
                    if (depositRebateOrder.Amount != dto.Card + dto.Cash)
                    {
                        result.Message = "对不起，金额不匹配，无法支付！";
                        return false;
                    }
                }
                else
                {
                    if (depositRebateOrder.Amount != dto.Cash)
                    {
                        result.Message = "对不起，金额不匹配，无法支付！";
                        return false;
                    }
                }


                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();

                foreach (var u in depositRebateOrder.Details)
                {
                    if (!CanRebate.Deposits.Any(n => n.DepositID == u.DepositID && n.Rest >= u.Amount))
                    {
                        result.Message = "预收款" + u.DepositName + "余额不足或者不存在！";
                        return false;
                    }

                    tasks.Add(_connection.ExecuteAsync(
                        @"update [SmartDeposit] set [Rest]=[Rest]-@Rest where [ID]=@ID",
                        new
                        {
                            ID = u.DepositID,
                            Rest = u.Amount
                        }, _transaction));
                    tasks.Add(_connection.ExecuteAsync(
                        @"insert into [SmartDepositUsage]([ID],[CashierID],[DepositID],[Amount])
                        values(@ID,@CashierID,@DepositID,@Amount)",
                        new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            CashierID = id,
                            DepositID = u.DepositID,
                            Amount = u.Amount
                        }, _transaction));
                }

                foreach (var u in depositRebateOrder.CouponDetails)
                {
                    if (!CanRebate.Coupons.Any(n => n.CouponID == u.CouponID && n.Rest >= u.Amount))
                    {
                        result.Message = "代金券" + u.CouponName + "余额不足或者不存在！";
                        return false;
                    }

                    //var t = CanRebate.Coupons.Where(n => n.CouponID == u.CouponID && n.Rest >= u.Amount).FirstOrDefault();

                    //if (t == null)
                    //{
                    //    result.Message = "代金券" + u.CouponName + "余额不足或者不存在！";
                    //    return false;
                    //}

                    tasks.Add(_connection.ExecuteAsync(
                        @"update [SmartCoupon] set [Rest]=[Rest]-@Rest where [ID]=@ID",
                        new
                        {
                            ID = u.CouponID,
                            Rest = u.Amount,
                            //Status = t.Rest == u.Amount ? CouponStatus.Use : CouponStatus.Effective
                        }, _transaction));
                    tasks.Add(_connection.ExecuteAsync(
                        @"insert into [SmartCouponUsage]([ID],[CashierID],[CouponID],[Amount])
                        values(@ID,@CashierID,@CouponID,@Amount)",
                        new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            CashierID = id,
                            CouponID = u.CouponID,
                            Amount = u.Amount
                        }, _transaction));
                }

                tasks.Add(_connection.ExecuteAsync(
                    @"update [SmartDepositRebateOrder] set [PaidStatus]=@PaidStatus,PaidTime=@PaidTime where ID=@ID",
                    new { ID = dto.OrderID, PaidTime = now, PaidStatus = PaidStatus.Paid }, _transaction));

                var customer = (await _connection.QueryAsync(
                   @"select MemberCategoryID,CashCardTotalAmount,FirstDealTime from SmartCustomer where ID=@ID", new { ID = dto.CustomerID }, _transaction)).FirstOrDefault();

                long newMemberCategoryID = 1;
                if ((decimal)customer.CashCardTotalAmount - depositRebateOrder.Amount > 0)
                {
                    newMemberCategoryID = (await _connection.QueryAsync<long>(
                    @"select top 1 ID from [SmartMemberCategory] where Amount<@Amount order by Level desc", new { Amount = (decimal)customer.CashCardTotalAmount - depositRebateOrder.Amount }, _transaction)).FirstOrDefault();

                }

                string sql_where = "";

                if (newMemberCategoryID != (long)customer.MemberCategoryID)
                {
                    sql_where += ",MemberCategoryID=@MemberCategoryID";
                    tasks.Add(_connection.ExecuteAsync(
                        @"insert into [SmartMember]([ID],[CustomerID],[CreateTime],[CategoryID],[Remark],[CreateUserID]) 
                        values(@ID,@CustomerID,@CreateTime,@CategoryID,@Remark,@CreateUserID)", new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            CustomerID = dto.CustomerID,
                            CreateTime = DateTime.Now,
                            CategoryID = newMemberCategoryID,
                            Remark = "退款自动降级",
                            CreateUserID = 1
                        }, _transaction));
                }

                tasks.Add(_connection.ExecuteAsync(
                    string.Format(@"update [SmartCustomer] set Deposit=Deposit+@Deposit,Coupon=Coupon+@Coupon,Point=Point+@Point,CashCardTotalAmount=CashCardTotalAmount+@CashCardTotalAmount {0} where ID=@ID", sql_where)
                    , new
                    {
                        ID = dto.CustomerID,
                        Deposit = depositRebateOrder.Deposit * -1,
                        Coupon = depositRebateOrder.Coupon * -1,
                        Point = depositRebateOrder.Point * -1,
                        CashCardTotalAmount = depositRebateOrder.Amount * -1,
                        MemberCategoryID = newMemberCategoryID
                    }, _transaction));

                var cashierNo = GetCashierNo();
                tasks.Add(_connection.ExecuteAsync(
                    @"insert into [SmartCashier]([ID],[HospitalID],[CustomerID],[No],[OrderType],[OrderID],[CreateUserID],[CreateTime],[Amount],[Cash],[Card],[Deposit],[Coupon],[Debt],[Status],[Remark]) 
                    values(@ID,@HospitalID,@CustomerID,@No,@OrderType,@OrderID,@CreateUserID,@CreateTime,@Amount,@Cash,@Card,@Deposit,@Coupon,@Debt,@Status,@Remark)",
                    new
                    {
                        ID = id,
                        HospitalID = dto.HospitalID,
                        CustomerID = dto.CustomerID,
                        No = cashierNo,
                        OrderType = OrderType.Refund,
                        OrderID = dto.OrderID,
                        CreateUserID = dto.CreateUserID,
                        CreateTime = now,
                        Amount = depositRebateOrder.Amount * -1,
                        Cash = dto.Cash * -1,
                        Card = dto.Card * -1,
                        Deposit = depositRebateOrder.Deposit * -1,
                        Coupon = depositRebateOrder.Coupon * -1,
                        Debt = 0,
                        Status = CashierStatus.No,
                        Remark = dto.Remark
                    }, _transaction));

                if (dto.CardCategoryID != null && dto.CardCategoryID > 0 && dto.Card > 0)
                {
                    tasks.Add(_connection.ExecuteAsync(
                        @"insert into [SmartCashierCardCatogoryDetail]([ID],[CashierID],[CardCategoryID],[Card]) 
                        values(@ID,@CashierID,@CardCategoryID,@Card)",
                        new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            CashierID = id,
                            CardCategoryID = dto.CardCategoryID,
                            Card = dto.Card * -1
                        }, _transaction));
                }

                await Task.WhenAll(tasks);
                result.ResultType = IFlyDogResultType.Success;
                result.Message = "收银成功";
                return true;
            });
            return result;
        }

        /// <summary>
        /// 退项目单
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> BackOrderCashier(BackCashierAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            var options = _optionService.Get().Data;
            DateTime now = DateTime.Now;
            //var CanRebate = (await _depositRebateOrder.GetCanRebate(dto.HospitalID, dto.CustomerID)).Data;
            var noDoneOrders = (await _orderService.GetNoDoneOrders(dto.HospitalID, dto.CustomerID)).Data;

            if (dto.CardCategoryID == null || dto.CardCategoryID == 0)
            {
                dto.Card = 0;
            }
            if (dto.DepositChargeID == null || dto.DepositChargeID == 0)
            {
                dto.Deposit = 0;
            }
            if (dto.CouponCategoryID == null || dto.CouponCategoryID == 0)
            {
                dto.Coupon = 0;
            }

            await TryTransactionAsync(async () =>
            {
                List<Task> tasks = new List<Task>();
                var backorders = (await _connection.QueryAsync<BackOrderTemp>(
                    @"select a.ID as OrderID,a.HospitalID,a.CustomerID,a.Amount,a.Point,a.PaidStatus,a.AuditStatus,b.DetailID,b.ChargeID,b.Amount as DetailAmount,b.Num
                    from SmartBackOrder a
                    inner join SmartBackOrderDetail b on a.ID=b.OrderID
                    where a.ID=@ID order by b.Amount", new { ID = dto.OrderID }, _transaction)).ToList();

                var backorderFirst = backorders.FirstOrDefault();

                if (backorderFirst == null)
                {
                    result.Message = "订单不存在";
                    return false;
                }

                if (backorderFirst.PaidStatus == PaidStatus.Paid)
                {
                    result.Message = "订单已经支付，无需重新支付！";
                    return false;
                }

                if (backorderFirst.AuditStatus == AuditType.Pending)
                {
                    result.Message = "订单尚未审核，请先审核！";
                    return false;
                }
                else if (backorderFirst.AuditStatus == AuditType.UnApprove)
                {
                    result.Message = "订单未通过审核，无法支付！";
                    return false;
                }

                if (backorderFirst.HospitalID != dto.HospitalID)
                {
                    result.Message = "对不起，该订单不属于您的医院！";
                    return false;
                }

                decimal totalAmount = 0;
                if (dto.CardCategoryID != null && dto.CardCategoryID > 0)
                {
                    totalAmount += dto.Card;
                }
                if (dto.DepositChargeID != null && dto.DepositChargeID > 0)
                {
                    totalAmount += dto.Deposit;
                }
                if (dto.CouponCategoryID != null && dto.CouponCategoryID > 0)
                {
                    totalAmount += dto.Coupon;
                }

                if (totalAmount + dto.Cash != backorderFirst.Amount)
                {
                    result.Message = " 金额不匹配，无法支付!";
                    return false;
                }

                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();

                //decimal coupon = 0;
                foreach (var u in backorders)
                {

                    if (!noDoneOrders.Any(n => n.DetailID == u.DetailID.ToString() && u.Num <= n.RestNum))
                    {
                        result.Message = "项目剩余次数不足！";
                        return false;
                    }
                    //coupon += u.Amount;
                    tasks.Add(_connection.ExecuteAsync(
                        @"update [SmartOrderDetail] set [RestNum]=[RestNum]-@Num where [ID]=@ID",
                        new
                        {
                            ID = u.DetailID,
                            Num = u.Num,
                        }, _transaction));
                }

                var cashierNo = GetCashierNo();

                tasks.Add(_connection.ExecuteAsync(
                    @"insert into [SmartCashier]([ID],[HospitalID],[CustomerID],[No],[OrderType],[OrderID],[CreateUserID],[CreateTime],[Amount],[Cash],[Card],[Deposit],[Coupon],[Debt],[Status],[Remark]) 
                    values(@ID,@HospitalID,@CustomerID,@No,@OrderType,@OrderID,@CreateUserID,@CreateTime,@Amount,@Cash,@Card,@Deposit,@Coupon,@Debt,@Status,@Remark)",
                    new
                    {
                        ID = id,
                        HospitalID = dto.HospitalID,
                        CustomerID = dto.CustomerID,
                        No = cashierNo,
                        OrderType = OrderType.Order,
                        OrderID = dto.OrderID,
                        CreateUserID = dto.CreateUserID,
                        CreateTime = now,
                        Amount = backorderFirst.Amount * -1,
                        Cash = dto.Cash * -1,
                        Card = dto.Card * -1,
                        Deposit = dto.Deposit * -1,
                        Coupon = dto.Coupon * -1,
                        Debt = 0,
                        Status = CashierStatus.No,
                        Remark = dto.Remark
                    }, _transaction));

                if (dto.CardCategoryID != null && dto.CardCategoryID > 0 && dto.Card > 0)
                {
                    tasks.Add(_connection.ExecuteAsync(
                       @"insert into [SmartCashierCardCatogoryDetail]([ID],[CashierID],[CardCategoryID],[Card]) 
                        values(@ID,@CashierID,@CardCategoryID,@Card)",
                       new
                       {
                           ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                           CashierID = id,
                           CardCategoryID = dto.CardCategoryID,
                           Card = dto.Card * -1
                       }, _transaction));
                }


                decimal cashTemp = 0;
                decimal cardTemp = 0;
                decimal depositTemp = 0;
                decimal couponTemp = 0;
                //decimal DebtTemp = 0;

                for (int i = 1; i <= backorders.Count(); i++)
                {
                    decimal a = 0; decimal b = 0; decimal c = 0; decimal d = 0;
                    if (backorderFirst.Amount > 0)
                    {
                        if (i == backorders.Count())
                        {
                            a = dto.Cash - cashTemp;
                            b = dto.Card - cardTemp;
                            c = dto.Deposit - depositTemp;
                            d = dto.Coupon - couponTemp;
                            //e = debt - DebtTemp;
                        }
                        else
                        {
                            a = Math.Round(backorders[i - 1].DetailAmount * dto.Cash / backorders[i - 1].Amount, 0);
                            b = Math.Round(backorders[i - 1].DetailAmount * dto.Card / backorders[i - 1].Amount, 0);
                            c = Math.Round(backorders[i - 1].DetailAmount * dto.Deposit / backorders[i - 1].Amount, 0);
                            d = Math.Round(backorders[i - 1].DetailAmount * dto.Coupon / backorders[i - 1].Amount, 0);
                            //e = Math.Round(backorders[i - 1].DetailFinalPrice * debt / backorders[i - 1].FinalPrice, 0);
                            cashTemp += a;
                            cardTemp += b;
                            depositTemp += c;
                            couponTemp += d;
                        }
                    }

                    tasks.Add(_connection.ExecuteAsync(
                        @"insert into [SmartCashierCharge]([ID],[CashierID],[ReferID],[CashAmount],[CardAmount],[DepositAmount],[CouponAmount],[DebtAmount],[Amount],[HospitalID]) 
                        values(@ID,@CashierID,@ReferID,@CashAmount,@CardAmount,@DepositAmount,@CouponAmount,@DebtAmount,@Amount,@HospitalID)",
                        new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            CashierID = id,
                            ReferID = backorders[i - 1].ChargeID,
                            CashAmount = a,
                            CardAmount = b,
                            DepositAmount = c,
                            CouponAmount = d,
                            DebtAmount = 0,
                            Amount = backorders[i - 1].DetailAmount,
                            HospitalID = dto.HospitalID
                        }, _transaction));
                }

                if (dto.Deposit > 0)
                {
                    tasks.Add(_connection.ExecuteAsync(
                       @"insert into [SmartDeposit]([ID],[HospitalID],[CustomerID],[CreateUserID],[CreateTime] ,[Access],[ChargeID],[Amount],[Rest],[Remark]) 
                        values(@ID,@HospitalID,@CustomerID,@CreateUserID,@CreateTime,@Access,@ChargeID,@Amount,@Rest,@Remark)",
                       new
                       {
                           ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                           HospitalID = dto.HospitalID,
                           CustomerID = dto.CustomerID,
                           CreateUserID = dto.CreateUserID,
                           CreateTime = now,
                           Access = DepositType.Back,
                           ChargeID = dto.DepositChargeID,
                           Amount = dto.Deposit,
                           Rest = dto.Deposit,
                           Remark = DepositType.Back.ToDescription() + "：" + dto.OrderID
                       }, _transaction));
                }
                if (dto.Coupon > 0)
                {
                    tasks.Add(_connection.ExecuteAsync(
                                  @"insert into [SmartCoupon]([ID],[HospitalID],[CustomerID],[CreateUserID],[CreateTime],[Access],[CategoryID],[Amount],[Rest],[Remark],Status) 
                                  values(@ID,@HospitalID,@CustomerID,@CreateUserID,@CreateTime,@Access,@CategoryID,@Amount,@Rest,@Remark,@Status)",
                                  new
                                  {
                                      ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                                      HospitalID = dto.HospitalID,
                                      CustomerID = dto.CustomerID,
                                      CreateUserID = dto.CreateUserID,
                                      CreateTime = now,
                                      Access = CouponType.BackSed,
                                      CategoryID = dto.CouponCategoryID,
                                      Amount = dto.Coupon,
                                      Rest = dto.Coupon,
                                      Remark = CouponType.BackSed.ToDescription() + "：" + dto.OrderID,
                                      Status = CouponStatus.Effective
                                  }, _transaction));
                }

                tasks.Add(_connection.ExecuteAsync(
                    @"update [SmartBackOrder] set [PaidStatus]=@PaidStatus,PaidTime=@PaidTime where ID=@ID",
                    new { ID = dto.OrderID, PaidTime = now, PaidStatus = PaidStatus.Paid }, _transaction));


                //decimal point = (dto.Cash + card) * Convert.ToDecimal(options.IntegralNumValue);


                var customer = (await _connection.QueryAsync(
                  @"select MemberCategoryID,CashCardTotalAmount,FirstDealTime from SmartCustomer where ID=@ID", new { ID = dto.CustomerID }, _transaction)).FirstOrDefault();
                long newMemberCategoryID = 1;
                if ((decimal)customer.CashCardTotalAmount - dto.Card - dto.Cash > 0)
                {
                    newMemberCategoryID = (await _connection.QueryAsync<long>(
                    @"select top 1 ID from [SmartMemberCategory] where Amount<@Amount order by Level desc", new { Amount = (decimal)customer.CashCardTotalAmount - dto.Card - dto.Cash }, _transaction)).FirstOrDefault();

                }

                string sql_where = "";

                if (newMemberCategoryID != (long)customer.MemberCategoryID)
                {
                    sql_where += ",MemberCategoryID=@MemberCategoryID";
                    tasks.Add(_connection.ExecuteAsync(
                        @"insert into [SmartMember]([ID],[CustomerID],[CreateTime],[CategoryID],[Remark],[CreateUserID]) 
                        values(@ID,@CustomerID,@CreateTime,@CategoryID,@Remark,@CreateUserID)", new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            CustomerID = dto.CustomerID,
                            CreateTime = DateTime.Now,
                            CategoryID = newMemberCategoryID,
                            Remark = "退项目自动降级",
                            CreateUserID = 1
                        }, _transaction));
                }


                tasks.Add(_connection.ExecuteAsync(
                    string.Format(@"update [SmartCustomer] set Deposit=Deposit+@Deposit,Coupon=Coupon+@Coupon,Point=Point+@Point,CashCardTotalAmount=CashCardTotalAmount+@CashCardTotalAmount {0} where ID=@ID", sql_where)
                    , new
                    {
                        ID = dto.CustomerID,
                        Deposit = dto.Deposit,
                        Coupon = dto.Coupon,
                        Point = backorderFirst.Point * -1,
                        CashCardTotalAmount = dto.Deposit - dto.Cash - dto.Card,
                        MemberCategoryID = newMemberCategoryID
                    }, _transaction));

                await Task.WhenAll(tasks);
                result.ResultType = IFlyDogResultType.Success;
                result.Message = "收银成功";
                return true;
            });
            return result;
        }

        /// <summary>
        /// 获取更新收银详细信息
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <param name="cashierID"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, CashierUpdateInfo>> GetCashierUpdateInfo(long cashierID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, CashierUpdateInfo>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            await TryExecuteAsync(async () =>
            {
                var multi = await _connection.QueryAsync(
                   @"select a.ID,a.HospitalID,a.CustomerID,a.Card,a.Cash,b.CardCategoryID,b.Card
                   from SmartCashier a
                   inner join SmartCashierCardCatogoryDetail b on a.ID=b.CashierID
                   where a.ID=@ID"
                   , new { ID = cashierID });

                var list = new Dictionary<string, CashierUpdateInfo>();
                await _connection.QueryAsync<CashierUpdateInfo, CardCashier, CashierUpdateInfo>(
                    @"select a.ID,a.HospitalID,a.CustomerID,a.Card,a.Cash,b.CardCategoryID,b.Card  as Amount
                    from SmartCashier a
                    inner join SmartCashierCardCatogoryDetail b on a.ID=b.CashierID
                    where a.ID=@ID",
                    (cashier, card) =>
                    {
                        CashierUpdateInfo temp = new CashierUpdateInfo();
                        if (!list.TryGetValue(cashier.ID, out temp))
                        {
                            list.Add(cashier.ID, temp = cashier);
                        }
                        if (card != null)
                            temp.CardList.Add(card);
                        return cashier;
                    }, new { ID = cashierID }, null, true, splitOn: "CardCategoryID");

                result.Data = list.Values.FirstOrDefault();

            });

            return result;
        }


        /// <summary>
        /// 收银单号
        /// </summary>
        /// <returns></returns>
        private string GetCashierNo()
        {
            //DateTime time = DateTime.Today;
            //string month = time.Month < 10 ? "0" + time.Month : time.Month.ToString();
            //string day = time.Day < 10 ? "0" + time.Day : time.Day.ToString();
            //string temp = "PN" + time.Year.ToString().Substring(2) + month + day;
            //var number = _redis.StringIncrement(RedisPreKey.CashierID, 1);

            //if (number < 10)
            //    temp += "0000" + number;
            //else if (number < 100)
            //    temp += "000" + number;
            //else if (number < 1000)
            //    temp += "00" + number;
            //else if (number < 10000)
            //    temp += "0" + number;
            //else
            //    temp += number;
            string temp = "PN";
            var number = _redis.StringIncrement(RedisPreKey.CashierID, 1);

            int n = 10;
            for (int i = 0; i < n - number.ToString().Length; i++)
            {
                temp += "0";
            }

            temp += number;

            return temp;
        }
    }
}
