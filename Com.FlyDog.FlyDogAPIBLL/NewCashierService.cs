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
    public class NewCashierService : BaseService, ICashierService
    {
        //private IDepositService _depositService;
        private IOptionService _optionService;
        private RedisStackExchangeHelper _redis = new RedisStackExchangeHelper();
        private IOrderService _orderService;
        private IDepositRebateOrderService _depositRebateOrder;
        public NewCashierService(IOptionService optionService, IDepositRebateOrderService depositRebateOrder, IOrderService orderService)
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

                result.Data = (await _connection.QueryAsync<CanCashier>(
                    @"select ID as CustomerID,[Commission] from [SmartCustomer] where ID=@ID", new { ID = customerID })).FirstOrDefault();
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

            dto.CardList = dto.CardList.Where(u => u.CardCategoryID > 0 && u.Amount > 0);

            if (dto.Cash < 0)
            {
                result.Message = "现金不能为负！";
                return result;
            }

            foreach (var u in dto.CardList)
            {
                if (u.Amount < 0)
                {
                    result.Message = "刷卡不能为负！";
                    return result;
                }
            }

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
                if (point > 0)
                {
                    tasks.Add(_connection.ExecuteAsync(
                    @"insert into [SmartPoint]([ID],[CustomerID],[CreateUserID],[CreateTime],[Type],[Amount],[Remark],[ConsumeAmount],[HospitalID])
                    values(@ID,@CustomerID,@CreateUserID,@CreateTime,@Type,@Amount,@Remark,@ConsumeAmount,@HospitalID)",
                    new
                    {
                        ID = id,
                        CustomerID = dto.CustomerID,
                        CreateUserID = dto.CreateUserID,
                        CreateTime = now,
                        Type = PointType.ConsumeGive,
                        Amount = point,
                        Remark = PointType.ConsumeGive.ToDescription() + ":" + dto.OrderID,
                        ConsumeAmount = depositFirst.Amount,
                        HospitalID = dto.HospitalID
                    }, _transaction));
                }


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

            if (dto.Cash < 0)
            {
                result.Message = "现金不能为负！";
                return result;
            }

            foreach (var u in dto.CardList)
            {
                if (u.Amount < 0)
                {
                    result.Message = "刷卡不能为负！";
                    return result;
                }
            }

            if (dto.Commission < 0)
            {
                result.Message = "佣金不能为负！";
                return result;
            }

            dto.CardList = dto.CardList.Where(u => u.CardCategoryID > 0 && u.Amount > 0);

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
                        @"insert into [SmartCouponUsage]([ID],[CashierID],[CouponID],[Amount],[Type],[Remark],CreateUserID)
                        values(@ID,@CashierID,@CouponID,@Amount,@Type,@Remark,@CreateUserID)",
                        new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            CashierID = id,
                            CouponID = u.CardCategoryID,
                            Amount = u.Amount,
                            Remark = CouponUsageType.OrderUse.ToDescription(),
                            Type = CouponUsageType.OrderUse,
                            CreateUserID = dto.CreateUserID
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
                    if (card + dto.Cash + deposit + coupon + dto.Commission >= orderFirst.FinalPrice)
                    {
                        result.Message = "欠款收银实收金额应该小于总金额！";
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


                decimal cashCardTemp = 0;
                //decimal cardTemp = 0;
                decimal depositTemp = 0;
                decimal couponTemp = 0;
                decimal DebtTemp = 0;
                decimal commissionTemp = 0;

                for (int i = 1; i <= orders.Count(); i++)
                {
                    decimal a = 0; decimal c = 0; decimal d = 0; decimal e = 0; decimal f = 0;
                    if (orderFirst.FinalPrice > 0)
                    {
                        if (i == orders.Count())
                        {
                            a = dto.Cash + card - cashCardTemp;
                            //b = card - cardTemp;
                            c = deposit - depositTemp;
                            d = coupon - couponTemp;
                            e = debt - DebtTemp;
                            f = dto.Commission - commissionTemp;
                        }
                        else
                        {
                            a = Math.Round(orders[i - 1].DetailFinalPrice * (dto.Cash + card) / orders[i - 1].FinalPrice, 0);
                            // b = Math.Round(orders[i - 1].DetailFinalPrice * card / orders[i - 1].FinalPrice, 0);
                            c = Math.Round(orders[i - 1].DetailFinalPrice * deposit / orders[i - 1].FinalPrice, 0);
                            d = Math.Round(orders[i - 1].DetailFinalPrice * coupon / orders[i - 1].FinalPrice, 0);
                            e = Math.Round(orders[i - 1].DetailFinalPrice * debt / orders[i - 1].FinalPrice, 0);
                            f = Math.Round(orders[i - 1].DetailFinalPrice * dto.Commission / orders[i - 1].FinalPrice, 0);
                            cashCardTemp += a;
                            //cardTemp += b;
                            depositTemp += c;
                            couponTemp += d;
                            DebtTemp += e;
                            commissionTemp += f;
                        }
                    }

                    tasks.Add(_connection.ExecuteAsync(
                        @"insert into [SmartCashierCharge]([ID],[CashierID],[ReferID],[CashCardAmount],[DepositAmount],[CouponAmount],[DebtAmount],[Amount],[HospitalID],[CommissionAmount],[CreateTime],[OrderType],[CustomerID]) 
                        values(@ID,@CashierID,@ReferID,@CashCardAmount,@DepositAmount,@CouponAmount,@DebtAmount,@Amount,@HospitalID,@CommissionAmount,@CreateTime,@OrderType,@CustomerID)",
                        new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            CashierID = id,
                            ReferID = orders[i - 1].DetailID,
                            CashCardAmount = a,
                            DepositAmount = c,
                            CouponAmount = d,
                            DebtAmount = e,
                            CommissionAmount = f,
                            Amount = orders[i - 1].DetailFinalPrice,
                            HospitalID = dto.HospitalID,
                            CreateTime = now,
                            OrderType = OrderType.Order,
                            CustomerID = dto.CustomerID
                        }, _transaction));
                }

                tasks.Add(_connection.ExecuteAsync(
                    @"update [SmartOrder] set [PaidStatus]=@PaidStatus,PaidTime=@PaidTime,[DebtAmount]=@DebtAmount where ID=@ID",
                    new { ID = dto.OrderID, PaidTime = now, PaidStatus = debt > 0 ? PaidStatus.Debt : PaidStatus.Paid, DebtAmount = debt }, _transaction));


                decimal point = (orderFirst.FinalPrice) * Convert.ToDecimal(options.IntegralNumValue);

                if (point > 0)
                {
                    tasks.Add(_connection.ExecuteAsync(
                    @"insert into [SmartPoint]([ID],[CustomerID],[CreateUserID],[CreateTime],[Type],[Amount],[Remark],[ConsumeAmount],[HospitalID])
                    values(@ID,@CustomerID,@CreateUserID,@CreateTime,@Type,@Amount,@Remark,@ConsumeAmount,@HospitalID)",
                    new
                    {
                        ID = id,
                        CustomerID = dto.CustomerID,
                        CreateUserID = dto.CreateUserID,
                        CreateTime = now,
                        Type = PointType.ConsumeGive,
                        Amount = point,
                        Remark = PointType.ConsumeGive.ToDescription() + ":" + dto.OrderID,
                        ConsumeAmount = orderFirst.FinalPrice,
                        HospitalID = dto.HospitalID
                    }, _transaction));
                }


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

                if (dto.Commission > 0)
                {
                    tasks.Add(_connection.ExecuteAsync(
                         @"insert into [SmartCommissionUsage]([ID],[CustomerID],[Type],[Remark],CreateTime,CreateUserID,Amount,HospitalID) 
                        values(@ID,@CustomerID,@Type,@Remark,@CreateTime,@CreateUserID,@Amount,@HospitalID)", new
                         {
                             ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                             CustomerID = dto.CustomerID,
                             CreateTime = DateTime.Now,
                             Type = CommissionType.Consume,
                             Remark = CommissionType.Consume.ToDescription() + ":" + dto.OrderID,
                             CreateUserID = dto.CreateUserID,
                             Amount = dto.Commission * -1,
                             HospitalID = dto.HospitalID
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
        /// 欠款收银
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> DebtCashier(DebtCashierAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;
            DateTime now = DateTime.Now;

            if (dto.Cash < 0)
            {
                result.Message = "现金不能为负！";
                return result;
            }

            foreach (var u in dto.CardList)
            {
                if (u.Amount < 0)
                {
                    result.Message = "刷卡不能为负！";
                    return result;
                }
            }

            dto.CardList = dto.CardList.Where(u => u.CardCategoryID > 0 && u.Amount > 0);

            if (dto.CardList != null)
            {
                if (dto.CardList.Count() > dto.CardList.DistinctBy(u => u.CardCategoryID).Count())
                {
                    result.Message = "同一订单银行卡支付方式不能相通！";
                    return result;
                }
            }

            if (dto.Remark.IsNullOrEmpty())
            {
                dto.Remark = " ";
            }
            else if (!dto.Remark.IsNullOrEmpty() && dto.Remark.Length >= 50)
            {
                result.Message = "备注最多50个字符！";
                return result;
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


                decimal cashCardTemp = 0;
                //decimal cardTemp = 0;
                //decimal depositTemp = 0;
                //decimal couponTemp = 0;
                decimal DebtTemp = 0;
                //decimal commissionTemp = 0;
                //decimal detailTemp = 0;

                for (int i = 1; i <= orders.Count(); i++)
                {
                    decimal a = 0; decimal c = 0; decimal d = 0; decimal e = 0; decimal f = 0;
                    if (orderFirst.FinalPrice > 0)
                    {
                        if (i == orders.Count())
                        {
                            a = dto.Cash + card - cashCardTemp;
                            //b = card - cardTemp;
                            //c = deposit - depositTemp;
                            //d = coupon - couponTemp;
                            e = debt - DebtTemp;
                            //g = a+e;
                            //f = dto.Commission - commissionTemp;
                        }
                        else
                        {
                            a = Math.Round(orders[i - 1].DetailFinalPrice * (dto.Cash + card) / orders[i - 1].FinalPrice, 0);
                            //b = Math.Round(orders[i - 1].DetailFinalPrice * card / orders[i - 1].FinalPrice, 0);
                            //c = Math.Round(orders[i - 1].DetailFinalPrice * deposit / orders[i - 1].FinalPrice, 0);
                            //d = Math.Round(orders[i - 1].DetailFinalPrice * coupon / orders[i - 1].FinalPrice, 0);
                            e = Math.Round(orders[i - 1].DetailFinalPrice * debt / orders[i - 1].FinalPrice, 0);
                            //f = Math.Round(orders[i - 1].DetailFinalPrice * dto.Commission / orders[i - 1].FinalPrice, 0);
                            //g = Math.Round(orders[i - 1].DetailFinalPrice * orders[i - 1].DebtAmount / orders[i - 1].FinalPrice, 0);
                            //g = a + e;
                            cashCardTemp += a;
                            //cardTemp += b;
                            //depositTemp += c;
                            //couponTemp += d;
                            DebtTemp += e;
                            //detailTemp += g;
                            //commissionTemp += f;
                        }
                    }

                    tasks.Add(_connection.ExecuteAsync(
                        @"insert into [SmartCashierCharge]([ID],[CashierID],[ReferID],[CashCardAmount],[DepositAmount],[CouponAmount],[DebtAmount],[Amount],[HospitalID],[CommissionAmount],CreateTime,OrderType,CustomerID) 
                        values(@ID,@CashierID,@ReferID,@CashCardAmount,@DepositAmount,@CouponAmount,@DebtAmount,@Amount,@HospitalID,@CommissionAmount,@CreateTime,@OrderType,@CustomerID)",
                        new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            CashierID = id,
                            ReferID = orders[i - 1].DetailID,
                            CashCardAmount = a,
                            DepositAmount = c,
                            CouponAmount = d,
                            DebtAmount = e,
                            CommissionAmount = f,
                            Amount = a + c + d + e + f,
                            HospitalID = dto.HospitalID,
                            CreateTime = now,
                            OrderType = OrderType.Debt,
                            CustomerID = dto.CustomerID
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

            if (dto.Cash < 0)
            {
                result.Message = "现金不能为负！";
                return result;
            }

            if (dto.Card < 0)
            {
                result.Message = "刷卡不能为负！";
                return result;
            }

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
                        @"insert into [SmartCouponUsage]([ID],[CashierID],[CouponID],[Amount],[Type],[Remark],CreateUserID)
                        values(@ID,@CashierID,@CouponID,@Amount,@Type,@Remark,@CreateUserID)",
                        new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            CashierID = id,
                            CouponID = u.CouponID,
                            Amount = u.Amount,
                            Remark = CouponUsageType.DepositRebateDelete.ToDescription(),
                            Type = CouponUsageType.DepositRebateDelete,
                            CreateUserID = dto.CreateUserID
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


                if (depositRebateOrder.Point != 0)
                {
                    tasks.Add(_connection.ExecuteAsync(
                    @"insert into [SmartPoint]([ID],[CustomerID],[CreateUserID],[CreateTime],[Type],[Amount],[Remark],[ConsumeAmount],[HospitalID])
                    values(@ID,@CustomerID,@CreateUserID,@CreateTime,@Type,@Amount,@Remark,@ConsumeAmount,@HospitalID)",
                    new
                    {
                        ID = id,
                        CustomerID = dto.CustomerID,
                        CreateUserID = dto.CreateUserID,
                        CreateTime = now,
                        Type = PointType.RebateRebate,
                        Amount = depositRebateOrder.Point * -1,
                        Remark = PointType.RebateRebate.ToDescription() + ":" + dto.OrderID,
                        ConsumeAmount = 0,
                        HospitalID = dto.HospitalID
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

            if (dto.Cash < 0)
            {
                result.Message = "现金不能为负！";
                return result;
            }

            if (dto.Card < 0)
            {
                result.Message = "刷卡不能为负！";
                return result;
            }
            if (dto.Deposit < 0)
            {
                result.Message = "预收款不能为负！";
                return result;
            }

            if (dto.Coupon < 0)
            {
                result.Message = "代金券不能为负！";
                return result;
            }

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
                        OrderType = OrderType.Back,
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


                decimal cashCardTemp = 0;
                //decimal cardTemp = 0;
                decimal depositTemp = 0;
                decimal couponTemp = 0;
                //decimal DebtTemp = 0;

                for (int i = 1; i <= backorders.Count(); i++)
                {
                    decimal a = 0; decimal c = 0; decimal d = 0;
                    if (backorderFirst.Amount > 0)
                    {
                        if (i == backorders.Count())
                        {
                            a = dto.Cash + dto.Card - cashCardTemp;
                            //b = dto.Card - cardTemp;
                            c = dto.Deposit - depositTemp;
                            d = dto.Coupon - couponTemp;
                            //e = debt - DebtTemp;
                        }
                        else
                        {
                            a = Math.Round(backorders[i - 1].DetailAmount * (dto.Cash + dto.Card) / backorders[i - 1].Amount, 0);
                            //b = Math.Round(backorders[i - 1].DetailAmount * dto.Card / backorders[i - 1].Amount, 0);
                            c = Math.Round(backorders[i - 1].DetailAmount * dto.Deposit / backorders[i - 1].Amount, 0);
                            d = Math.Round(backorders[i - 1].DetailAmount * dto.Coupon / backorders[i - 1].Amount, 0);
                            //e = Math.Round(backorders[i - 1].DetailFinalPrice * debt / backorders[i - 1].FinalPrice, 0);
                            cashCardTemp += a;
                            //cardTemp += b;
                            depositTemp += c;
                            couponTemp += d;
                        }
                    }

                    tasks.Add(_connection.ExecuteAsync(
                        @"insert into [SmartCashierCharge]([ID],[CashierID],[ReferID],[CashCardAmount],[DepositAmount],[CouponAmount],[DebtAmount],[Amount],[HospitalID],CreateTime,OrderType,CustomerID) 
                        values(@ID,@CashierID,@ReferID,@CashCardAmount,@DepositAmount,@CouponAmount,@DebtAmount,@Amount,@HospitalID,@CreateTime,@OrderType,@CustomerID)",
                        new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            CashierID = id,
                            ReferID = backorders[i - 1].DetailID,
                            CashCardAmount = a * -1,
                            DepositAmount = c * -1,
                            CouponAmount = d * -1,
                            DebtAmount = 0,
                            Amount = backorders[i - 1].DetailAmount * -1,
                            HospitalID = dto.HospitalID,
                            CreateTime = now,
                            OrderType = OrderType.Back,
                            CustomerID = dto.CustomerID
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

                if (backorderFirst.Point != 0)
                {
                    tasks.Add(_connection.ExecuteAsync(
                    @"insert into [SmartPoint]([ID],[CustomerID],[CreateUserID],[CreateTime],[Type],[Amount],[Remark],[ConsumeAmount],[HospitalID])
                    values(@ID,@CustomerID,@CreateUserID,@CreateTime,@Type,@Amount,@Remark,@ConsumeAmount,@HospitalID)",
                    new
                    {
                        ID = id,
                        CustomerID = dto.CustomerID,
                        CreateUserID = dto.CreateUserID,
                        CreateTime = now,
                        Type = PointType.BackRebate,
                        Amount = backorderFirst.Point * -1,
                        Remark = PointType.BackRebate.ToDescription() + ":" + dto.OrderID,
                        ConsumeAmount = 0,
                        HospitalID = dto.HospitalID
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
                var list = new Dictionary<string, CashierUpdateInfo>();
                await _connection.QueryAsync<CashierUpdateInfo, CardCashier, CashierUpdateInfo>(
                    @"select a.ID,a.HospitalID,a.OrderType,a.CustomerID,a.Card,a.Cash,b.CardCategoryID,b.Card  as Amount
                    from SmartCashier a
                    left join SmartCashierCardCatogoryDetail b on a.ID=b.CashierID
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
        /// 订单修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> CashierUpdate(CashierUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.Cash < 0)
            {
                result.Message = "现金不能为负！";
                return result;
            }

            foreach (var u in dto.CardList)
            {
                if (u.Amount < 0)
                {
                    result.Message = "刷卡不能为负！";
                    return result;
                }
            }

            if (dto.OrderType == OrderType.Back || dto.OrderType == OrderType.Refund)
            {
                if (dto.CardList.Count > 1)
                {
                    result.Message = "对不起，退款单跟退项目单只允许一种卡类型！";
                    return result;
                }
            }
            if (dto.CardList.Count() > dto.CardList.DistinctBy(u => u.CardCategoryID).Count())
            {
                result.Message = "卡类型重复，请检查您的卡类型！";
                return result;
            }
            var cashier = (await GetCashierUpdateInfo(dto.ID)).Data;
            if (cashier == null)
            {
                result.Message = "对不起，订单不存在！";
                return result;
            }

            if (cashier.HospitalID != dto.HospitalID.ToString())
            {
                result.Message = "对不起，您无权操作其他家医院的订单！";
                return result;
            }

            if (dto.OrderType == OrderType.Back || dto.OrderType == OrderType.Refund)
            {
                if (dto.CardList.Count > 1)
                {
                    result.Message = "对不起，退款单跟退项目单只允许一种卡类型！";
                    return result;
                }
            }
            if (cashier.Status == CashierStatus.Yes)
            {
                result.Message = "对不起，已经结算订单不允许修改！";
                return result;
            }

            await TryTransactionAsync(async () =>
            {
                await _connection.ExecuteAsync(
                    @"delete from [SmartCashierCardCatogoryDetail] where [CashierID]=@CashierID",
                    new
                    {
                        CashierID = dto.ID
                    }, _transaction);


                List<Task> tasks = new List<Task>();
                decimal card = 0;
                foreach (var u in dto.CardList)
                {
                    if (u.Amount != 0)
                    {
                        tasks.Add(_connection.ExecuteAsync(
                       @"insert into [SmartCashierCardCatogoryDetail]([ID],[CashierID],[CardCategoryID],[Card]) 
                        values(@ID,@CashierID,@CardCategoryID,@Card)",
                       new
                       {
                           ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                           CashierID = dto.ID,
                           CardCategoryID = u.CardCategoryID,
                           Card = u.Amount
                       }, _transaction));
                        card += u.Amount;
                    }
                }


                if (dto.Cash + card != cashier.Cash + cashier.Card)
                {
                    result.Message = "金额不匹配，无法支付！";
                    return false;
                }

                tasks.Add(_connection.ExecuteAsync(
                    @"update [SmartCashier] set [Cash]=@Cash,[Card]=@Card where ID=@ID", new { Cash = dto.Cash, Card = card, ID = dto.ID }, _transaction));

                await Task.WhenAll(tasks);

                result.Message = "更新成功！";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 今日收银记录
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<Cashier>>> GetCashierToday(long hospitalID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<Cashier>>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            await TryExecuteAsync(async () =>
            {
                var list = new Dictionary<string, Cashier>();
                await _connection.QueryAsync<Cashier, string, Cashier>(
                    @"select a.ID,a.CustomerID,d.Name as CustomerName,a.OrderType,a.HospitalID,e.Name as HospitalName,
                    a.No,a.CreateTime,c.Name as CreateUserName,a.Status,a.Amount,a.Amount-a.Debt as RealAmount,a.Cash,a.Card,
                    a.Deposit,a.Coupon,a.Debt,a.[Commission],a.Remark,f.Name+ ':'+ CAST(b.Card AS nvarchar(50)) as CardCategoryNames
                    from SmartCashier a
                    left join SmartCashierCardCatogoryDetail b on a.ID=b.CashierID
                    left join SmartCardCategory f on b.CardCategoryID=f.ID
                    left join SmartUser c on a.CreateUserID=c.ID
                    left join SmartCustomer d on a.CustomerID=d.ID
                    left join SmartHospital e on a.HospitalID=e.ID
                    where a.CreateTime between @StartTime and @EndTime and a.HospitalID=@HospitalID order by a.CreateTime desc",
                    (cashier, card) =>
                    {
                        Cashier temp = new Cashier();
                        if (!list.TryGetValue(cashier.ID, out temp))
                        {
                            list.Add(cashier.ID, temp = cashier);
                        }
                        if (card != null)
                            temp.CardCategoryNames.Add(card);
                        return cashier;
                    }, new { HospitalID = hospitalID, StartTime = DateTime.Today, EndTime = DateTime.Today.AddDays(1) }, null, true, splitOn: "CardCategoryNames");

                result.Data = list.Values;

            });

            return result;
        }

        /// <summary>
        /// 收银记录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<Cashier>>>> GetCashier(CashierSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<Cashier>>>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;
            result.Data = new Pages<IEnumerable<Cashier>>();
            result.Data.PageNum = dto.PageNum;
            result.Data.PageSize = dto.PageSize;

            await TryExecuteAsync(async () =>
            {
                string sql_where = " where 1=1 ";
                if (dto.HospitalID > 0)
                {
                    sql_where += " and a.HospitalID=@HospitalID ";
                }
                if (dto.StartTime != null && dto.EndTime != null)
                {
                    sql_where += " and a.CreateTime between @StartTime and @EndTime ";
                }
                if (dto.CustomerID > 0)
                {
                    sql_where += " and a.CustomerID=@CustomerID ";
                }
                if (!dto.No.IsNullOrEmpty())
                {
                    sql_where += " and a.No=@No ";
                }

                int startRow = dto.PageSize * (dto.PageNum - 1);
                int endRow = dto.PageSize;

                result.Data.PageTotals = (await _connection.QueryAsync<int>(
                    string.Format(@"select count(a.ID)
                    from SmartCashier a
                    {0}", sql_where), dto)).FirstOrDefault();
                var list = new Dictionary<string, Cashier>();
                await _connection.QueryAsync<Cashier, string, Cashier>(
                    string.Format(@"select a.ID,a.CustomerID,d.Name as CustomerName,a.OrderType,a.HospitalID,e.Name as HospitalName,
                    a.No,a.CreateTime,c.Name as CreateUserName,a.Status,a.Amount,a.Amount-a.Debt as RealAmount,a.Cash,a.Card,
                    a.Deposit,a.Coupon,a.Debt,a.[Commission],a.Remark,f.Name+ ':'+ CAST(b.Card AS nvarchar(50)) as CardCategoryNames
                    from SmartCashier a
                    left join SmartCashierCardCatogoryDetail b on a.ID=b.CashierID
                    left join SmartCardCategory f on b.CardCategoryID=f.ID
                    left join SmartUser c on a.CreateUserID=c.ID
                    left join SmartCustomer d on a.CustomerID=d.ID
                    left join SmartHospital e on a.HospitalID=e.ID
                    {0} order by a.CreateTime desc  OFFSET {1} ROWS FETCH NEXT {2} ROWS only", sql_where, startRow, endRow),
                    (cashier, card) =>
                    {
                        Cashier temp = new Cashier();
                        if (!list.TryGetValue(cashier.ID, out temp))
                        {
                            list.Add(cashier.ID, temp = cashier);
                        }
                        if (card != null)
                            temp.CardCategoryNames.Add(card);
                        return cashier;
                    }, dto, null, true, splitOn: "CardCategoryNames");

                result.Data.PageDatas = list.Values;

            });

            return result;
        }

        /// <summary>
        /// 查询收银详细
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, CashierDetail>> GetDetail(long ID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, CashierDetail>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;
            await TryExecuteAsync(async () =>
            {
                var list = new Dictionary<string, CashierDetail>();
                await _connection.QueryAsync<CashierDetail, CardCashier, CashierDetail>(
                    @"select a.HospitalID,a.ID,a.OrderID,a.CustomerID,b.Name as CustomerName,a.No,a.CreateTime,a.Amount,a.Card,a.Cash,a.Commission,a.Coupon,a.Debt,
                    a.Deposit,e.Name as CreateUserName,a.OrderType,c.CardCategoryID,d.Name as CardCategoryName,c.Card as Amount
                    from SmartCashier a
                    inner join SmartCustomer b on a.CustomerID=b.ID
                    left join SmartCashierCardCatogoryDetail c on c.CashierID=a.ID
                    left join SmartCardCategory d on c.CardCategoryID=d.ID
                    left join SmartUser e on a.CreateUserID=e.ID
                    where a.ID=@ID",
                    (cashier, card) =>
                    {
                        CashierDetail temp = new CashierDetail();
                        if (!list.TryGetValue(cashier.ID, out temp))
                        {
                            list.Add(cashier.ID, temp = cashier);
                        }
                        if (card != null)
                            temp.CardList.Add(card);
                        return cashier;
                    }, new { ID = ID }, null, true, splitOn: "CardCategoryID");

                var data = list.Values.FirstOrDefault();

                if (data == null)
                {
                    result.Message = "收银记录不存在！";
                    result.ResultType = IFlyDogResultType.Failed;
                    return;
                }

                if (data.OrderType == OrderType.InPatient || data.OrderType == OrderType.Order || data.OrderType == OrderType.Debt)
                {
                    data.ChargeList = await _connection.QueryAsync<CashierChargeTemp>(
                        @"select c.ChargeID,d.Name as ChargeName,c.Num,e.Name as UnitName,b.Amount,b.DebtAmount,d.Price
                        from [SmartCashierCharge] b
                        inner join SmartOrderDetail c on c.ID=b.ReferID
                        inner join SmartCharge d on c.ChargeID=d.ID
                        inner join SmartUnit e on d.UnitID=e.ID
                        where b.CashierID=@CashierID", new { CashierID = ID });
                }
                else if (data.OrderType == OrderType.Deposit)
                {
                    data.ChargeList = await _connection.QueryAsync<CashierChargeTemp>(
                        @"select a.ChargeID,b.Name as ChargeName,a.Num,a.Price,a.Total as Amount
                        from SmartDepositOrderDetail a
                        inner join SmartDepositCharge b on a.ChargeID=b.ID
                        where a.OrderID=@OrderID", new { OrderID = data.OrderID });
                }
                else if (data.OrderType == OrderType.Back)
                {
                    data.ChargeList = await _connection.QueryAsync<CashierChargeTemp>(
                        @"select a.ChargeID,b.Name as ChargeName,a.Num,a.Amount
                        from SmartBackOrderDetail a
                        inner join SmartCharge b on a.ChargeID=b.ID
                        where a.OrderID=@OrderID", new { OrderID = data.OrderID });
                }
                else if (data.OrderType == OrderType.Refund)
                {
                    data.ChargeList = await _connection.QueryAsync<CashierChargeTemp>(
                       @"select b.ChargeID,c.Name as ChargeName,a.Amount
                        from [SmartDepositRebateOrderDetail] a
                        inner join SmartDeposit b on a.DepositID=b.ID
						inner join SmartDepositCharge c on b.ChargeID=c.ID
                        where a.OrderID=@OrderID
						union all
						select b.CategoryID,c.Name as ChargeName,a.Amount
                        from SmartDepositRebateCouponOrderDetail a
                        inner join SmartCoupon b on a.CouponID=b.ID
						inner join SmartCouponCategory c on b.CategoryID=c.ID
                        where a.OrderID=@OrderID", new { OrderID = data.OrderID });
                }

                result.Data = data;
            });

            return result;
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, string>> Print(long ID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, string>();
            result.Message = "查询成功！";
            result.ResultType = IFlyDogResultType.Success;

            var cashier = (await this.GetDetail(ID)).Data;

            await TryExecuteAsync(async () =>
            {
                if (cashier != null)
                {
                    string table = string.Empty;
                    string tableDetail = string.Empty;
                    string cardDetail = string.Empty;

                    HospitalPrintStatus status = HospitalPrintStatus.menu;

                    if (cashier.OrderType == OrderType.Deposit)
                    {
                        status = HospitalPrintStatus.advancesReceived;
                    }
                    else if (cashier.OrderType == OrderType.Back)
                    {
                        status = HospitalPrintStatus.returnAProject;
                    }
                    else if (cashier.OrderType == OrderType.Refund)
                    {
                        status = HospitalPrintStatus.depositRefund;
                    }

                    var hospitalPrint = (await _connection.QueryAsync<HospitalPrintInfo>(
                            @"SELECT ID,HospitalID,Type,Width,Content,FontSize,FontFamily FROM SmartHospitalPrint WHERE HospitalID = @HospitalID AND Type = @Type",
                            new { HospitalID = cashier.HospitalID, Type = status })).FirstOrDefault();

                    foreach (var u in cashier.CardList)
                    {
                        cardDetail += u.CardCategoryName + "：" + u.Amount + " ";
                    }

                    if (cashier.OrderType == OrderType.Deposit)
                    {
                        foreach (var u in cashier.ChargeList)
                        {
                            tableDetail += "<tr value=" +
                            u.ChargeID +
                            ">" +
                            "<td>" +
                            u.ChargeName +
                            "</td>" +
                            "<td>" +
                            u.Price +
                            "</td>" +
                            "<td>" +
                            u.Num +
                            "</td>" +
                            "<td>" +
                            u.Amount +
                            "</td>" +
                            "</tr>";
                        }

                        table += "<table class='site-table table-hover' style='width:" + hospitalPrint.Width + "px;font-size:" + hospitalPrint.FontSize + "px;font-family:" + hospitalPrint.FontFamily + ";'>";
                        table += "<thead><tr><th>项目</th><th>价格</th><th>数量</th><th>金额</th></tr></thead>";
                        table += "<tbody id='cashierID'>";
                        table += tableDetail;
                        table += "</tbody>";
                        table += "</table>";
                    }
                    else if (cashier.OrderType == OrderType.Back)
                    {
                        foreach (var u in cashier.ChargeList)
                        {
                            tableDetail += "<tr value=" +
                            u.ChargeID +
                            ">" +
                            "<td>" +
                            u.ChargeName +
                            "</td>" +
                            "<td>" +
                            u.Num +
                            "</td>" +
                            "<td>" +
                            u.Amount +
                            "</td>" +
                            "</tr>";
                        }

                        table += "<table class='site-table table-hover' style='width:" + hospitalPrint.Width + "px;font-size:" + hospitalPrint.FontSize + "px;font-family:" + hospitalPrint.FontFamily + ";'>";
                        table += "<thead><tr><th>项目</th><th>数量</th><th>金额</th></tr></thead>";
                        table += "<tbody id='cashierID'>";
                        table += tableDetail;
                        table += "</tbody>";
                        table += "</table>";
                    }
                    else if (cashier.OrderType == OrderType.Refund)
                    {
                        foreach (var u in cashier.ChargeList)
                        {
                            tableDetail += "<tr value=" +
                            u.ChargeID +
                            ">" +
                            "<td>" +
                            u.ChargeName +
                            "</td>" +
                            "<td>" +
                            u.Amount +
                            "</tr>";
                        }

                        table += "<table class='site-table table-hover' style='width:" + hospitalPrint.Width + "px;font-size:" + hospitalPrint.FontSize + "px;font-family:" + hospitalPrint.FontFamily + ";'>";
                        table += "<thead><tr><th>预收款/券</th><th>金额</th></tr></thead>";
                        table += "<tbody id='cashierID'>";
                        table += tableDetail;
                        table += "</tbody>";
                        table += "</table>";
                    }
                    else
                    {
                        foreach (var u in cashier.ChargeList)
                        {
                            tableDetail += "<tr value=" +
                            u.ChargeID +
                            ">" +
                            "<td>" +
                            u.ChargeName +
                            "</td>" +
                            "<td>" +
                            u.UnitName +
                            "</td>" +
                            "<td>" +
                            u.Num +
                            "</td>" +
                            "<td>" +
                            u.Amount +
                            "</td>" +
                            "<td>" +
                            (u.Amount - u.DebtAmount) +
                            "</td>" +
                            "</td>" +
                            "<td>" +
                            u.DebtAmount +
                            "</td>" +
                            "</tr>";
                        }

                        table += "<table class='site-table table-hover' style='width:" + hospitalPrint.Width + "px;font-size:" + hospitalPrint.FontSize + "px;font-family:" + hospitalPrint.FontFamily + ";'>";
                        table += "<thead><tr><th>项目</th><th>单位</th><th>数量</th><th>应收金额</th><th>实收金额</th><th>欠款</th></tr></thead>";
                        table += "<tbody id='cashierID'>";
                        table += tableDetail;
                        table += "</tbody>";
                        table += "</table>";
                    }

                    string printStr = hospitalPrint.Content.ToString();
                    printStr = printStr.Replace("$CustomerID", cashier.CustomerID);
                    printStr = printStr.Replace("$CustomerName", cashier.CustomerName);
                    printStr = printStr.Replace("$No", cashier.No);
                    printStr = printStr.Replace("$CreateDate", cashier.CreateTime.ToString("d"));
                    printStr = printStr.Replace("$Table", table);
                    printStr = printStr.Replace("$CreateTime", cashier.CreateTime.ToString("u"));
                    printStr = printStr.Replace("$CreateUserName", cashier.CreateUserName);
                    printStr = printStr.Replace("$OrderID", cashier.OrderID);
                    printStr = printStr.Replace("$Amount", cashier.Amount.ToString());
                    printStr = printStr.Replace("$Total", (cashier.Amount - cashier.Debt).ToString());
                    printStr = printStr.Replace("$Cash", cashier.Cash.ToString());
                    printStr = printStr.Replace("$Card", cashier.Card.ToString());
                    printStr = printStr.Replace("CardCategory", cardDetail);
                    printStr = printStr.Replace("Deposit", cashier.Deposit.ToString());
                    printStr = printStr.Replace("Coupon", cashier.Coupon.ToString());
                    printStr = printStr.Replace("Debt", cashier.Debt.ToString());
                    printStr = printStr.Replace("Commission", cashier.Commission.ToString());
                    result.Data = printStr;
                }
                else
                {
                    result.Message = "打印数据查询出现异常!";
                }
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
