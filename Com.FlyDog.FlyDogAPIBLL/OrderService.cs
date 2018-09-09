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
    public class OrderService : BaseService, IOrderService
    {
        /// <summary>
        /// 
        /// </summary>
        private IChargeDiscountService _chargeDiscountService;
        public OrderService(IChargeDiscountService chargeDiscountService)
        {
            _chargeDiscountService = chargeDiscountService;
        }
        /// <summary>
        /// 查询套餐
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pym"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<ChargeSet>>> GetChargeSet(string name, string pym)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<ChargeSet>>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            string sql_where = "";
            if (!name.IsNullOrEmpty())
            {
                sql_where += " and a.Name like @Name ";
            }
            if (!pym.IsNullOrEmpty())
            {
                sql_where += " and a.PinYin like  @PinYin ";
            }
            await TryExecuteAsync(async () =>
            {
                var list = new Dictionary<string, ChargeSet>();
                await _connection.QueryAsync<ChargeSet, ChargeSetDetail, ChargeSet>(
                    string.Format(@"select a.ID as SetID,a.Name as SetName,a.Price,b.ChargeID,c.Name as ChargeName,b.Amount,b.Num 
                    from SmartChargeSet a
                    inner join SmartChargeSetDetail b on a.ID=b.SetID
                    inner join SmartCharge c on b.ChargeID=c.ID
                    where a.Status=@Status {0}", sql_where),
                    (set, charge) =>
                    {
                        ChargeSet temp = new ChargeSet();
                        if (!list.TryGetValue(set.SetID, out temp))
                        {
                            list.Add(set.SetID, temp = set);
                        }
                        if (charge != null)
                            temp.Details.Add(charge);
                        return set;
                    }, new { Status = CommonStatus.Use, Name = name, PinYin = pym }, null, true, splitOn: "ChargeID");

                result.Data = list.Values;
            });

            return result;
        }

        /// <summary>
        /// 添加订单
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Add(OrderAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.Details == null || dto.Details.Count() == 0)
            {
                result.Message = "请选择收费项目！";
                return result;
            }

            if (!dto.Remark.IsNullOrEmpty() && dto.Remark.Length >= 150)
            {
                result.Message = "备注不能超过150字！";
                return result;
            }

            var sets = (await GetChargeSet(null, null)).Data;

            await TryTransactionAsync(async () =>
            {
                if (dto.InpatientID == null || dto.InpatientID == 0)
                {
                    if (!await IsCome(dto.CustomerID, dto.CreateUserID))
                    {
                        result.Message = "今日未上门，不能操作，请在前台先分诊 ！";
                        return false;
                    }
                    dto.InpatientID = null;
                }
                else
                {
                    int countInpatient = (await _connection.QueryAsync<int>(
                    @"select count(ID) from [SmartOrder] where [CustomerID]=@CustomerID and PaidStatus=@PaidStatus and InpatientID=@InpatientID",
                    new
                    {
                        CustomerID = dto.CustomerID,
                        PaidStatus = PaidStatus.NotPaid,
                        InpatientID = dto.InpatientID
                    }, _transaction)).FirstOrDefault();

                    if (countInpatient > 0)
                    {
                        result.Message = "您有住院单未结算，不能添加新的住院单！";
                        return false;
                    }
                }


                if (!await HasCustomerOAuthTransactionAsync(dto.CreateUserID, dto.CustomerID))
                {
                    result.Message = "对不起，您无权操作该用户！";
                    result.ResultType = IFlyDogResultType.NoAuth;
                    return false;
                }

                string sql_where = " where [Status]=@Status";
                int count = 0;
                foreach (var u in dto.Details)
                {
                    if (u.SetID <= 0)
                    {
                        if (count == 0)
                            sql_where += " and [ID]=" + u.ChargeID;
                        else
                            sql_where += " or ID=" + u.ChargeID;

                        count++;
                    }
                }


                var charges = await _connection.QueryAsync<OrderDetailAddTemp>(
                    string.Format(@"select distinct [ID] as ChargeID,[Price] from [SmartCharge] {0}", sql_where), new { Status = CommonStatus.Use }, _transaction);


                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();
                List<OrderDetailAddTemp> detailList = new List<OrderDetailAddTemp>();
                decimal finalPrice = 0;
                decimal totalPrice = 0;
                foreach (var u in dto.Details)
                {
                    if (u.Num <= 0)
                    {
                        result.Message = "项目数量不能小于等于0！";
                        return false;
                    }

                    if (u.FinalPrice < 0)
                    {
                        result.Message = "项目金额不能小于0！";
                        return false;
                    }

                    if (u.SetID > 0)
                    {
                        foreach (var n in sets)
                        {
                            if (u.SetID.ToString() == n.SetID)
                            {
                                foreach (var s in n.Details)
                                {
                                    detailList.Add(new OrderDetailAddTemp()
                                    {
                                        ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                                        OrderID = id,
                                        ChargeID = long.Parse(s.ChargeID),
                                        SetID = long.Parse(n.SetID),
                                        Num = s.Num * u.Num,
                                        RestNum = s.Num * u.Num,
                                        Price = s.Amount * u.Num,
                                        FinalPrice = s.Amount * u.Num,
                                        SetNum = u.Num
                                    });
                                }
                                finalPrice += n.Price * u.Num;
                                totalPrice += n.Price * u.Num;
                            }
                        }
                    }
                    else
                    {
                        foreach (var n in charges)
                        {
                            if (u.ChargeID == n.ChargeID)
                            {
                                detailList.Add(new OrderDetailAddTemp()
                                {
                                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                                    OrderID = id,
                                    Num = u.Num,
                                    ChargeID = u.ChargeID,
                                    SetID = null,
                                    Price = n.Price * u.Num,
                                    FinalPrice = u.FinalPrice,
                                    RestNum = u.Num,
                                    SetNum = null,
                                });

                                totalPrice += n.Price * u.Num;
                                finalPrice += u.FinalPrice;
                            }
                        }
                    }
                }


                AuditType auditStatus = await IsAudit(totalPrice, finalPrice, detailList, dto.HospitalID, dto.CreateUserID, dto.CustomerID);


                Task task1 = _connection.ExecuteAsync(
                    @"insert into [SmartOrder]([ID],[CustomerID],[CreateUserID],[CreateTime],[TotalPrice],[FinalPrice],[PaidStatus],[Remark],[AuditStatus],[DebtAmount],[HospitalID],[InpatientID]) 
                        values(@ID,@CustomerID,@CreateUserID,@CreateTime,@TotalPrice,@FinalPrice,@PaidStatus,@Remark,@AuditStatus,@DebtAmount,@HospitalID,@InpatientID)",
                    new
                    {
                        ID = id,
                        HospitalID = dto.HospitalID,
                        CustomerID = dto.CustomerID,
                        CreateUserID = dto.CreateUserID,
                        CreateTime = DateTime.Now,
                        FinalPrice = finalPrice,
                        TotalPrice = totalPrice,
                        Remark = dto.Remark,
                        PaidStatus = PaidStatus.NotPaid,
                        AuditStatus = auditStatus,
                        DebtAmount = 0,
                        InpatientID = dto.InpatientID
                    }, _transaction);

                Task task2 = _connection.ExecuteAsync(
                    @"insert into [SmartOrderDetail]([ID],[OrderID],[ChargeID],[Price],[Num],[FinalPrice],[RestNum],[SetID],SetNum) 
                    values(@ID,@OrderID,@ChargeID,@Price,@Num,@FinalPrice,@RestNum,@SetID,@SetNum)", detailList, _transaction, 30, CommandType.Text);

                await Task.WhenAll(task1, task2);

                result.Message = "添加订单成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 订单删除
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
                    @"select [PaidStatus],HospitalID from [SmartOrder] where [ID]=@ID", new { ID = dto.OrderID }, _transaction)).FirstOrDefault();

                if (temp == null)
                {
                    result.Message = "预收款订单不存在！";
                    return false;
                }

                if ((PaidStatus)temp.PaidStatus == PaidStatus.Paid || (PaidStatus)temp.PaidStatus == PaidStatus.Debt)
                {
                    result.Message = "已付款或者欠款订单不允许删除！";
                    return false;
                }
                if ((long)temp.HospitalID != dto.HospitalID)
                {
                    result.Message = "对不起，您无权操作其他家医院的订单！";
                    return false;
                }

                Task task1 = _connection.ExecuteAsync(
                    @"delete from [SmartOrder] where ID=@ID", new { ID = dto.OrderID }, _transaction);
                Task task2 = _connection.ExecuteAsync(
                    @"delete from [SmartOrderDetail] where [OrderID]=@ID", new { ID = dto.OrderID }, _transaction);
                Task task3 = _connection.ExecuteAsync(
                    @"delete from SmartAudit where OrderID=@OrderID and OrderType=@OrderType", new { OrderID = dto.OrderID, OrderType = OrderType.Order }, _transaction);

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
        public async Task<IFlyDogResult<IFlyDogResultType, Order>> GetDetail(long customerID, long orderID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, Order>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;
            await TryExecuteAsync(async () =>
            {
                var orderTemp = (await _connection.QueryAsync<Order>(
                    @"select a.ID as OrderID,a.CustomerID,a.CreateUserID,b.Name as CreateUserName,a.HospitalID,c.Name as HospitalName,a.Remark,
                    a.CreateTime,a.TotalPrice,a.FinalPrice,a.PaidStatus,a.PaidTime,a.AuditStatus,a.DebtAmount,
                    case when TotalPrice>0 then FinalPrice*100/TotalPrice else 0 end as Discount
                    from [SmartOrder] a 
                    inner join SmartUser b on a.CreateUserID=b.ID
                    inner join SmartHospital c on a.HospitalID=c.ID
                    where a.CustomerID=@CustomerID and a.ID=@ID", new { CustomerID = customerID, ID = orderID })).FirstOrDefault();

                if (orderTemp == null)
                {
                    result.Message = "订单不存在";
                    result.ResultType = IFlyDogResultType.Failed;
                    return;
                }

                orderTemp.ChargeDetials = await _connection.QueryAsync<OrderChargeDetail>(
                    @"select a.ChargeID,b.Name as ChargeName,a.Price/a.Num as Price,a.Num,a.FinalPrice
                    from [SmartOrderDetail] a
                    inner join SmartCharge b on a.ChargeID=b.ID 
                    where OrderID=@OrderID and SetID is null", new { OrderID = orderID });


                var list = new Dictionary<string, OrderSetDetail>();
                await _connection.QueryAsync<OrderSetDetail, OrderChargeDetail, OrderSetDetail>(
                    @"select a.SetID,b.Name as SetName,a.SetNum,a.ChargeID,c.Name as ChargeName, a.Price/a.SetNum as Price, a.FinalPrice/a.SetNum as FinalPrice,a.Num/a.SetNum as Num
                    from [SmartOrderDetail] a
                    inner join SmartChargeSet b on a.SetID=b.ID
                    inner join SmartCharge c on a.ChargeID=c.ID 
                    where OrderID=@OrderID and SetID > 0",
                    (order, detail) =>
                    {
                        OrderSetDetail temp = new OrderSetDetail();
                        if (!list.TryGetValue(order.SetID, out temp))
                        {
                            list.Add(order.SetID, temp = order);
                        }
                        if (detail != null)
                        {
                            temp.FinalPrice += detail.FinalPrice * temp.SetNum;
                            temp.Price += detail.Price;
                            temp.ChargeDetails.Add(detail);
                        }

                        return order;
                    }, new { OrderID = orderID }, _transaction, true, splitOn: "ChargeID");

                orderTemp.SetDetials = list.Values;
                result.Data = orderTemp;
            });

            return result;
        }

        /// <summary>
        /// 添加订单
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Update(OrderAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.Details == null || dto.Details.Count() == 0)
            {
                result.Message = "请选择收费项目！";
                return result;
            }

            if (!dto.Remark.IsNullOrEmpty() && dto.Remark.Length >= 150)
            {
                result.Message = "备注不能超过150字！";
                return result;
            }

            var sets = (await GetChargeSet(null, null)).Data;

            await TryTransactionAsync(async () =>
            {
                if (!await IsCome(dto.CustomerID, dto.CreateUserID))
                {
                    result.Message = "今日未上门，不能操作，请在前台先分诊 ！";
                    return false;
                }

                if (!await HasCustomerOAuthTransactionAsync(dto.CreateUserID, dto.CustomerID))
                {
                    result.Message = "对不起，您无权操作该用户！";
                    result.ResultType = IFlyDogResultType.NoAuth;
                    return false;
                }

                var temp = (await _connection.QueryAsync(
                    @"select [PaidStatus],HospitalID from [SmartOrder] where [ID]=@ID", new { ID = dto.OrderID }, _transaction)).FirstOrDefault();

                if (temp == null)
                {
                    result.Message = "订单不存在！";
                    return false;
                }

                if ((PaidStatus)temp.PaidStatus == PaidStatus.Paid || (PaidStatus)temp.PaidStatus == PaidStatus.Debt)
                {
                    result.Message = "已付款或者欠款订单不允许更新！";
                    return false;
                }
                if ((long)temp.HospitalID != dto.HospitalID)
                {
                    result.Message = "对不起，您无权操作其他家医院的订单！";
                    return false;
                }

                string sql_where = " where [Status]=@Status";
                int count = 0;
                foreach (var u in dto.Details)
                {
                    if (u.SetID <= 0)
                    {
                        if (count == 0)
                            sql_where += " and [ID]=" + u.ChargeID;
                        else
                            sql_where += " or ID=" + u.ChargeID;

                        count++;
                    }
                }


                var charges = await _connection.QueryAsync<OrderDetailAddTemp>(
                    string.Format(@"select distinct [ID] as ChargeID,[Price] from [SmartCharge] {0}", sql_where), new { Status = CommonStatus.Use }, _transaction);


                List<OrderDetailAddTemp> detailList = new List<OrderDetailAddTemp>();
                decimal finalPrice = 0;
                decimal totalPrice = 0;
                foreach (var u in dto.Details)
                {
                    if (u.Num <= 0)
                    {
                        result.Message = "预收款数量不能小于0！";
                        return false;
                    }

                    if (u.FinalPrice < 0)
                    {
                        result.Message = "项目金额不能小于0！";
                        return false;
                    }

                    if (u.SetID > 0)
                    {
                        foreach (var n in sets)
                        {
                            if (u.SetID.ToString() == n.SetID)
                            {
                                foreach (var s in n.Details)
                                {
                                    detailList.Add(new OrderDetailAddTemp()
                                    {
                                        ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                                        OrderID = dto.OrderID,
                                        ChargeID = long.Parse(s.ChargeID),
                                        SetID = long.Parse(n.SetID),
                                        Num = s.Num * u.Num,
                                        RestNum = s.Num * u.Num,
                                        Price = s.Amount * u.Num,
                                        FinalPrice = s.Amount * u.Num,
                                        SetNum = u.Num
                                    });
                                }
                                finalPrice += n.Price * u.Num;
                                totalPrice += n.Price * u.Num;
                            }
                        }
                    }
                    else
                    {
                        foreach (var n in charges)
                        {
                            if (u.ChargeID == n.ChargeID)
                            {
                                detailList.Add(new OrderDetailAddTemp()
                                {
                                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                                    OrderID = dto.OrderID,
                                    Num = u.Num,
                                    ChargeID = u.ChargeID,
                                    SetID = null,
                                    Price = n.Price * u.Num,
                                    FinalPrice = u.FinalPrice,
                                    RestNum = u.Num,
                                    SetNum = null
                                });

                                totalPrice += n.Price * u.Num;
                                finalPrice += u.FinalPrice;
                            }
                        }
                    }
                }

                AuditType auditStatus = await IsAudit(totalPrice, finalPrice, detailList, dto.HospitalID, dto.CreateUserID, dto.CustomerID);

                await _connection.ExecuteAsync(
                    @"delete from [SmartOrderDetail] where [OrderID]=@OrderID", new { OrderID = dto.OrderID }, _transaction);
                await _connection.ExecuteAsync(
                    @"delete from SmartAudit where OrderID=@OrderID and OrderType=@OrderType", new { OrderID = dto.OrderID, OrderType = OrderType.Order }, _transaction);

                Task task1 = _connection.ExecuteAsync(
                    @"update [SmartOrder] set CreateUserID=@CreateUserID,CreateTime=@CreateTime,TotalPrice=@TotalPrice,FinalPrice=@FinalPrice,Remark=@Remark,AuditStatus=@AuditStatus 
                    where ID=@ID",
                    new
                    {
                        ID = dto.OrderID,
                        CreateUserID = dto.CreateUserID,
                        CreateTime = DateTime.Now,
                        FinalPrice = finalPrice,
                        TotalPrice = totalPrice,
                        Remark = dto.Remark,
                        AuditStatus = auditStatus,
                    }, _transaction);

                Task task2 = _connection.ExecuteAsync(
                    @"insert into [SmartOrderDetail]([ID],[OrderID],[ChargeID],[Price],[Num],[FinalPrice],[RestNum],[SetID],SetNum) 
                    values(@ID,@OrderID,@ChargeID,@Price,@Num,@FinalPrice,@RestNum,@SetID,@SetNum)", detailList, _transaction, 30, CommandType.Text);

                await Task.WhenAll(task1, task2);

                result.Message = "更新订单成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 预约界面获取已购买项目
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<AppointCharges>>> GetAppointCharges(long customerID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<AppointCharges>>();
            result.Message = "";
            result.ResultType = IFlyDogResultType.Success;

            await TryExecuteAsync(async () =>
            {
                result.Data = await _connection.QueryAsync<AppointCharges>(
                    @"select a.ChargeID as ID,a.RestNum,c.Name ,b.CreateTime as OrderTime,d.Name as CategoryName,c.PinYin,c.Size
                    from SmartOrderDetail a
                    inner join SmartOrder b on a.OrderID=b.ID and b.CustomerID=@CustomerID and b.PaidStatus=@PaidStatus
                    inner join SmartCharge c on a.ChargeID=c.ID
                    inner join SmartChargeCategory d on c.CategoryID=d.ID
                    where a.RestNum>0", new { CustomerID = customerID, PaidStatus = PaidStatus.Paid });
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
        private async Task<AuditType> IsAudit(decimal totalPrice, decimal finalPrice, List<OrderDetailAddTemp> detailList, long hospitalID, long createUserID, long customerID)
        {
            var discount = (await _connection.QueryAsync<int?>(@"select Discount from [SmartAuditRule] where [Type]=@Type and [HospitalID]=@HospitalID and Status=@Status",
                    new { Type = RuleType.Order, HospitalID = hospitalID, Status = CommonStatus.Use }, _transaction)).FirstOrDefault();

            var totaldiscount = Convert.ToInt32(finalPrice * 100 / totalPrice);

            AuditType auditStatus = AuditType.NoApprove;
            if (totalPrice > 0)
            {
                if (discount != null && discount > 0)
                {
                    if (finalPrice == 0)
                    {
                        auditStatus = AuditType.Pending;
                    }
                    else
                    {
                        if (totaldiscount < discount)
                        {
                            auditStatus = AuditType.Pending;
                        }
                    }
                }
            }

            if (auditStatus == AuditType.Pending)
            {
                var equityDiscount = (await _connection.QueryAsync<int?>(
                    @"select top 1 b.Discount from 
                    [SmartMemberCategoryEquity] a,[SmartEquity] b,SmartCustomer c where a.EquityID=b.ID and b.Type=@Type and b.Status=@Status and c.MemberCategoryID=a.CategoryID and c.ID=@ID order by Discount desc",
                    new { ID = customerID, Status = CommonStatus.Use, Type = EquityType.Discount }, _transaction)).FirstOrDefault();
                if (equityDiscount != null && equityDiscount <= totaldiscount)
                {
                    auditStatus = AuditType.NoApprove;
                    return auditStatus;
                }

                var userDiscount = (await _connection.QueryAsync<int?>(@"select [Discount] from [SmartUserDiscount] where [UserID]=@UserID", new { UserID = createUserID }, _transaction)).FirstOrDefault();
                if (userDiscount != null && userDiscount <= totaldiscount)
                {
                    auditStatus = AuditType.NoApprove;
                    return auditStatus;
                }
                var chargeDiscount = (await _chargeDiscountService.GetChargesDiscount(hospitalID)).Data.OrderBy(u => u.ChargeID).ThenByDescending(u => u.Discount);

                var newChargesDiscount = new Dictionary<long?, int>();
                foreach (var u in chargeDiscount)
                {
                    int temp;
                    if (u.ChargeID == 0)
                    {
                        if (u.Discount <= totaldiscount)
                        {
                            auditStatus = AuditType.NoApprove;
                            break;
                        }
                    }
                    else
                    {
                        if (!newChargesDiscount.TryGetValue(u.ChargeID, out temp))
                        {
                            newChargesDiscount.Add(u.ChargeID, u.Discount);
                        }
                    }
                }

                if (auditStatus == AuditType.Pending)
                {
                    bool can = true;
                    foreach (var u in detailList)
                    {

                        int temp2;
                        if (newChargesDiscount.TryGetValue(u.ChargeID, out temp2))
                        {
                            if (temp2 > totaldiscount)
                            {
                                can = false;
                                break;
                            }
                        }
                        else
                        {
                            can = false;
                            break;
                        }
                    }

                    if (can)
                    {
                        auditStatus = AuditType.NoApprove;
                    }
                }
            }

            return auditStatus;
        }

        /// <summary>
        /// 获取未完成项目
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<NoDoneOrders>>> GetNoDoneOrders(long hospitalID, long customerID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<NoDoneOrders>>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            await TryExecuteAsync(async () =>
            {
                result.Data = await _connection.QueryAsync<NoDoneOrders>(
                    @"select a.ID as DetailID,a.ChargeID,c.Name as ChargeName,a.RestNum,a.Num,a.FinalPrice from SmartOrderDetail a 
                    inner join SmartOrder b on a.OrderID=b.ID and b.PaidStatus in (@PaidStatus,@PaidStatus2) and b.CustomerID=@CustomerID and b.HospitalID=@HospitalID
                    inner join SmartCharge c on a.ChargeID=c.ID
                    where a.RestNum>0", new { CustomerID = customerID, HospitalID = hospitalID, PaidStatus = PaidStatus.Paid, PaidStatus2 = PaidStatus.Debt });
            });

            return result;
        }

        /// <summary>
        /// 欠款订单
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<DebtOrders>>> GetDebtOrdes(DebtSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<DebtOrders>>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            string sql_where = "";
            if (dto.StartTime != null && dto.EndTime != null)
            {
                sql_where += " and a.CreateTime between @StartTime and @EndTime ";
            }
            if (dto.CustomerID > 0)
            {
                sql_where += " and a.CustomerID=@CustomerID ";
            }
            if (dto.HospitalID > 0)
            {
                sql_where += " and a.HospitalID=@HospitalID ";
            }

            await TryExecuteAsync(async () =>
            {
                result.Data = await _connection.QueryAsync<DebtOrders>(
                    string.Format(@"select a.ID,a.CustomerID,b.Name as CustomerName,c.Name as CreateUserName,a.CreateTime,
                    a.FinalPrice,a.FinalPrice-a.DebtAmount as RealAmount,a.DebtAmount,case when a.InpatientID is null then 1 else 2 end as OrderType
                    from SmartOrder a 
                    left join SmartCustomer b on a.CustomerID=b.ID
                    left join SmartUser c on a.CreateUserID=c.ID
                    where a.DebtAmount>0 and a.PaidStatus=@PaidStatus {0}", sql_where),
                    new
                    {
                        PaidStatus = PaidStatus.Debt,
                        HospitalID = dto.HospitalID,
                        CustomerID = dto.CustomerID,
                        StartTime = dto.StartTime,
                        EndTime = dto.EndTime.ToString().Replace(" 0:00:00"," 23:59:59")
                    });
            });

            return result;
        }
    }
}
