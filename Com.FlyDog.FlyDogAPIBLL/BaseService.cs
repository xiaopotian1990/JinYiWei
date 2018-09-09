using Com.IFlyDog.APIDTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Dapper;
using Com.JinYiWei.MongoDB;
using Com.JinYiWei.Common.DataAccess;
using System.Threading.Tasks;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.Common.Extensions;

namespace Com.FlyDog.FlyDogAPIBLL
{
    public class BaseService
    {
        /// <summary>
        /// database connection
        /// </summary>
        internal IDbConnection _connection;
        internal IDbTransaction _transaction;

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="dto"></param>
        public void AddOperationLog(SmartOperationLog dto)
        {
            _connection.Execute("insert into SmartOperationLog(ID,[Type],[CustomerID],[Remark],[CreateTime],[CreateUserID],[IP]) values(@ID,@Type,@CustomerID,@Remark,@CreateTime,@CreateUserID,@IP)",
                dto, _transaction);
        }

        /// <summary>
        /// 添加日志
        /// </summary>
        /// <param name="dto"></param>
        public Task<int> AddOperationLogAsync(SmartOperationLog dto)
        {
            return _connection.ExecuteAsync("insert into SmartOperationLog(ID,[Type],[CustomerID],[Remark],[CreateTime],[CreateUserID],[IP]) values(@ID,@Type,@CustomerID,@Remark,@CreateTime,@CreateUserID,@IP)",
                 dto, _transaction);
        }

        /// <summary>
        /// 数据库操作基类
        /// </summary>
        /// <param name="action">具体执行的动作</param>
        /// <param name="isWrite">是否数据库写操作</param>
        public void TryExecute(System.Action action, bool isWrite = false)
        {
            using (_connection = BaseDBRepository.GetConnection(isWrite))
            {
                action();
            }
        }


        /// <summary>
        /// 数据库操作基类
        /// </summary>
        /// <param name="action">具体执行的动作</param>
        /// <param name="isWrite">是否数据库写操作</param>
        public async Task TryExecuteAsync(Func<Task> action, bool isWrite = false)
        {
            using (_connection = BaseDBRepository.GetConnection(isWrite))
            {
                await action();
            }
        }

        /// <summary>
        /// 数据库事物操作
        /// </summary>
        /// <param name="action">具体执行的数据库事务操作</param>
        public void TryTransaction(Func<bool> action)
        {
            using (_connection = BaseDBRepository.GetConnection())
            {
                try
                {
                    _transaction = _connection.BeginTransaction();
                    bool isCan = action();
                    if (isCan)
                        _transaction.Commit();
                    else
                    {
                        _transaction.Rollback();
                    }
                }
                catch (Exception e)
                {
                    _transaction.Rollback();
                    throw e;
                }
            }
        }

        /// <summary>
        /// 数据库事物操作
        /// </summary>
        /// <param name="action">具体执行的数据库事务操作</param>
        public async Task TryTransactionAsync(Func<Task<bool>> action)
        {
            using (_connection = BaseDBRepository.GetConnection())
            {
                try
                {
                    _transaction = _connection.BeginTransaction();
                    bool isCan = await action();
                    if (isCan)
                        _transaction.Commit();
                    else
                    {
                        _transaction.Rollback();
                    }
                }
                catch (Exception e)
                {
                    _transaction.Rollback();
                    throw e;
                }
            }
        }

        /// <summary>
        /// 是否拥有顾客访问权限
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        internal async Task<bool> HasCustomerOAuthAsync(long userID, long customerID)
        {
            var num = (await _connection.QueryAsync<int>(@"with tree as
                                                  (
                                                  select distinct b.ID from SmartUserHospital a,SmartUser b where b.HospitalID=a.HospitalID and a.UserID=@UserID
                                                  union 
                                                  select distinct b.ID from SmartUserDept a,SmartUser b where a.DeptID=b.DeptID and a.UserID=@UserID
                                                  union 
                                                  select distinct OwinUserID from SmartUserUser where UserID=@UserID
                                                  union
                                                  select distinct AssignUserID from SmartTriage where CustomerID=@CustomerID and DateDiff(dd,CreateTime,getdate())=0
                                                  )
                                                  select Count(b.ID) from tree a,SmartOwnerShip b where a.ID=b.UserID and b.EndTime> getdate() and b.CustomerID=@CustomerID",
                                               new { UserID = userID, CustomerID = customerID })).FirstOrDefault();
            return num == 0 ? false : true;
        }

