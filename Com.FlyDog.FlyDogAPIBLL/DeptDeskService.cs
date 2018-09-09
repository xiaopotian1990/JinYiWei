using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.Common.DataAccess;
using Com.JinYiWei.Common.Extensions;
using Com.JinYiWei.Common.Helper;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.FlyDogAPIBLL
{
    public class DeptDeskService : BaseService, IDeptDeskService
    {
        /// <summary>
        /// 今日科室上门
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<DeptVisitToday>>> GetDeptVisitTodayAsync(DeptVisitSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<DeptVisitToday>>();

            await TryExecuteAsync(async () =>
            {
                var temp = await _connection.QueryAsync<DeptVisitToday>(
                        @"select a.CustomerID,b.Name as CustomerName,a.CreateTime
                        from SmartDeptVisit a
                        inner join SmartCustomer b on a.CustomerID=b.ID
                        inner join SmartUser c on c.ID=@CreateUserID and a.DeptID=c.DeptID
                        where a.HospitalID=@HospitalID and DateDiff(dd,a.CreateTime,getdate())=0",
                        dto);
                if (dto.Name.IsNullOrEmpty())
                {
                    result.Data = temp;
                }
                else
                {
                    result.Data = temp.Where(u => u.CustomerName.Contains(dto.Name));
                }

                result.Message = "查询成功！";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 今日医院上门
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<DeptVisitToday>>> GetHospitalVisitTodayAsync(DeptVisitSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<DeptVisitToday>>();

            await TryExecuteAsync(async () =>
            {
                var temp = await _connection.QueryAsync<DeptVisitToday>(
                        @"select a.CustomerID,b.Name as CustomerName,a.CreateTime
                        from SmartDeptVisit a
                        inner join SmartCustomer b on a.CustomerID=b.ID
                        where a.HospitalID=@HospitalID and DateDiff(dd,a.CreateTime,getdate())=0",
                        dto);

                Dictionary<string, DeptVisitToday> dic = new Dictionary<string, DeptVisitToday>();

                foreach (var u in temp)
                {
                    DeptVisitToday t = new DeptVisitToday();
                    if (!dic.TryGetValue(u.CustomerID, out t))
                    {
                        dic.Add(u.CustomerID, u);
                    }
                }

                temp = dic.Values;

                if (dto.Name.IsNullOrEmpty())
                {
                    result.Data = temp;
                }
                else
                {
                    result.Data = temp.Where(u => u.CustomerName.Contains(dto.Name));
                }
                result.Message = "查询成功！";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 查询顾客可划扣项目
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<CanOperation>>> GetCanOperation(long customerID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<CanOperation>>();

            await TryExecuteAsync(async () =>
            {
                result.Data = await _connection.QueryAsync<CanOperation>(
                        @"select a.ID as DetailID,a.ChargeID,b.Name as ChargeName,f.Name as HospitalName,c.PaidTime,d.Name as ChargeCategoryName,
                        e.Name as UnitName,a.Num,a.RestNum,a.FinalPrice 
                        from SmartOrderDetail a
                        inner join SmartCharge b on a.ChargeID=b.ID and a.RestNum>0
                        inner join SmartOrder c on a.OrderID=c.ID and c.PaidStatus!=@PaidStatus and c.CustomerID=@CustomerID
                        inner join SmartChargeCategory d on b.CategoryID=d.ID
                        inner join SmartUnit e on b.UnitID=e.ID
                        inner join SmartHospital f on c.HospitalID=f.ID",
                        new { PaidStatus = PaidStatus.NotPaid, CustomerID = customerID });

                result.Message = "查询成功！";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 添加划扣
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> AddOperation(OperationAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            dto.ChargesList = dto.ChargesList.Where(u => u.Num > 0 && u.DetailID > 0);
            dto.OperationerList = dto.OperationerList.Where(u => u.PositionID > 0 && u.UserID > 0);

            if (dto.OperationerList.Count() == 0)
            {
                result.Message = "请选择医生！";
                return result;
            }
            if (dto.ChargesList.Count() == 0)
            {
                result.Message = "请选择划扣项目！";
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

            var temp = (await GetCanOperation(dto.CustomerID)).Data;

            List<Task> tasks = new List<Task>();
            await TryTransactionAsync(async () =>
            {
                var now = DateTime.Now;
                foreach (var u in dto.ChargesList)
                {
                    if (!temp.Any(m => m.DetailID == u.DetailID.ToString() && u.Num <= m.RestNum))
                    {
                        result.Message = "项目数量不足，无法划扣！";
                        return false;
                    }
                    var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();
                    tasks.Add(_connection.ExecuteAsync(
                    @"insert into [SmartOperation]([ID],[HospitalID],[Num],[CreateUserID],[CreateTime],[Remark],[OrderDetailID],[CustomerStatus],[CustomerID],ChargeID) 
                    values(@ID,@HospitalID,@Num,@CreateUserID,@CreateTime,@Remark,@OrderDetailID,@CustomerStatus,@CustomerID,@ChargeID)",
                    new
                    {
                        ID = id,
                        HospitalID = dto.HospitalID,
                        Num = u.Num,
                        CreateUserID = dto.CreateUserID,
                        CreateTime = now,
                        Remark = dto.Remark,
                        OrderDetailID = u.DetailID,
                        CustomerStatus = 0,
                        CustomerID = dto.CustomerID,
                        ChargeID = u.ChargeID
                    }, _transaction));
                    foreach (var n in dto.OperationerList)
                    {
                        tasks.Add(_connection.ExecuteAsync(
                        @"insert into [SmartOperator]([ID],[OperationID],[UserID],[PositionID]) values(@ID,@OperationID,@UserID,@PositionID)",
                        new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            OperationID = id,
                            UserID = n.UserID,
                            PositionID = n.PositionID
                        }, _transaction));
                    }

                    tasks.Add(_connection.ExecuteAsync(
                        @"update [SmartOrderDetail] set [RestNum]=[RestNum]-@Num where ID=@ID", new { ID = u.DetailID, Num = u.Num }, _transaction));

                    await NewCaculateCommission(id, dto.CustomerID, u);
                }

                await Task.WhenAll(tasks);



                result.Message = "划扣成功！";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 更新划扣
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> UpdateOperation(OperationUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;
            dto.OperationerList = dto.OperationerList.Where(u => u.PositionID > 0 && u.UserID > 0);

            if (dto.OperationerList.Count() == 0)
            {
                result.Message = "请选择医生！";
                return result;
            }

            await TryTransactionAsync(async () =>
            {
                var createUserID = (await _connection.QueryAsync<long?>(
                    @"select [CreateUserID] from [SmartOperation] where ID=@ID", new { ID = dto.ID }, _transaction)).FirstOrDefault();
                if (createUserID == null)
                {
                    result.Message = "划扣不存在！";
                    return false;
                }
                if (createUserID != dto.CreateUserID)
                {
                    result.Message = "划扣只允许创建人删除跟修改！";
                    return false;
                }
                var now = DateTime.Now;

                await _connection.ExecuteAsync(
                    @"delete from [SmartOperator] where [OperationID]=@OperationID", new { OperationID = dto.ID }, _transaction);

                result.Data = await _connection.ExecuteAsync(
                        @"insert into [SmartOperator]([ID],[OperationID],[UserID],[PositionID]) values(@ID,@OperationID,@UserID,@PositionID)",
                        dto.OperationerList.Select(u => new
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            OperationID = dto.ID,
                            UserID = u.UserID,
                            PositionID = u.PositionID
                        }), _transaction);

                result.Message = "更新划扣成功！";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 获取今日划扣列表
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<OperationToday>>> GetOperationToday(long hospitalID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<OperationToday>>();

            await TryExecuteAsync(async () =>
            {
                var lsit = new Dictionary<string, OperationToday>();

                await _connection.QueryAsync<OperationToday, Operator, OperationToday>(
                        @"select a.ID as OperationID,a.CustomerID,g.Name as CustomerName,d.Name as ChargeName,a.ChargeID,
                        a.Num,a.CreateTime,f.Name as CreateUserName,b.UserID,e.Name as UserName,b.PositionID,c.Name as PositionName 
                        from SmartOperation a
                        left join SmartOperator b on a.ID=b.OperationID
                        left join SmartPosition c on b.PositionID=c.ID
                        left join SmartCharge d on a.ChargeID=d.ID
                        left join SmartUser e on b.UserID=e.ID
                        left join SmartUser f on f.ID=a.CreateUserID
                        left join SmartCustomer g on a.CustomerID=g.ID
                        where a.HospitalID=@HospitalID and DateDiff(dd,a.CreateTime,getdate())=0 order by a.CreateTime desc",
                        (o, p) =>
                        {
                            OperationToday temp = new OperationToday();
                            if (!lsit.TryGetValue(o.OperationID, out temp))
                            {
                                lsit.Add(o.OperationID, temp = o);
                            }
                            if (p != null)
                                temp.OperatorList.Add(p);
                            return o;
                        }, new { HospitalID = hospitalID }, null, true, splitOn: "UserID");

                result.Data = lsit.Values;
                result.Message = "查询成功！";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 划扣详细
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, OperationToday>> GetOperationDetail(long ID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, OperationToday>();

            await TryExecuteAsync(async () =>
            {
                var lsit = new Dictionary<string, OperationToday>();

                await _connection.QueryAsync<OperationToday, Operator, OperationToday>(
                        @"select a.ID as OperationID,a.CustomerID,g.Name as CustomerName,d.Name as ChargeName,a.ChargeID,
                        a.Num,a.CreateTime,b.UserID,e.Name as UserName,b.PositionID,c.Name as PositionName 
                        from SmartOperation a
                        left join SmartOperator b on a.ID=b.OperationID
                        left join SmartPosition c on b.PositionID=c.ID
                        left join SmartCharge d on a.ChargeID=d.ID
                        left join SmartUser e on b.UserID=e.ID
                        left join SmartCustomer g on a.CustomerID=g.ID
                        where a.ID=@ID",
                        (o, p) =>
                        {
                            OperationToday temp = new OperationToday();
                            if (!lsit.TryGetValue(o.OperationID, out temp))
                            {
                                lsit.Add(o.OperationID, temp = o);
                            }
                            if (p != null)
                                temp.OperatorList.Add(p);
                            return o;
                        }, new { ID = ID }, null, true, splitOn: "UserID");

                result.Data = lsit.Values.FirstOrDefault();
                result.Message = "查询成功！";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 获取耗材默认详细
        /// </summary>
        /// <param name="operationID"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, DefaultChargeInfo>> GetDefaultChargeInfo(long operationID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, DefaultChargeInfo>();

            await TryExecuteAsync(async () =>
            {
                var lsit = new Dictionary<string, DefaultChargeInfo>();

                await _connection.QueryAsync<DefaultChargeInfo, DefaultProductsOfCharge, DefaultChargeInfo>(
                        @"select a.ID as OperationID,a.CustomerID,g.Name as CustomerName,d.Name as ChargeName,a.ChargeID,
                        a.Num,a.CreateTime,b.ID as OperationProductID,b.ProductID,c.Name as ProductName,e.Name as UnitName,c.Size,b.Num as MinNum,b.Status,b.WarehouseID,f.Name as WarehouseName
                        from SmartOperation a
						left join SmartOperationProduct b on a.ID=b.OperationID
						left join SmartProduct c on b.ProductID=c.ID
						left join SmartUnit e on c.UnitID=e.ID
						left join SmartWarehouse f on b.WarehouseID=f.ID
                        left join SmartCharge d on a.ChargeID=d.ID  
                        left join SmartCustomer g on a.CustomerID=g.ID
                        where a.ID=@ID",
                        (o, p) =>
                        {
                            DefaultChargeInfo temp = new DefaultChargeInfo();
                            if (!lsit.TryGetValue(o.OperationID, out temp))
                            {
                                lsit.Add(o.OperationID, temp = o);
                            }
                            if (p != null)
                                temp.Products.Add(p);
                            return o;
                        }, new { ID = operationID }, null, true, splitOn: "OperationProductID");

                result.Data = lsit.Values.FirstOrDefault();

                result.Data.DefaultProducts = await _connection.QueryAsync<DefaultProductsOfCharge>(
                    @"select a.ID as OperationID,a.ChargeID,b.Name as ChargeName,
                    f.ProductID,d.Name as ProductName,e.Name as UnitName,d.Size,f.MinNum,f.MaxNum
                    from SmartOperation a
                    inner join SmartCharge b on a.ChargeID=b.ID
                    inner join SmartChargeProductDetail f on f.ChargeID=b.ID
                    left join SmartProduct d on f.ProductID=d.ID
                    left join SmartUnit e on d.UnitID=e.ID
                    where a.ID=@ID", new { ID = operationID });

                result.Message = "查询成功！";
                result.ResultType = IFlyDogResultType.Success;
            });
            return result;
        }

        /// <summary>
        /// 添加耗材
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> AddProduct(OperationProductAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.Num <= 0)
            {
                result.Message = "耗材数量不能小于0！";
                return result;
            }


            var temp = (await GetDefaultChargeInfo(dto.OperationID)).Data;
            if (temp == null)
            {
                result.Message = "划扣记录不存在！";
                return result;
            }

            if (temp.Products.Any(u => u.ProductID == dto.ProductID.ToString()))
            {
                result.Message = "不允许添加重复耗材！";
                return result;
            }

            foreach (var u in temp.Products)
            {
                if (u.ProductID == dto.ProductID.ToString())
                {
                    result.Message = "耗材" + u.ProductName + "重复添加失败！";
                    return result;
                }
            }

            foreach (var u in temp.DefaultProducts)
            {
                if (u.ProductID == dto.ProductID.ToString())
                {
                    if (dto.Num > u.MaxNum || dto.Num < u.MinNum)
                    {
                        result.Message = String.Format("耗材{0}限制数量为{1}-{2}，添加失败！", u.ProductName, u.MinNum, u.MaxNum);
                        return result;
                    }
                }
            }

            await TryExecuteAsync(async () =>
            {
                result.Data = await _connection.ExecuteAsync(
                    @"insert into [SmartOperationProduct]([ID],[OperationID],[ProductID],[WarehouseID],[Status],[Num]) 
                    values(@ID,@OperationID,@ProductID,@WarehouseID,@Status,@Num)",
                    new
                    {
                        ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                        OperationID = dto.OperationID,
                        ProductID = dto.ProductID,
                        WarehouseID = dto.WarehouseID,
                        Status = OperationProductStatus.No,
                        Num = dto.Num
                    });


                result.Message = "添加耗材成功！";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 删除耗材
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> DeleteProduct(OperationDelete dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();

            await TryExecuteAsync(async () =>
            {
                int count = await _connection.ExecuteAsync(
                    @"select count(ID) from [SmartOperationProduct] where [ID]=@ID and [Status]=@Status", new { Status = OperationProductStatus.No, ID = dto.ID });
                if (count == 0)
                {
                    result.Message = "该耗材已发货或者不存在！";
                    return;
                }

                result.Data = await _connection.ExecuteAsync(
                    @"delete from SmartOperationProduct where ID=@ID",
                    new
                    {
                        ID = dto.ID
                    });


                result.Message = "耗材删除成功！";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 删除划扣记录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> DeleteOperation(OperationDelete dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();

            await TryTransactionAsync(async () =>
            {
                int count = await _connection.ExecuteAsync(
                    @"select count(ID) from [SmartOperationProduct] where [OperationID]=@OperationID and [Status]=@Status", new { Status = OperationProductStatus.Yes, OperationID = dto.ID }, _transaction);
                if (count == 0)
                {
                    result.Message = "已经有耗材发货，无法删除划扣记录！";
                    return false;
                }

                var detail = (await _connection.QueryAsync(
                    @"select OrderDetailID,Num,[CreateUserID] from [SmartOperation] where ID=@ID", new { ID = dto.ID }, _transaction)).FirstOrDefault();
                if (detail == null)
                {
                    result.Message = "划扣不存在！";
                    return false;
                }
                if ((long)detail.CreateUserID != dto.CreateUserID)
                {
                    result.Message = "划扣只允许创建人删除跟修改！";
                    return false;
                }

                List<Task> tasks = new List<Task>();

                tasks.Add(_connection.ExecuteAsync(
                    @"update [SmartOrderDetail] set [RestNum]=[RestNum]+@Num where ID=@ID", new { ID = (long)detail.OrderDetailID, Num = (int)detail.Num }, _transaction));

                tasks.Add(_connection.ExecuteAsync(
                    @"delete from [SmartOperation] where ID=@ID", new { ID = dto.ID }, _transaction));

                tasks.Add(_connection.ExecuteAsync(
                    @"delete from [SmartOperationProduct] where [OperationID]=@OperationID", new { OperationID = dto.ID }, _transaction));

                tasks.Add(_connection.ExecuteAsync(
                    @"delete from [SmartOperator] where [OperationID]=@OperationID", new { OperationID = dto.ID }, _transaction));

                await Task.WhenAll(tasks);

                var temp = await _connection.QueryAsync<CommissionDeleteTemp>(
                    @"select d.Name,a.Commission,a.Amount,c.Mobile,b.Name as ChargeName,a.Type
                    from [SmartCommissionRecord] a
                    left join SmartCharge b on a.[ChargeID]=b.ID
                    left join SmartCustomer c on a.PromoterID=c.ID
                    left join SmartCustomer d on a.CustomerID=d.ID
                    where a.OperationID=@OperationID", new { OperationID = dto.ID }, _transaction);

                await _connection.ExecuteAsync(
                    @"update SmartCustomer set Commission=a.Commission-b.Commission from SmartCustomer a,SmartCommissionRecord b
                    where a.ID=b.PromoterID and b.OperationID=@OperationID", new { OperationID = dto.ID }, _transaction);
                await _connection.ExecuteAsync(
                    @"delete from SmartCommissionRecord where OperationID=@OperationID", new { OperationID = dto.ID }, _transaction);

                if (temp != null && temp.Count() > 0)
                {
                    BatchSubmit batch = new BatchSubmit();
                    foreach (var u in temp)
                    {
                        string message = string.Format("您的好友{0}取消了{1}的消费，系统自动扣除您{2}花瓣！", u.Name, u.ChargeName, u.Commission);
                        batch.Data.Add(new BatchTemp() { content = message, phones = u.Mobile });
                    }

                    try
                    {
                        APIHelper _apiHelper = new APIHelper(Key.SSMApiUri, Key.SSMApiUriToken, Key.SSMAppid, Key.SSMAppsecred, Key.SSMSignKey, Key.SSMRedis);
                        var r = await _apiHelper.Post<IFlyDogResult<IFlyDogResultType, BatchSubmitResult>, BatchSubmit>("/api/SSM/BatchSubmit", batch);
                    }
                    catch (Exception e)
                    {

                    }
                }

                result.Message = "划扣删除成功！";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 计算佣金
        /// </summary>
        /// <param name="operationID"></param>
        /// <param name="customerID"></param>
        /// <param name="charges"></param>
        /// <returns></returns>
        private async Task CaculateCommission(long operationID, long customerID, List<ChargesAdd> charges)
        {
            string sql_where = "";
            if (charges.Count == 0)
            {
                sql_where += " and a.ID=" + charges.FirstOrDefault().DetailID;
            }
            else
            {
                sql_where += " and a.ID in(";
                foreach (var u in charges)
                {
                    sql_where += u.DetailID + ",";
                }
                sql_where = sql_where.Substring(0, sql_where.Length - 1);
                sql_where += ")";
            }
            var temp = await _connection.QueryAsync<CommissionTemp>(
                string.Format(@"select a.ID as DetailID,c.Name,case when b.Amount=0 then 0 else a.FinalPrice*(b.Amount-b.Commission-b.Coupon-b.Debt)/b.Amount end  as Amount,a.Num,b.CreateTime
                from SmartOrderDetail a,SmartCashier b,SmartCharge c where a.OrderID=b.OrderID and b.OrderType in (1,2) and a.ChargeID=c.ID {0}", sql_where), new { }, _transaction);

            temp = temp.Where(u => u.Amount > 0);

            int totalAmount = 0;
            string totalCharges = "";
            foreach (var u in temp)
            {
                ChargesAdd charge = charges.Where(m => m.DetailID == u.DetailID).FirstOrDefault();
                totalAmount += u.Amount * charge.Num / u.Num;
                totalCharges += u.Name + "、";
            }
            totalCharges = totalCharges.Substring(0, totalCharges.Length - 1);
            if (totalAmount == 0)
            {
                return;
            }


            var levelList = (await _connection.QueryAsync<PromoteLevel>(
                @"select [Level],[Rate] from [SmartPromoteLevel] order by Level desc", null, _transaction)).ToList();
            if (levelList == null || levelList.Count() == 0)
            {
                return;
            }

            int maxLevel = levelList.FirstOrDefault().Level;

            var options = await _connection.QueryAsync<OptionTemp>(
                    @"select Code,Value from [SmartOption] where Code in (@Code1,@Code2,@Code3)", new { Code1 = WXOption.OpenCommissionCode, Code2 = WXOption.CommissionLevelCode, Code3 = WXOption.CommissionRemindedCode }, _transaction);

            OptionTemp OpenCommissionCode = options.Where(u => u.Code == WXOption.OpenCommissionCode).FirstOrDefault();
            OptionTemp CommissionLevelCode = options.Where(u => u.Code == WXOption.CommissionLevelCode).FirstOrDefault();
            OptionTemp CommissionRemindedCode = options.Where(u => u.Code == WXOption.CommissionRemindedCode).FirstOrDefault();
            if (OpenCommissionCode != null && CommissionLevelCode != null && CommissionRemindedCode != null)
            {
                if (OpenCommissionCode.Value == OpenCommission.Open.ToString())
                {
                    maxLevel += Convert.ToInt32(CommissionLevelCode.Value);
                }
            }

            var promoterList = (await _connection.QueryAsync<PromoterTemp>(
                @"with tree as 
                ( 
                select a.Name as CustomerName,a.PromoterID,Level=1,b.Mobile as Phone from SmartCustomer a,SmartCustomer b where a.ID=@ID and a.PromoterID is not null and a.PromoterID=b.ID
                union all
                select a.Name,a.PromoterID,b.Level+1,c.Mobile as Phone from SmartCustomer a,tree b,SmartCustomer c where a.ID=b.PromoterID and a.PromoterID is not null and Level<@Level and a.PromoterID=c.ID
                ) 
                select * from tree order by Level", new { ID = customerID, Level = maxLevel }, _transaction)).ToList();

            List<CommissionRecordAdd> commissionRecord = new List<CommissionRecordAdd>();
            if (promoterList == null || promoterList.Count() == 0)
            {
                return;
            }

            DateTime now = DateTime.Now;
            int promoterCount = promoterList.Count();
            string Name = promoterList.FirstOrDefault().CustomerName;
            for (int i = 1; i <= levelList.Count(); i++)
            {
                int commission = Convert.ToInt32(totalAmount * levelList[i - 1].Rate);
                if (i > promoterCount)
                {
                    break;
                }

                commissionRecord.Add(new CommissionRecordAdd()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    Amount = totalAmount,
                    Commission = commission,
                    CreateTime = now,
                    CustomerID = customerID,
                    Level = levelList[i - 1].Level,
                    OperationID = operationID,
                    PromoterID = promoterList[levelList[i - 1].Level - 1].PromoterID,
                    Rate = levelList[i - 1].Rate,
                    Remark = levelList[i - 1].Level + '层' + Name + levelList[i - 1].Rate * 100 + "%的佣金【" + CommissionRecordType.Consume.ToDescription() + "】",
                    Type = CommissionRecordType.Consume,
                    Phone = promoterList[levelList[i - 1].Level - 1].Phone
                });

                for (int n = 1; n <= Convert.ToInt32(CommissionLevelCode.Value); n++)
                {
                    if (i + n > promoterCount)
                    {
                        break;
                    }

                    commissionRecord.Add(new CommissionRecordAdd()
                    {
                        ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                        Amount = commission,
                        Commission = Convert.ToInt32(commission * Decimal.Parse(CommissionRemindedCode.Value)),
                        CreateTime = now,
                        CustomerID = customerID,
                        Level = n,
                        OperationID = operationID,
                        PromoterID = promoterList[levelList[i - 1].Level - 1 + n].PromoterID,
                        Rate = Decimal.Parse(CommissionRemindedCode.Value),
                        Remark = CommissionRecordType.Commission.ToDescription(),
                        Type = CommissionRecordType.Commission,
                        Phone = promoterList[levelList[i - 1].Level - 1 + n].Phone
                    });
                }
            }

            Dictionary<long, PromoterTemp> dic = new Dictionary<long, PromoterTemp>();
            foreach (var u in commissionRecord)
            {
                PromoterTemp commissionTemp = new PromoterTemp();
                if (dic.TryGetValue(u.PromoterID, out commissionTemp))
                {
                    commissionTemp.Amount += u.Commission;
                }
                else
                {
                    dic.Add(u.PromoterID, new PromoterTemp() { PromoterID = u.PromoterID, Amount = u.Commission, Phone = u.Phone });
                }
            }

            BatchSubmit batch = new BatchSubmit();
            foreach (var u in dic.Values)
            {
                string message = string.Format("您的好友{0}消费了{1}等{2}个项目，合计共{3}元，您获得了{4}元佣金！", Name, totalCharges, charges.Count(), totalAmount, u.Amount);
                batch.Data.Add(new BatchTemp() { content = message, phones = u.Phone });
            }

            await _connection.ExecuteAsync(
                @"update SmartCustomer set Commission=Commission+@Amount where ID=@PromoterID", dic.Values, _transaction);
            await _connection.ExecuteAsync(
                @"insert into [SmartCommissionRecord]([ID],[OperationID],[CustomerID],[PromoterID],[Amount],[Commission],[CreateTime],[Rate],[Type],[Level],[Remark]) 
                values(@ID,@OperationID,@CustomerID,@PromoterID,@Amount,@Commission,@CreateTime,@Rate,@Type,@Level,@Remark)", commissionRecord, _transaction);

            try
            {
                APIHelper _apiHelper = new APIHelper(Key.SSMApiUri, Key.SSMApiUriToken, Key.SSMAppid, Key.SSMAppsecred, Key.SSMSignKey, Key.SSMRedis);
                var r = await _apiHelper.Post<IFlyDogResult<IFlyDogResultType, BatchSubmitResult>, BatchSubmit>("/api/SSM/BatchSubmit", batch);
            }
            catch (Exception e)
            {

            }

        }

        private async Task NewCaculateCommission(long operationID, long customerID, ChargesAdd charge)
        {
            var temp = (await _connection.QueryAsync<CommissionTemp>(
                @"select a.ID as DetailID,c.Name,case when b.Amount=0 then 0 else a.FinalPrice*(b.Amount-b.Commission-b.Coupon)/b.Amount end  as Amount,a.Num,b.CreateTime
                from SmartOrderDetail a,SmartCashier b,SmartCharge c where a.OrderID=b.OrderID and b.OrderType in (1,2) and a.ChargeID=c.ID and a.ID=@ID", new { ID = charge.DetailID }, _transaction)).FirstOrDefault();

            if (temp == null || temp.Amount <= 0)
            {
                return;
            }

            if (temp.CreateTime < DateTime.Parse("2017-03-01"))
            {
                return;
            }

            int totalAmount = temp.Amount * charge.Num / temp.Num;

            var levelList = (await _connection.QueryAsync<PromoteLevel>(
                @"select [Level],[Rate] from [SmartPromoteLevel] order by Level desc", null, _transaction)).ToList();
            if (levelList == null || levelList.Count() == 0)
            {
                return;
            }

            int maxLevel = levelList.FirstOrDefault().Level;

            var options = await _connection.QueryAsync<OptionTemp>(
                    @"select Code,Value from [SmartOption] where Code in (@Code1,@Code2,@Code3)", new { Code1 = WXOption.OpenCommissionCode, Code2 = WXOption.CommissionLevelCode, Code3 = WXOption.CommissionRemindedCode }, _transaction);

            OptionTemp OpenCommissionCode = options.Where(u => u.Code == WXOption.OpenCommissionCode).FirstOrDefault();
            OptionTemp CommissionLevelCode = options.Where(u => u.Code == WXOption.CommissionLevelCode).FirstOrDefault();
            OptionTemp CommissionRemindedCode = options.Where(u => u.Code == WXOption.CommissionRemindedCode).FirstOrDefault();
            if (OpenCommissionCode != null && CommissionLevelCode != null && CommissionRemindedCode != null)
            {
                if (OpenCommissionCode.Value.CastTo<OpenCommission>() == OpenCommission.Open)
                {
                    maxLevel += Convert.ToInt32(CommissionLevelCode.Value);
                }
            }

            var promoterList = (await _connection.QueryAsync<PromoterTemp>(
                @"with tree as 
                ( 
                select a.Name as CustomerName,a.PromoterID,Level=1,b.Mobile as Phone from SmartCustomer a,SmartCustomer b where a.ID=@ID and a.PromoterID is not null and a.PromoterID=b.ID
                union all
                select a.Name,a.PromoterID,b.Level+1,c.Mobile as Phone from SmartCustomer a,tree b,SmartCustomer c where a.ID=b.PromoterID and a.PromoterID is not null and Level<@Level and a.PromoterID=c.ID
                ) 
                select * from tree order by Level", new { ID = customerID, Level = maxLevel }, _transaction)).ToList();

            List<CommissionRecordAdd> commissionRecord = new List<CommissionRecordAdd>();
            if (promoterList == null || promoterList.Count() == 0)
            {
                return;
            }

            DateTime now = DateTime.Now;
            int promoterCount = promoterList.Count();
            string Name = promoterList.FirstOrDefault().CustomerName;
            for (int i = 1; i <= levelList.Count(); i++)
            {
                int commission = Convert.ToInt32(totalAmount * levelList[i - 1].Rate / 100);
                if (i > promoterCount)
                {
                    break;
                }

                commissionRecord.Add(new CommissionRecordAdd()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    Amount = totalAmount,
                    Commission = commission,
                    CreateTime = now,
                    CustomerID = customerID,
                    Level = levelList[i - 1].Level,
                    OperationID = operationID,
                    PromoterID = promoterList[levelList[i - 1].Level - 1].PromoterID,
                    Rate = levelList[i - 1].Rate,
                    Remark = levelList[i - 1].Level + '层' + Name + levelList[i - 1].Rate + "%的佣金【" + CommissionRecordType.Consume.ToDescription() + "】",
                    Type = CommissionRecordType.Consume,
                    Phone = promoterList[levelList[i - 1].Level - 1].Phone,
                    ChargeID = charge.ChargeID
                });

                if (OpenCommissionCode.Value.CastTo<OpenCommission>() == OpenCommission.Open)
                {
                    for (int n = 1; n <= Convert.ToInt32(CommissionLevelCode.Value); n++)
                    {
                        if (i + n > promoterCount)
                        {
                            break;
                        }

                        commissionRecord.Add(new CommissionRecordAdd()
                        {
                            ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                            Amount = commission,
                            Commission = Convert.ToInt32(commission * Decimal.Parse(CommissionRemindedCode.Value) / 100),
                            CreateTime = now,
                            CustomerID = customerID,
                            Level = n,
                            OperationID = operationID,
                            PromoterID = promoterList[levelList[i - 1].Level - 1 + n].PromoterID,
                            Rate = Decimal.Parse(CommissionRemindedCode.Value),
                            Remark = CommissionRecordType.Commission.ToDescription(),
                            Type = CommissionRecordType.Commission,
                            Phone = promoterList[levelList[i - 1].Level - 1 + n].Phone,
                            ChargeID = charge.ChargeID
                        });
                    }
                }

            }

            Dictionary<long, PromoterTemp> dic = new Dictionary<long, PromoterTemp>();
            foreach (var u in commissionRecord)
            {
                PromoterTemp commissionTemp = new PromoterTemp();
                if (dic.TryGetValue(u.PromoterID, out commissionTemp))
                {
                    commissionTemp.Amount += u.Commission;
                }
                else
                {
                    dic.Add(u.PromoterID, new PromoterTemp() { PromoterID = u.PromoterID, Amount = u.Commission, Phone = u.Phone });
                }
            }

            BatchSubmit batch = new BatchSubmit();
            foreach (var u in dic.Values)
            {
                string message = string.Format("您的好友{0}消费了{1}次{2}，合计共{3}元，您获得了{4}元佣金！", Name, charge.Num, temp.Name, totalAmount, u.Amount);
                batch.Data.Add(new BatchTemp() { content = message, phones = u.Phone });
            }

            await _connection.ExecuteAsync(
                @"update SmartCustomer set Commission=Commission+@Amount where ID=@PromoterID", dic.Values, _transaction);
            await _connection.ExecuteAsync(
                @"insert into [SmartCommissionRecord]([ID],[OperationID],[CustomerID],[PromoterID],[Amount],[Commission],[CreateTime],[Rate],[Type],[Level],[Remark],[ChargeID]) 
                values(@ID,@OperationID,@CustomerID,@PromoterID,@Amount,@Commission,@CreateTime,@Rate,@Type,@Level,@Remark,@ChargeID)", commissionRecord, _transaction);

            try
            {
                APIHelper _apiHelper = new APIHelper(Key.SSMApiUri, Key.SSMApiUriToken, Key.SSMAppid, Key.SSMAppsecred, Key.SSMSignKey, Key.SSMRedis);
                var r = await _apiHelper.Post<IFlyDogResult<IFlyDogResultType, BatchSubmitResult>, BatchSubmit>("/api/SSM/BatchSubmit", batch);
            }
            catch (Exception e)
            {

            }

        }
    }

    public class CommissionDeleteTemp
    {
        public string Name { get; set; }
        public decimal Commission { get; set; }
        public decimal Amount { get; set; }
        public string Mobile { get; set; }
        public string ChargeName { get; set; }
        public CommissionRecordType Type { get; set; }
    }

    public class CommissionTemp
    {
        public long DetailID { get; set; }
        public string Name { get; set; }
        public int Amount { get; set; }
        public int Num { get; set; }
        public DateTime CreateTime { get; set; }
    }

    public class PromoteLevel
    {
        public int Level { get; set; }
        public decimal Rate { get; set; }
    }
    public class OptionTemp
    {
        public WXOption Code { get; set; }
        public string Value { get; set; }
    }

    public class PromoterTemp
    {
        public string CustomerName { get; set; }
        public long PromoterID { get; set; }
        public int Level { get; set; }
        public int Amount { get; set; }
        public string Phone { get; set; }
    }
}
