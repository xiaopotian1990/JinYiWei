using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
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
    public class SettlementService : BaseService, ISettlementService
    {
        /// <summary>
        /// 结算时查询出的收银信息
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, CashierOfUserInfo>> GetCashier(long userID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, CashierOfUserInfo>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            CashierOfUserInfo data = new CashierOfUserInfo();

            await TryExecuteAsync(async () =>
            {
                var muti = await _connection.QueryMultipleAsync(
                    @"select a.ID,a.Cash,a.Card,a.CustomerID,c.Name as CustomerName,a.CreateTime,d.Name as CreateUserName
                    from [SmartCashier] a
                    left join SmartCustomer c on a.CustomerID=c.ID
                    left join SmartUser d on a.CreateUserID=d.ID
                    where a.Status=@Status and a.CreateUserID=@CreateUserID order by a.CreateTime

                    select e.Name +'：'+cast(sum(b.Card) as nvarchar(150)) as Amount
                    from [SmartCashier] a
                    inner join SmartCashierCardCatogoryDetail b on a.id=b.CashierID 
                    inner join SmartCardCategory e on b.CardCategoryID=e.ID
                    where a.Status=@Status and a.CreateUserID=@CreateUserID group by b.CardCategoryID,e.Name",
                    new { CreateUserID = userID, Status = CashierStatus.No });

                data.CashierOfUserList = await muti.ReadAsync<CashierOfUser>();
                data.CardCategoryList = await muti.ReadAsync<string>();

                int count = 1;
                foreach (var u in data.CashierOfUserList)
                {
                    if (count == 1)
                    {
                        data.StartTime = u.CreateTime;
                    }
                    data.Cash += u.Cash;
                    data.Card += u.Card;
                    data.EndTime = u.CreateTime;
                    count++;
                }

                data.Count = count - 1;
                result.Data = data;
            });

            return result;
        }

        /// <summary>
        /// 结算
        /// </summary>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> AddSettlement(SettlementAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            var temp = (await GetCashier(dto.CreateUserID)).Data;
            if (temp.Count == 0)
            {
                result.Message = "对不起，现在没有待结算记录！";
                return result;
            }

            await TryTransactionAsync(async () =>
            {
                Task task1 = _connection.ExecuteAsync(
                    @"insert into [SmartSettlement]([ID],[CreateUserID],[CreateTime],[StartTime],[EndTime],[Cash],[Card],[HospitalID])
                    values(@ID,@CreateUserID,@CreateTime,@StartTime,@EndTime,@Cash,@Card,@HospitalID)",
                    new
                    {
                        ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                        CreateUserID = dto.CreateUserID,
                        CreateTime = DateTime.Now,
                        StartTime = temp.StartTime,
                        EndTime = temp.EndTime,
                        Cash = temp.Cash,
                        Card = temp.Card,
                        HospitalID = dto.HospitalID
                    }, _transaction);
                Task task2 = _connection.ExecuteAsync(
                    @"update [SmartCashier] set Status=@Status where CreateUserID=@CreateUserID",
                    new
                    {
                        CreateUserID = dto.CreateUserID,
                        Status = CashierStatus.Yes
                    }, _transaction);

                await Task.WhenAll(task1, task2);

                result.Message = "结算成功！";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 结算记录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<Settlement>>>> Get(SettlementSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<Settlement>>>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            result.Data = new Pages<IEnumerable<Settlement>>();
            result.Data.PageNum = dto.PageNum;
            result.Data.PageSize = dto.PageSize;

            await TryExecuteAsync(async () =>
            {
                string sql_where = "";
                if (dto.HospitalID > 0)
                {
                    sql_where += " and a.HospitalID=@HospitalID ";
                }
                if (dto.StartTime != null && dto.EndTime != null)
                {
                    sql_where += " and a.CreateTime between @StartTime and @EndTime ";
                }
                if (!dto.Name.IsNullOrEmpty())
                {
                    sql_where += " and b.Name=@Name ";
                }

                int startRow = dto.PageSize * (dto.PageNum - 1);
                int endRow = dto.PageSize;

                result.Data.PageDatas = await _connection.QueryAsync<Settlement>(
                            string.Format(@"select a.ID,a.CreateTime,a.CreateUserID,b.Name as CreateUserName,a.StartTime,a.EndTime,
                            a.Cash,a.Card,a.Cash+a.Card as Amount
                            from SmartSettlement a,SmartUser b 
                            where a.CreateUserID=b.ID {0}
                            ORDER by a.CreateTime desc OFFSET {1} ROWS FETCH NEXT {2} ROWS only", sql_where, startRow, endRow),
                            new { StartTime = dto.StartTime, EndTime = dto.EndTime.ToString().Replace(" 0:00:00"," 23:59:59"), HospitalID = dto.HospitalID, Name = dto.Name });
                result.Data.PageTotals = (await _connection.QueryAsync<int>(
                            string.Format(@"select count(a.ID)
                            from SmartSettlement a,SmartUser b 
                            where a.CreateUserID=b.ID {0}", sql_where),
                            new { StartTime = dto.StartTime, EndTime = dto.EndTime.ToString().Replace(" 0:00:00", " 23:59:59"), HospitalID = dto.HospitalID, Name = dto.Name })).FirstOrDefault();
            });

            return result;
        }
    }
}