        /// <summary>
        /// 是否拥有顾客访问权限,带事物
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public async Task<bool> HasCustomerOAuthTransactionAsync(long userID, long customerID)
        {
            var num = (await _connection.QueryAsync<int>(@"with tree as
                                                  (
                                                  select distinct b.ID from SmartUserHospital a,SmartUser b where b.HospitalID=a.HospitalID and a.UserID=@UserID
                                                  union 
                                                  select distinct b.ID from SmartUserDept a,SmartUser b where a.DeptID=b.DeptID and a.UserID=@UserID
                                                  union 
                                                  select distinct OwinUserID from SmartUserUser where UserID=@UserID
                                                  union
                                                  select distinct AssignUserID from SmartTriage where CustomerID=@CustomerID and DateDiff(dd,CreateTime,getdate())=0
                                                  )
                                                  select Count(b.ID) from tree a,SmartOwnerShip b where a.ID=b.UserID and b.EndTime> getdate() and b.CustomerID=@CustomerID",
                                              new { UserID = userID, CustomerID = customerID }, _transaction)).FirstOrDefault();
            return num == 0 ? false : true;
        }

        /// <summary>
        /// 是否拥有顾客回访权限
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        internal async Task<bool> HasCustomerCallbackOAuthAsync(long userID, long customerID)
        {
            var num = (await _connection.QueryAsync<int>(@"with tree as
                                                  (
                                                  select distinct b.ID from SmartCallbackHospital a,SmartUser b where b.HospitalID=a.HospitalID and a.UserID=@UserID
                                                  union 
                                                  select distinct b.ID from SmartCallbackDept a,SmartUser b where a.DeptID=b.DeptID and a.UserID=@UserID
                                                  union 
                                                  select distinct OwinUserID from SmartCallbackUser where UserID=@UserID
                                                  union
                                                  select distinct AssignUserID from SmartTriage where CustomerID=@CustomerID and DateDiff(dd,CreateTime,getdate())=0
                                                  )
                                                  select Count(b.ID) from tree a,SmartOwnerShip b where a.ID=b.UserID and b.EndTime> getdate()  and b.CustomerID=@CustomerID",
                                                new { UserID = userID, CustomerID = customerID })).FirstOrDefault();
            return num == 0 ? false : true;
        }

        /// <summary>
        /// 是否拥有顾客回访权限,带事物
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        public async Task<bool> HasCustomerCalbackOAuthTransactionAsync(long userID, long customerID)
        {
            var num = (await _connection.QueryAsync<int>(@"with tree as
                                                  (
                                                  select distinct b.ID from SmartCallbackHospital a,SmartUser b where b.HospitalID=a.HospitalID and a.UserID=@UserID
                                                  union all
                                                  select distinct b.ID from SmartCallbackDept a,SmartUser b where a.DeptID=b.DeptID and a.UserID=@UserID
                                                  union all
                                                  select distinct OwinUserID from SmartCallbackUser where UserID=@UserID
                                                  union
                                                  select distinct AssignUserID from SmartTriage where CustomerID=@CustomerID and DateDiff(dd,CreateTime,getdate())=0
                                                  )
                                                  select Count(b.ID) from tree a,SmartOwnerShip b where a.ID=b.UserID and b.EndTime> getdate() and b.CustomerID=@CustomerID",
                                              new { UserID = userID, CustomerID = customerID }, _transaction)).FirstOrDefault();
            return num == 0 ? false : true;
        }


