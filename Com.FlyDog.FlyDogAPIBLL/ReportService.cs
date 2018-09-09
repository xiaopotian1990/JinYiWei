using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.FlyDogAPIBLL
{
    public class ReportService : BaseService, IReportService
    {
        #region Failture
        /// <summary>
        /// 未成交报表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<ReportFailture>>>> FailturePages(ReportFailtureSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<ReportFailture>>>();
            result.Data = new Pages<IEnumerable<ReportFailture>>();
            result.ResultType = IFlyDogResultType.Failed;

            string sql_where = "";
            #region sql
            if (dto.StartTime != null && dto.EndTime != null)
            {
                if (dto.StartTime > dto.EndTime)
                {
                    result.Message = "开始时间不能大于结束时间！";
                    return result;
                }
                dto.EndTime = dto.EndTime.Value.AddDays(1);
                sql_where += " and a.CreateTime between @StartTime and @EndTime ";
            }

            if (dto.CustomerID > 0)
            {
                sql_where += " and a.CustomerID=@CustomerID ";
            }
            if (dto.CatogoryID > 0)
            {
                sql_where += " and a.CategoryID=@CatogoryID ";
            }
            if (dto.HospitalID > 0)
            {
                sql_where += " and a.HospitalID=@HospitalID ";
            }
            if (dto.CreateUserID > 0)
            {
                sql_where += " and a.CreateUserID=@CreateUserID ";
            }
            #endregion
            result.Data.PageNum = dto.PageNum;
            result.Data.PageSize = dto.PageSize;

            int startRow = dto.PageSize * (dto.PageNum - 1);
            int endRow = dto.PageSize;

            await TryExecuteAsync(async () =>
            {
                var task1 = _connection.QueryAsync<ReportFailture>(
                    string.Format(@"select a.CreateTime,a.CustomerID,c.Name as CustomerName,b.Name as CategoryName,d.Name as HospitalName,e.Name as CreateUserName,a.Content 
                    from SmartFailture a,SmartFailtureCategory b,SmartCustomer c,SmartHospital d,SmartUser e 
                    where a.CategoryID=b.ID and a.CreateUserID=e.ID and e.HospitalID=d.ID and a.CustomerID=c.ID {0}
                    order by a.CreateTime  
                    OFFSET {1} ROWS FETCH NEXT {2} ROWS only", sql_where, startRow, endRow), dto);
                var task2 = _connection.QueryAsync<int>(
                    string.Format(@"select count(a.ID) 
                    from SmartFailture a,SmartFailtureCategory b,SmartCustomer c,SmartHospital d,SmartUser e 
                    where a.CategoryID=b.ID and a.CreateUserID=e.ID and e.HospitalID=d.ID and a.CustomerID=c.ID {0}", sql_where), dto);

                result.Data.PageDatas = await task1;
                result.Data.PageTotals = (await task2).FirstOrDefault();

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }
        /// <summary>
        /// 未成交报表，导出Excel
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<ReportFailture>>> Failture(ReportFailtureSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<ReportFailture>>();
            result.ResultType = IFlyDogResultType.Failed;

            string sql_where = "";
            #region sql
            if (dto.StartTime != null && dto.EndTime != null)
            {
                if (dto.StartTime > dto.EndTime)
                {
                    result.Message = "开始时间不能大于结束时间！";
                    return result;
                }
                dto.EndTime = dto.EndTime.Value.AddDays(1);
                sql_where += " and a.CreateTime between @StartTime and @EndTime ";
            }

            if (dto.CustomerID > 0)
            {
                sql_where += " and a.CustomerID=@CustomerID ";
            }
            if (dto.CatogoryID > 0)
            {
                sql_where += " and a.CategoryID=@CatogoryID ";
            }
            if (dto.HospitalID > 0)
            {
                sql_where += " and a.HospitalID=@HospitalID ";
            }
            if (dto.CreateUserID > 0)
            {
                sql_where += " and a.CreateUserID=@CreateUserID ";
            }
            #endregion

            await TryExecuteAsync(async () =>
            {
                result.Data = await _connection.QueryAsync<ReportFailture>(
                    string.Format(@"select a.CreateTime,a.CustomerID,c.Name as CustomerName,b.Name as CategoryName,d.Name as HospitalName,e.Name as CreateUserName,a.Content 
                    from SmartFailture a,SmartFailtureCategory b,SmartCustomer c,SmartHospital d,SmartUser e 
                    where a.CategoryID=b.ID and a.CreateUserID=e.ID and e.HospitalID=d.ID and a.CustomerID=c.ID {0}
                    order by a.CreateTime ", sql_where), dto);

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 未成交类型统计
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<ReportFailtureCount>>> FailtureCount(ReportFailtureSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<ReportFailtureCount>>();
            result.ResultType = IFlyDogResultType.Failed;

            string sql_where = "";
            #region sql
            if (dto.StartTime != null && dto.EndTime != null)
            {
                if (dto.StartTime > dto.EndTime)
                {
                    result.Message = "开始时间不能大于结束时间！";
                    return result;
                }
                dto.EndTime = dto.EndTime.Value.AddDays(1);
                sql_where += " and a.CreateTime between @StartTime and @EndTime ";
            }

            if (dto.HospitalID > 0)
            {
                sql_where += " and a.HospitalID=@HospitalID ";
            }
            if (dto.CreateUserID > 0)
            {
                sql_where += " and a.CreateUserID=@CreateUserID ";
            }
            #endregion

            await TryExecuteAsync(async () =>
            {
                result.Data = await _connection.QueryAsync<ReportFailtureCount>(
                    string.Format(@"select a.CategoryID,b.Name as CategoryName,count(a.ID) over(partition by a.CategoryID,b.Name) as Num, cast((count(a.ID) over(partition by a.CategoryID,b.Name))*1.0/(count(a.ID) over()) as numeric(8,2))  as Per
                    from SmartFailture a,SmartFailtureCategory b,SmartHospital d,SmartUser e 
                    where a.CategoryID=b.ID and a.CreateUserID=e.ID and e.HospitalID=d.ID {0} 
                    union all
					select 0,'合计',count(a.ID),100
					from SmartFailture a,SmartFailtureCategory b,SmartHospital d,SmartUser e 
                    where a.CategoryID=b.ID and a.CreateUserID=e.ID and e.HospitalID=d.ID {0}", sql_where), dto);

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        #endregion

        #region DebtCashier
        /// <summary>
        /// 集团还款明细
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<ReportDebtCashier>>>> DebtCashierPages(ReportDebtCashierSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<ReportDebtCashier>>>();
            result.Data = new Pages<IEnumerable<ReportDebtCashier>>();
            result.ResultType = IFlyDogResultType.Failed;

            string sql_where = "";
            #region sql
            if (dto.StartTime != null && dto.EndTime != null)
            {
                if (dto.StartTime > dto.EndTime)
                {
                    result.Message = "开始时间不能大于结束时间！";
                    return result;
                }
                dto.EndTime = dto.EndTime.Value.AddDays(1);
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

            dto.OrderType = OrderType.Debt;

            #endregion
            result.Data.PageNum = dto.PageNum;
            result.Data.PageSize = dto.PageSize;

            int startRow = dto.PageSize * (dto.PageNum - 1);
            int endRow = dto.PageSize;

            await TryExecuteAsync(async () =>
            {
                var task1 = _connection.QueryAsync<ReportDebtCashier>(
                    string.Format(@"select a.CreateTime,a.CustomerID,b.Name as CustomerName,d.Name as HospitalName,c.Name as CreateUserName,a.No,
                    a.OrderID,a.Amount,a.Cash,a.Card,a.Cash+a.Card as RealAmount,a.Debt,a.Remark
                    from SmartCashier a,SmartCustomer b,SmartUser c,SmartHospital d where a.CustomerID=b.ID and a.CreateUserID=c.ID and a.HospitalID=d.ID
                    and a.OrderType=@OrderType {0}
                    order by a.CreateTime  
                    OFFSET {1} ROWS FETCH NEXT {2} ROWS only", sql_where, startRow, endRow), dto);
                var task2 = _connection.QueryAsync<int>(
                    string.Format(@"select count(a.ID)
                    from SmartCashier a,SmartCustomer b,SmartUser c,SmartHospital d where a.CustomerID=b.ID and a.CreateUserID=c.ID and a.HospitalID=d.ID
                    and a.OrderType=@OrderType {0}", sql_where), dto);

                result.Data.PageDatas = await task1;
                result.Data.PageTotals = (await task2).FirstOrDefault();

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 集团还款明细，导出Excel
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<ReportDebtCashier>>> DebtCashier(ReportDebtCashierSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<ReportDebtCashier>>();
            result.ResultType = IFlyDogResultType.Failed;

            string sql_where = "";
            #region sql
            if (dto.StartTime != null && dto.EndTime != null)
            {
                if (dto.StartTime > dto.EndTime)
                {
                    result.Message = "开始时间不能大于结束时间！";
                    return result;
                }
                dto.EndTime = dto.EndTime.Value.AddDays(1);
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
            #endregion

            dto.OrderType = OrderType.Debt;

            await TryExecuteAsync(async () =>
            {
                result.Data = await _connection.QueryAsync<ReportDebtCashier>(
                    string.Format(@"select a.CreateTime,a.CustomerID,b.Name as CustomerName,d.Name as HospitalName,c.Name as CreateUserName,a.No,
                    a.OrderID,a.Amount,a.Cash,a.Card,a.Cash+a.Card as RealAmount,a.Debt,a.Remark
                    from SmartCashier a,SmartCustomer b,SmartUser c,SmartHospital d where a.CustomerID=b.ID and a.CreateUserID=c.ID and a.HospitalID=d.ID
                    and a.OrderType=@OrderType {0}
                    order by a.CreateTime ", sql_where), dto);

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 还款日合计
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<ReportDebtCashierDay>>> DebtCashierDay(ReportDebtCashierSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<ReportDebtCashierDay>>();

            result.ResultType = IFlyDogResultType.Failed;

            string sql_where = "";
            #region sql
            if (dto.StartTime != null && dto.EndTime != null)
            {
                if (dto.StartTime > dto.EndTime)
                {
                    result.Message = "开始时间不能大于结束时间！";
                    return result;
                }
                dto.EndTime = dto.EndTime.Value.AddDays(1);
                sql_where += " and a.CreateTime between @StartTime and @EndTime ";
            }

            if (dto.HospitalID > 0)
            {
                sql_where += " and a.HospitalID=@HospitalID ";
            }

            dto.OrderType = OrderType.Debt;

            #endregion

            await TryExecuteAsync(async () =>
            {
                result.Data = await _connection.QueryAsync<ReportDebtCashierDay>(
                    string.Format(@"select CONVERT(varchar(100), a.CreateTime, 23) as CreateTime,count(a.ID) as Num,SUM(Cash) as Cash,SUM(a.Card) as Card,SUM(Cash)+SUM(a.Card) as DealAmount,
                    count(distinct a.CustomerID) as CustomerNum
                    from SmartCashier a,SmartHospital b where a.HospitalID=b.ID and a.OrderType=@OrderType {0} group by CONVERT(varchar(100), a.CreateTime, 23)                     
                    union all
                    select '合计',count(a.ID) as Num,SUM(Cash) as Cash,SUM(a.Card) as Card,SUM(Cash)+SUM(a.Card) as DealAmount,
                    count(distinct a.CustomerID) as CustomerNum
                    from SmartCashier a,SmartHospital b where a.HospitalID=b.ID and a.OrderType=@OrderType {0} 
                    ", sql_where), dto);

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 集团还款各医院统计
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<ReportDebtCashierDay>>> DebtCashierDayByHospital(ReportDebtCashierSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<ReportDebtCashierDay>>();

            result.ResultType = IFlyDogResultType.Failed;

            string sql_where = "";
            #region sql
            if (dto.StartTime != null && dto.EndTime != null)
            {
                if (dto.StartTime > dto.EndTime)
                {
                    result.Message = "开始时间不能大于结束时间！";
                    return result;
                }
                dto.EndTime = dto.EndTime.Value.AddDays(1);
                sql_where += " and a.CreateTime between @StartTime and @EndTime ";
            }

            dto.OrderType = OrderType.Debt;

            #endregion

            await TryExecuteAsync(async () =>
            {
                result.Data = await _connection.QueryAsync<ReportDebtCashierDay>(
                    string.Format(@"select b.Name as HospitalName,count(a.ID) as Num,SUM(Cash) as Cash,SUM(a.Card) as Card,SUM(Cash)+SUM(a.Card) as DealAmount,
                    count(distinct a.CustomerID) as CustomerNum
                    from SmartCashier a,SmartHospital b where a.HospitalID=b.ID and a.OrderType=@OrderType {0} group by b.Name 
                    union all
                    select '合计',count(a.ID) as Num,SUM(Cash) as Cash,SUM(a.Card) as Card,SUM(Cash)+SUM(a.Card) as DealAmount,
                    count(distinct a.CustomerID) as CustomerNum
                    from SmartCashier a,SmartHospital b where a.HospitalID=b.ID and a.OrderType=@OrderType {0}", sql_where), dto);

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        #endregion

        #region Operation
        /// <summary>
        /// 工作量明细
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<ReportOperation>>>> OperationPages(ReportOperationSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<ReportOperation>>>();
            result.Data = new Pages<IEnumerable<ReportOperation>>();
            result.ResultType = IFlyDogResultType.Failed;

            string sql_where = "";
            #region sql
            if (dto.StartTime != null && dto.EndTime != null)
            {
                if (dto.StartTime > dto.EndTime)
                {
                    result.Message = "开始时间不能大于结束时间！";
                    return result;
                }
                dto.EndTime = dto.EndTime.Value.AddDays(1);
                sql_where += " and a.CreateTime between @StartTime and @EndTime ";
            }

            if (dto.OperatorID > 0)
            {
                sql_where += " and c.UserID=@OperatorID ";
            }
            if (dto.HospitalID > 0)
            {
                sql_where += " and a.HospitalID=@HospitalID ";
            }
            if (dto.PositionID > 0)
            {
                sql_where += " and c.PositionID=@PositionID ";
            }
            if (dto.ChargeID > 0)
            {
                sql_where += " and a.ChargeID=@ChargeID ";
            }
            if (dto.ItemID > 0)
            {
                sql_where += " and j.ItemID=@ItemID ";
            }
            if (dto.ItemGroupID > 0)
            {
                sql_where += " and i.GroupID=@ItemGroupID ";
            }

            #endregion
            result.Data.PageNum = dto.PageNum;
            result.Data.PageSize = dto.PageSize;

            int startRow = dto.PageSize * (dto.PageNum - 1);
            int endRow = dto.PageSize;

            await TryExecuteAsync(async () =>
            {
                var task1 = _connection.QueryAsync<ReportOperation>(
                    string.Format(@"select a.CreateTime,a.CustomerID,b.Name as CustomerName,g.Name as HospitalName,d.Name as Operator,e.Name as Position,i.Name as Item,k.Name as ItemGroup,a.ChargeID,f.Name as ChargeName,h.Name Unit,a.Num
                    from SmartOperation a,SmartCustomer b,SmartOperator c,SmartUser d,SmartPosition e,SmartCharge f,SmartHospital g,SmartUnit h,SmartItem i,SmartItemChargeDetail j,SmartItemGroup k
                    where a.CustomerID=b.ID and a.ID=c.OperationID and c.UserID=d.ID and c.PositionID=e.ID and a.ChargeID=f.ID and a.HospitalID=g.ID and f.UnitID=h.ID and a.ChargeID=j.ChargeID and j.ItemID=i.ID and i.GroupID=k.ID
                    {0}
                    order by a.CreateTime  
                    OFFSET {1} ROWS FETCH NEXT {2} ROWS only", sql_where, startRow, endRow), dto);
                var task2 = _connection.QueryAsync<int>(
                    string.Format(@"select count(a.ID)
                    from SmartOperation a,SmartCustomer b,SmartOperator c,SmartUser d,SmartPosition e,SmartCharge f,SmartHospital g,SmartUnit h,SmartItem i,SmartItemChargeDetail j,SmartItemGroup k
                    where a.CustomerID=b.ID and a.ID=c.OperationID and c.UserID=d.ID and c.PositionID=e.ID and a.ChargeID=f.ID and a.HospitalID=g.ID and f.UnitID=h.ID and a.ChargeID=j.ChargeID and j.ItemID=i.ID and i.GroupID=k.ID
                    {0}", sql_where), dto);

                result.Data.PageDatas = await task1;
                result.Data.PageTotals = (await task2).FirstOrDefault();

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 工作量明细，导出Excel
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<ReportOperation>>> Operation(ReportOperationSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<ReportOperation>>();
            result.ResultType = IFlyDogResultType.Failed;

            string sql_where = "";
            #region sql
            if (dto.StartTime != null && dto.EndTime != null)
            {
                if (dto.StartTime > dto.EndTime)
                {
                    result.Message = "开始时间不能大于结束时间！";
                    return result;
                }
                dto.EndTime = dto.EndTime.Value.AddDays(1);
                sql_where += " and a.CreateTime between @StartTime and @EndTime ";
            }

            if (dto.OperatorID > 0)
            {
                sql_where += " and c.UserID=@OperatorID ";
            }
            if (dto.HospitalID > 0)
            {
                sql_where += " and a.HospitalID=@HospitalID ";
            }
            if (dto.PositionID > 0)
            {
                sql_where += " and c.PositionID=@PositionID ";
            }
            if (dto.ChargeID > 0)
            {
                sql_where += " and a.ChargeID=@ChargeID ";
            }
            if (dto.ItemID > 0)
            {
                sql_where += " and j.ItemID=@ItemID ";
            }
            if (dto.ItemGroupID > 0)
            {
                sql_where += " and i.GroupID=@ItemGroupID ";
            }
            #endregion

            await TryExecuteAsync(async () =>
            {
                result.Data = await _connection.QueryAsync<ReportOperation>(
                    string.Format(@"select a.CreateTime,a.CustomerID,b.Name as CustomerName,g.Name as HospitalName,d.Name as Operator,e.Name as Position,i.Name as Item,k.Name as ItemGroup,a.ChargeID,f.Name as ChargeName,h.Name Unit,a.Num
                    from SmartOperation a,SmartCustomer b,SmartOperator c,SmartUser d,SmartPosition e,SmartCharge f,SmartHospital g,SmartUnit h,SmartItem i,SmartItemChargeDetail j,SmartItemGroup k
                    where a.CustomerID=b.ID and a.ID=c.OperationID and c.UserID=d.ID and c.PositionID=e.ID and a.ChargeID=f.ID and a.HospitalID=g.ID and f.UnitID=h.ID and a.ChargeID=j.ChargeID and j.ItemID=i.ID and i.GroupID=k.ID
                    {0}
                    order by a.CreateTime ", sql_where), dto);

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }
        #endregion

        #region Age
        /// <summary>
        /// 年龄明细
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<ReportAge>>>> AgePages(ReportAgeSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<ReportAge>>>();
            result.Data = new Pages<IEnumerable<ReportAge>>();
            result.ResultType = IFlyDogResultType.Failed;

            string sql_where = "";
            #region sql
            if (dto.StartTime != null && dto.EndTime != null)
            {
                if (dto.StartTime > dto.EndTime)
                {
                    result.Message = "开始时间不能大于结束时间！";
                    return result;
                }
                dto.EndTime = dto.EndTime.Value.AddDays(1);
                sql_where += " and a.CreateTime between @StartTime and @EndTime ";
            }

            if (dto.ChannelID > 0)
            {
                sql_where += " and a.ChannelID=@ChannelID ";
            }
            if (dto.SymptomID > 0)
            {
                sql_where += " and a.CurrentConsultSymptomID=@SymptomID ";
            }
            if (dto.MemberCategoryID > 0)
            {
                sql_where += " and a.MemberCategoryID=@MemberCategoryID ";
            }
            if (dto.Age > 0)
            {
                sql_where += " and datediff(year,a.Birthday,getdate())=@Age ";
            }
            

            #endregion
            result.Data.PageNum = dto.PageNum;
            result.Data.PageSize = dto.PageSize;

            int startRow = dto.PageSize * (dto.PageNum - 1);
            int endRow = dto.PageSize;

            await TryExecuteAsync(async () =>
            {
                var task1 = _connection.QueryAsync<ReportAge>(
                    string.Format(@"select a.ID as CustomerID,a.Name as CustomerName,datediff(year,a.Birthday,getdate()) as Age,a.Gender,d.Name as ChannelName,e.Name as SymptomName,
                    b.Name as MemberCategoryName,case when a.FirstDealTime is null then 0 else 1 end as DealType,case when a.FirstVisitTime is null then 0 else 1 end as ComeType
                    from SmartCustomer a,SmartMemberCategory b,SmartChannel d,SmartSymptom e 
                    where a.ChannelID=d.ID and a.CurrentConsultSymptomID=e.ID and a.MemberCategoryID=b.ID 
                    {0}
                    order by a.CreateTime  
                    OFFSET {1} ROWS FETCH NEXT {2} ROWS only", sql_where, startRow, endRow), dto);
                var task2 = _connection.QueryAsync<int>(
                    string.Format(@"select count(a.ID) 
                    from SmartCustomer a,SmartMemberCategory b,SmartChannel d,SmartSymptom e 
                    where a.ChannelID=d.ID and a.CurrentConsultSymptomID=e.ID and a.MemberCategoryID=b.ID 
                    {0}", sql_where), dto);

                result.Data.PageDatas = await task1;
                result.Data.PageTotals = (await task2).FirstOrDefault();

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 年龄明细，导出Excel
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<ReportAge>>> Age(ReportAgeSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<ReportAge>>();
            result.ResultType = IFlyDogResultType.Failed;

            string sql_where = "";
            #region sql
            if (dto.StartTime != null && dto.EndTime != null)
            {
                if (dto.StartTime > dto.EndTime)
                {
                    result.Message = "开始时间不能大于结束时间！";
                    return result;
                }
                dto.EndTime = dto.EndTime.Value.AddDays(1);
                sql_where += " and a.CreateTime between @StartTime and @EndTime ";
            }

            if (dto.ChannelID > 0)
            {
                sql_where += " and a.ChannelID=@ChannelID ";
            }
            if (dto.SymptomID > 0)
            {
                sql_where += " and a.CurrentConsultSymptomID=@SymptomID ";
            }
            if (dto.MemberCategoryID > 0)
            {
                sql_where += " and a.MemberCategoryID=@MemberCategoryID ";
            }
            if (dto.Age > 0)
            {
                sql_where += " and datediff(year,a.Birthday,getdate())=@Age ";
            }


            #endregion

            await TryExecuteAsync(async () =>
            {
                result.Data = await _connection.QueryAsync<ReportAge>(
                    string.Format(@"select a.ID as CustomerID,a.Name as CustomerName,datediff(year,a.Birthday,getdate()) as Age,a.Gender,d.Name as ChannelName,e.Name as SymptomName,
                    b.Name as MemberCategoryName,case when a.FirstDealTime is null then 0 else 1 end as DealType,case when a.FirstVisitTime is null then 0 else 1 end as ComeType
                    from SmartCustomer a,SmartMemberCategory b,SmartChannel d,SmartSymptom e 
                    where a.ChannelID=d.ID and a.CurrentConsultSymptomID=e.ID and a.MemberCategoryID=b.ID 
                    {0}
                    order by a.CreateTime ", sql_where), dto);

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }
        #endregion

        #region 现金流明细
        /// <summary>
        /// 现金流明细
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<ReportCashDetail>>>> CashDetailPages(ReportAgeSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<ReportCashDetail>>>();
            result.Data = new Pages<IEnumerable<ReportCashDetail>>();
            result.ResultType = IFlyDogResultType.Failed;

            string sql_where = "";
            #region sql
            if (dto.StartTime != null && dto.EndTime != null)
            {
                if (dto.StartTime > dto.EndTime)
                {
                    result.Message = "开始时间不能大于结束时间！";
                    return result;
                }
                dto.EndTime = dto.EndTime.Value.AddDays(1);
                sql_where += " and a.CreateTime between @StartTime and @EndTime ";
            }

            if (dto.ChannelID > 0)
            {
                sql_where += " and a.ChannelID=@ChannelID ";
            }
            if (dto.SymptomID > 0)
            {
                sql_where += " and a.CurrentConsultSymptomID=@SymptomID ";
            }
            if (dto.MemberCategoryID > 0)
            {
                sql_where += " and a.MemberCategoryID=@MemberCategoryID ";
            }
            if (dto.Age > 0)
            {
                sql_where += " and datediff(year,a.Birthday,getdate())=@Age ";
            }


            #endregion
            result.Data.PageNum = dto.PageNum;
            result.Data.PageSize = dto.PageSize;

            int startRow = dto.PageSize * (dto.PageNum - 1);
            int endRow = dto.PageSize;

            await TryExecuteAsync(async () =>
            {
                var task1 = _connection.QueryAsync<ReportCashDetail>(
                    string.Format(@"select a.ID as CustomerID,a.Name as CustomerName,datediff(year,a.Birthday,getdate()) as Age,a.Gender,d.Name as ChannelName,e.Name as SymptomName,
                    b.Name as MemberCategoryName,case when a.FirstDealTime is null then 0 else 1 end as DealType,case when a.FirstVisitTime is null then 0 else 1 end as ComeType
                    from SmartCustomer a,SmartMemberCategory b,SmartChannel d,SmartSymptom e 
                    where a.ChannelID=d.ID and a.CurrentConsultSymptomID=e.ID and a.MemberCategoryID=b.ID 
                    {0}
                    order by a.CreateTime  
                    OFFSET {1} ROWS FETCH NEXT {2} ROWS only", sql_where, startRow, endRow), dto);
                var task2 = _connection.QueryAsync<int>(
                    string.Format(@"select count(a.ID) 
                    from SmartCustomer a,SmartMemberCategory b,SmartChannel d,SmartSymptom e 
                    where a.ChannelID=d.ID and a.CurrentConsultSymptomID=e.ID and a.MemberCategoryID=b.ID 
                    {0}", sql_where), dto);

                result.Data.PageDatas = await task1;
                result.Data.PageTotals = (await task2).FirstOrDefault();

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 现金流明细，导出Excel
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<ReportCashDetail>>> CashDetail(ReportAgeSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<ReportCashDetail>>();
            result.ResultType = IFlyDogResultType.Failed;

            string sql_where = "";
            #region sql
            if (dto.StartTime != null && dto.EndTime != null)
            {
                if (dto.StartTime > dto.EndTime)
                {
                    result.Message = "开始时间不能大于结束时间！";
                    return result;
                }
                dto.EndTime = dto.EndTime.Value.AddDays(1);
                sql_where += " and a.CreateTime between @StartTime and @EndTime ";
            }

            if (dto.ChannelID > 0)
            {
                sql_where += " and a.ChannelID=@ChannelID ";
            }
            if (dto.SymptomID > 0)
            {
                sql_where += " and a.CurrentConsultSymptomID=@SymptomID ";
            }
            if (dto.MemberCategoryID > 0)
            {
                sql_where += " and a.MemberCategoryID=@MemberCategoryID ";
            }
            if (dto.Age > 0)
            {
                sql_where += " and datediff(year,a.Birthday,getdate())=@Age ";
            }


            #endregion

            await TryExecuteAsync(async () =>
            {
                result.Data = await _connection.QueryAsync<ReportCashDetail>(
                    string.Format(@"select a.ID as CustomerID,a.Name as CustomerName,datediff(year,a.Birthday,getdate()) as Age,a.Gender,d.Name as ChannelName,e.Name as SymptomName,
                    b.Name as MemberCategoryName,case when a.FirstDealTime is null then 0 else 1 end as DealType,case when a.FirstVisitTime is null then 0 else 1 end as ComeType
                    from SmartCustomer a,SmartMemberCategory b,SmartChannel d,SmartSymptom e 
                    where a.ChannelID=d.ID and a.CurrentConsultSymptomID=e.ID and a.MemberCategoryID=b.ID 
                    {0}
                    order by a.CreateTime ", sql_where), dto);

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }
        #endregion
    }
}
