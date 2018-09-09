using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.JinYiWei.Common.DataAccess;
using Com.IFlyDog.Common;
using Com.FlyDog.IFlyDogAPIBLL;
using Dapper;
using System.Reflection;

namespace Com.FlyDog.FlyDogAPIBLL
{
    /// <summary>
    /// 日志业务
    /// </summary>
    public class OperationLogService : BaseService, IOperationLogService
    { 
        /// <summary>
        /// 查询当前医院的所有日志信息
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<SmartOperationLog>> Get()
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<SmartOperationLog>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<SmartOperationLog>(@"SELECT sol.ID,sol.ID AS LogID,sol.Type,sol.Remark,sol.CreateTime,sol.CreateUserID,su.Name,su.Name AS LogCreateName FROM [dbo].[SmartOperationLog] AS sol LEFT JOIN dbo.SmartUser AS su ON sol.CreateUserID = su.ID ORDER BY sol.CreateTime DESC");
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion


            return result;
        }

        /// <summary>
        /// 根据id】查询日志详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, SmartOperationLog> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, SmartOperationLog>();

            #region 开始根据id查询单位信息动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<SmartOperationLog>(@"SELECT sol.ID,sol.ID AS LogID,sol.Type,sol.Remark,sol.CreateTime,sol.CreateUserID,su.Name,su.Name AS LogCreateName FROM [dbo].[SmartOperationLog] AS sol LEFT JOIN dbo.SmartUser AS su ON sol.CreateUserID = su.ID WHERE sol.ID = @ID", new { ID = id }).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 获取日志下拉
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<OperationLogType>> GetLogSelect()
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<OperationLogType>>();

            Dictionary<string, int> enumDic = new Dictionary<string, int>();

            List<OperationLogType> list = new List<OperationLogType>();
            Type t = typeof(LogType);
            Array arrays = Enum.GetValues(t);
            for (int i = 0; i < arrays.LongLength; i++)
            {
                OperationLogType olt = new OperationLogType();
                LogType test = (LogType)arrays.GetValue(i);
                FieldInfo fieldInfo = test.GetType().GetField(test.ToString());
                object[] attribArray = fieldInfo.GetCustomAttributes(false);
                System.ComponentModel.DescriptionAttribute da = (System.ComponentModel.DescriptionAttribute)attribArray[0];     
                olt.ID = (int)test;
                olt.Name = da.Description;
                list.Add(olt);
            }
            result.Data = list;
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;
            return result;
        }

        /// <summary>
        /// 分页查询系统日志
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<SmartOperationLog>>> GetPages(OperationLogSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<SmartOperationLog>>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                int startRow = dto.PageSize * (dto.PageNum - 1);
                int endRow = dto.PageSize;

                result.Data = new Pages<IEnumerable<SmartOperationLog>>();
                string sql = string.Empty;
                string sql2 = string.Empty;

                sql = @"SELECT sol.ID,sol.ID AS LogID,sol.Type,sol.Type AS TypeValue,sol.Remark,sol.CreateTime,sol.CreateUserID,su.Name,su.Name AS LogCreateName ,su.Account FROM [dbo].[SmartOperationLog] AS sol LEFT JOIN dbo.SmartUser AS su ON sol.CreateUserID = su.ID  where 1=1";

                sql2 = @" SELECT COUNT(sol.ID) AS Count
				  FROM [dbo].[SmartOperationLog] AS sol LEFT JOIN dbo.SmartUser AS su ON sol.CreateUserID = su.ID  WHERE 1 = 1";

                if (!string.IsNullOrWhiteSpace(dto.BeginTime) && !string.IsNullOrWhiteSpace(dto.EndTime))
                {
                    string endTime = dto.EndTime + " 23:59:59";
                    sql += @" And sol.CreateTime between '" + dto.BeginTime + "' and '" + endTime + "'";
                    sql2 += @" And sol.CreateTime between '" + dto.BeginTime + "' and '" + endTime + "'";
                }

                if (!string.IsNullOrWhiteSpace(dto.Name))
                {
                    sql += @" AND su.Name LIKE '%" + dto.Name + "%'";
                    sql2 += @" AND su.Name LIKE '%" + dto.Name + "%'";
                }

                if (!string.IsNullOrWhiteSpace(dto.Account))
                {
                    sql += @" AND su.Account LIKE '%" + dto.Account + "%'";
                    sql2 += @" AND su.Account LIKE '%" + dto.Account + "%'";
                }

                if (!string.IsNullOrWhiteSpace(dto.LogType)&&dto.LogType!="-1") {
                    sql += @" AND sol.Type='" + dto.LogType + "'";
                    sql2 += @" AND sol.Type='" + dto.LogType + "'";
                }

                sql += " AND su.HospitalID='"+ dto.HospitalID+ "'";
                sql2 += " AND su.HospitalID='" + dto.HospitalID + "'";
               sql += " ORDER BY sol.CreateTime DESC OFFSET " + startRow + " ROWS FETCH NEXT " + endRow + " ROWS only";

                List<OperationLogType> logTypeData = new List<OperationLogType>();
               logTypeData =  GetLogSelect().Data.ToList();

                List<SmartOperationLog> lslog = new List<SmartOperationLog>();
                lslog = _connection.Query<SmartOperationLog>(sql).ToList();

                foreach (var item in lslog)
                {
                    foreach (var itemLogType in logTypeData)
                    {
                        if (item.TypeValue== itemLogType.ID) {
                            item.TypeName = itemLogType.Name;
                        }
                    }
                }
                result.Data.PageDatas = lslog;

                result.Data.PageTotals = _connection.Query<int>(sql2, dto).FirstOrDefault();

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
                
            });
            #endregion
            return result;

        }
    }
}