        /// <summary>
        /// 是否可以预约
        /// </summary>
        /// <param name="AppointmentDate"></param>
        /// <param name="AppointmentStartTime"></param>
        /// <param name="AppointmentEndTime"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, string>> CanAppointmentTransactionAsync(DateTime AppointmentDate, TimeSpan AppointmentStartTime, TimeSpan AppointmentEndTime)
        {
            var result = new IFlyDogResult<IFlyDogResultType, string>();
            result.ResultType = IFlyDogResultType.Failed;

            if (AppointmentDate < DateTime.Today)
            {
                result.Message = "预约日期不能小于今天";
                return result;
            }
            if (AppointmentStartTime > AppointmentEndTime)
            {
                result.Message = "开始时间不能大于终止时间";
                return result;
            }

            dynamic options = await _connection.QueryAsync(@"select Code,Value from [SmartOption] where Code in (@MakeBeginTime,@MakeEndTime)",
                    new { MakeBeginTime = Option.MakeBeginTime.CastTo<int>().ToString(), MakeEndTime = Option.MakeEndTime.CastTo<int>().ToString() }, _transaction);

            foreach (var u in options)
            {
                if (u.Code == Option.MakeBeginTime.CastTo<int>().ToString())
                {
                    if (AppointmentStartTime < TimeSpan.Parse(u.Value))
                    {
                        result.Message = "开始时间不能小于" + TimeSpan.Parse(u.Value);
                        return result;
                    }
                }
                else if (u.Code == Option.MakeEndTime.CastTo<int>().ToString())
                {
                    if (AppointmentEndTime > TimeSpan.Parse(u.Value))
                    {
                        result.Message = "终止时间不能大于" + TimeSpan.Parse(u.Value);
                        return result;
                    }
                }
            }

            result.ResultType = IFlyDogResultType.Success;
            result.Message = "可以预约";
            return result;
        }

