using Com.FlyDog.IFlyDogAPIBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Dapper;
using Com.JinYiWei.Common.DataAccess;
using Com.JinYiWei.Common.Extensions;

namespace Com.FlyDog.FlyDogAPIBLL
{
    /// <summary>
    /// 投诉处理业务逻辑
    /// </summary>
    public class ComplainService : BaseService, IComplainService
    {
             
        
        /// <summary>
        /// 查询当前医院投诉处理
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<ComplainInfo>>> Get(ComplainSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<ComplainInfo>>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                int startRow = dto.PageSize * (dto.PageNum - 1);
                int endRow = dto.PageSize;

                result.Data = new Pages<IEnumerable<ComplainInfo>>();
                string sql = string.Empty;
                string sql2 = string.Empty;

                sql = @" SELECT sc.ID,sc.CustomerID,sct.Name AS CustomerName,Convert(varchar(30),sc.CreateTime,23) AS CreateTime,sc.Content,sc.Status,sc.FinishTime,sc.Solution,su.Name AS FinishUserName FROM [dbo].[SmartComplain] AS sc
                  LEFT JOIN dbo.SmartCustomer AS sct ON sc.CustomerID=sct.ID
                  LEFT JOIN dbo.SmartUser AS su ON sc.FinishUserID=su.ID
                  WHERE 1=1 ";

                sql2 = @" SELECT COUNT(sc.ID) FROM [dbo].[SmartComplain] AS sc
                          LEFT JOIN dbo.SmartCustomer AS sct ON sc.CustomerID=sct.ID
                          LEFT JOIN dbo.SmartUser AS su ON sc.FinishUserID=su.ID
                          WHERE 1=1";

                if (!string.IsNullOrWhiteSpace(dto.CustomerName))
                {
                    sql += @" AND sct.Name LIKE '%" + dto.CustomerName + "%'";
                    sql2 += @" AND sct.Name LIKE '%" + dto.CustomerName + "%'";
                }

                if (!string.IsNullOrWhiteSpace(dto.FinishUserName))
                {
                    sql += @" AND su.Name LIKE '%" + dto.FinishUserName + "%'";
                    sql2 += @" AND su.Name LIKE '%" + dto.FinishUserName + "%'";
                }

                if (!string.IsNullOrWhiteSpace(dto.BeginTime) && !string.IsNullOrWhiteSpace(dto.EndTime))
                {
                    string endTime = dto.EndTime + " 23:59:59";
                    sql += @" And sc.CreateTime between '" + dto.BeginTime + "' and '" + endTime + "'";
                    sql2 += @" And sc.CreateTime between '" + dto.BeginTime + "' and '" + endTime + "'";
                }

                sql2 += " AND sc.HospitalID=" + dto.HospitalID + "";
                sql += " AND sc.HospitalID=" + dto.HospitalID + "";
                sql += " ORDER by ID desc OFFSET " + startRow + " ROWS FETCH NEXT " + endRow + " ROWS only";

                result.Data.PageDatas = _connection.Query<ComplainInfo>(sql);
                result.Data.PageTotals = _connection.Query<int>(sql2, dto).FirstOrDefault();

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据id获取投诉信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, ComplainInfo> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, ComplainInfo>();
            TryExecute(() =>
            {
                result.Data = _connection.Query<ComplainInfo>(" SELECT sc.ID,sct.Name AS CustomerName,sc.Content,sc.Solution FROM [dbo].[SmartComplain] AS sc LEFT JOIN dbo.SmartCustomer AS sct ON sc.CustomerID = sct.ID WHERE sc.ID = @ID", new { ID = id }).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 更新投诉处理
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(ComplainUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            #region 数据验证
            ///判断DTO是否为空
            if (string.IsNullOrWhiteSpace(dto.ID.ToString()))
            {
                result.Message = "数据ID异常!";
                return result;
            }

            if (string.IsNullOrWhiteSpace(dto.Solution))
            {
                result.Message = "处理内容不能为空!";
                return result;
            } else if (!string.IsNullOrWhiteSpace(dto.Solution)&&dto.Solution.Length>500) {
                result.Message = "处理结果长度不能超过500个字符!";
                return result;
            }
            #endregion

            TryTransaction(() =>
            {
                result.Data = _connection.Execute("UPDATE [SmartComplain] SET FinishTime=@FinishTime,Solution=@Solution,FinishUserID=@FinishUserID,Status=1 WHERE ID=@ID", new {
                    FinishTime=DateTime.Now,
                    Solution=dto.Solution,
                    FinishUserID=dto.FinishUserID,
                    ID=dto.ID
                }, _transaction);

                //操作日志记录
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = Convert.ToInt64(dto.FinishUserID),
                    Type = LogType.ComplainUpdate,
                    Remark = LogType.ComplainUpdate.ToDescription() + dto.ToJsonString()
                });

                result.Message = "处理成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }


    }
}