        /// <summary>
        /// 是否可以预约
        /// </summary>
        /// <param name="AppointmentDate"></param>
        /// <param name="AppointmentStartTime"></param>
        /// <param name="AppointmentEndTime"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, string>> CanTreatAsync(DateTime AppointmentDate, TimeSpan AppointmentStartTime, TimeSpan AppointmentEndTime, long userID, long ID = 0)
        {
            var result = new IFlyDogResult<IFlyDogResultType, string>();
            result.ResultType = IFlyDogResultType.Failed;

            if (AppointmentDate < DateTime.Today)
            {
                result.Message = "预约日期不能小于今天";
                return result;
            }
            if (AppointmentStartTime > AppointmentEndTime)
            {
                result.Message = "开始时间不能大于终止时间";
                return result;
            }

            dynamic options = await _connection.QueryAsync(@"select Code,Value from [SmartOption] where Code in (@MakeBeginTime,@MakeEndTime)",
                    new { MakeBeginTime = Option.MakeBeginTime.CastTo<int>().ToString(), MakeEndTime = Option.MakeEndTime.CastTo<int>().ToString() });

            foreach (var u in options)
            {
                if (u.Code == Option.MakeBeginTime.CastTo<int>().ToString())
                {
                    if (AppointmentStartTime < TimeSpan.Parse(u.Value))
                    {
                        result.Message = "开始时间不能小于" + TimeSpan.Parse(u.Value);
                        return result;
                    }
                }
                else if (u.Code == Option.MakeEndTime.CastTo<int>().ToString())
                {
                    if (AppointmentEndTime > TimeSpan.Parse(u.Value))
                    {
                        result.Message = "终止时间不能大于" + TimeSpan.Parse(u.Value);
                        return result;
                    }
                }
            }

            string sqlWhere = "";
            if (ID != 0)
            {
                sqlWhere = " and ID!=@ID ";
            }

            var count = (await _connection.QueryAsync<int>(
                string.Format(@"select count(ID) from SmartTreat where UserID=1 and AppointmentDate=@AppointmentDate
                and (@AppointmentStartTime between AppointmentStartTime and AppointmentEndTime or @AppointmentEndTime between AppointmentStartTime and AppointmentEndTime) {0}", sqlWhere),
                new
                {
                    AppointmentDate = AppointmentDate.Date,
                    AppointmentStartTime = AppointmentStartTime,
                    AppointmentEndTime = AppointmentEndTime,
                    ID = ID
                })).FirstOrDefault();

            if (count > 0)
            {
                result.Message = "预约失败，该时间段内该医生已经有预约！";
                return result;
            }

            result.ResultType = IFlyDogResultType.Success;
            result.Message = "可以预约";
            return result;
        }

        /// <summary>
        /// 是否可以预约
        /// </summary>
        /// <param name="AppointmentDate"></param>
        /// <param name="AppointmentStartTime"></param>
        /// <param name="AppointmentEndTime"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, string>> CanSurgeryTransactionAsync(DateTime AppointmentDate, TimeSpan AppointmentStartTime, TimeSpan AppointmentEndTime, long userID, long ID = 0)
        {
            var result = new IFlyDogResult<IFlyDogResultType, string>();
            result.ResultType = IFlyDogResultType.Failed;

            if (AppointmentDate < DateTime.Today)
            {
                result.Message = "预约日期不能小于今天";
                return result;
            }
            if (AppointmentStartTime > AppointmentEndTime)
            {
                result.Message = "开始时间不能大于终止时间";
                return result;
            }

            dynamic options = await _connection.QueryAsync(@"select Code,Value from [SmartOption] where Code in (@MakeBeginTime,@MakeEndTime)",
                    new { MakeBeginTime = Option.MakeBeginTime.CastTo<int>().ToString(), MakeEndTime = Option.MakeEndTime.CastTo<int>().ToString() }, _transaction);

            foreach (var u in options)
            {
                if (u.Code == Option.MakeBeginTime.CastTo<int>().ToString())
                {
                    if (AppointmentStartTime < TimeSpan.Parse(u.Value))
                    {
                        result.Message = "开始时间不能小于" + TimeSpan.Parse(u.Value);
                        return result;
                    }
                }
                else if (u.Code == Option.MakeEndTime.CastTo<int>().ToString())
                {
                    if (AppointmentEndTime > TimeSpan.Parse(u.Value))
                    {
                        result.Message = "终止时间不能大于" + TimeSpan.Parse(u.Value);
                        return result;
                    }
                }
            }

            string sqlWhere = "";
            if (ID != 0)
            {
                sqlWhere = " and ID!=@ID ";
            }

            var count = (await _connection.QueryAsync<int>(
                string.Format(@"select count(ID) from SmartSurgery where UserID=@UserID and AppointmentDate=@AppointmentDate
                and (@AppointmentStartTime between AppointmentStartTime and AppointmentEndTime or @AppointmentEndTime between AppointmentStartTime and AppointmentEndTime) {0}", sqlWhere),
                new
                {
                    AppointmentDate = AppointmentDate.Date,
                    AppointmentStartTime = AppointmentStartTime,
                    AppointmentEndTime = AppointmentEndTime,
                    UserID = userID,
                    ID = ID
                }, _transaction)).FirstOrDefault();

            if (count > 0)
            {
                result.Message = "预约失败，该时间段内该医生已经有预约！";
                return result;
            }

            result.ResultType = IFlyDogResultType.Success;
            result.Message = "可以预约";
            return result;
        }

        /// <summary>
        /// 判断顾客今日是否上门
        /// </summary>
        /// <param name="customerID"></param>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        public async Task<bool> IsCome(long customerID, long hospitalID)
        {
            var num = (await _connection.QueryAsync<int>(@"SELECT count([ID]) FROM [SmartVisit] where CustomerID=@CustomerID and HospitalID=@HospitalID and CreateTime between @StartTime and @EndTime",
                                             new { CustomerID = customerID, HospitalID = hospitalID, StartTime = DateTime.Today, EndTime = DateTime.Today.AddDays(1) }, _transaction)).FirstOrDefault();
            return num == 0 ? false : true;
        }
    }
}
