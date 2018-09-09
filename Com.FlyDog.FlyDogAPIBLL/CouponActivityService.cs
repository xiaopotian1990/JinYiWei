using Com.FlyDog.IFlyDogAPIBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.Common.DataAccess;
using Dapper;
using Com.JinYiWei.Common.Extensions;

namespace Com.FlyDog.FlyDogAPIBLL
{
    /// <summary>
    /// 卷活动业务处理类
    /// </summary>
    public class CouponActivityService : BaseService, ICouponActivityService
    {

        /// <summary>
        /// 查询所有的卷
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<CouponActivityInfo>> GetAllByHospitalIDGroup(string hospitalID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<CouponActivityInfo>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<CouponActivityInfo>("SELECT ID,Name,Price,Amount,Remark,CategoryID,CreateDate,Channel,Expiration,HospitalID,IsRepetition,Prefix FROM dbo.SmartCouponActivity WHERE HospitalID = @HospitalID", new { HospitalID = hospitalID });
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 添加卷活动
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(CouponActivityAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            var getAllByHospitalIDGroupData = GetAllByHospitalIDGroup(dto.HospitalID);
            List<CouponActivityInfo> listData = getAllByHospitalIDGroupData.Data.ToList();
            if (listData != null && listData.Count > 0)
            {
                if (listData.Exists(o => o.Prefix.Trim() == dto.Prefix))
                {
                    result.Message = "当前 '"+dto.Prefix+"' 前缀已存在，不能重复添加!";
                    return result;
                }
            }

            #region 验证
            if (string.IsNullOrWhiteSpace(dto.ActivityName))
            {
                result.Message = "卷活动名称不能为空!";
                return result;
            }
            else if (!string.IsNullOrWhiteSpace(dto.ActivityName) && dto.ActivityName.Length > 20)
            {
                result.Message = "卷活动名称不能超过20个字符!";
                return result;
            }

            if (string.IsNullOrWhiteSpace(dto.Price))
            {
                result.Message = "售价不能为空!";
                return result;
            }

            if (string.IsNullOrWhiteSpace(dto.Amount))
            {
                result.Message = "卷金额不能为空!";
                return result;
            }

            if (string.IsNullOrWhiteSpace(dto.CategoryID))
            {
                result.Message = "请选择卷类型!";
                return result;
            }

            if (string.IsNullOrWhiteSpace(dto.CreateDate))
            {
                result.Message = "生效日期不能为空!";
                return result;
            }
            if (string.IsNullOrWhiteSpace(dto.Expiration))
            {
                result.Message = "失效日期不能为空!";
                return result;
            }
            if (string.IsNullOrWhiteSpace(dto.Channel))
            {
                result.Message = "发放渠道不能为空!";
                return result;
            }

            if (string.IsNullOrWhiteSpace(dto.Prefix))
            {
                result.Message = "代码前缀不能为空!";
                return result;
            }

            if (dto.Remark.IsNullOrEmpty())
            {
                dto.Remark = " ";
            }
            else if (!dto.Remark.IsNullOrEmpty() && dto.Remark.Length > 100)
            {
                result.Message = "备注最多100个字符！";
                return result;
            }
            #endregion

            TryTransaction(() =>
            {

                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();
                _connection.Execute("insert into SmartCouponActivity(ID,Name,Price,Amount,Remark,CategoryID,CreateDate,Channel,Expiration,HospitalID,IsRepetition,Prefix) VALUES(@ID, @Name, @Price, @Amount, @Remark, @CategoryID, @CreateDate, @Channel, @Expiration, @HospitalID, @IsRepetition, @Prefix)",
                 new { ID = id, Name = dto.ActivityName, Price = dto.Price, Amount = dto.Amount, Remark = dto.Remark, CategoryID = dto.CategoryID, Channel = dto.Channel, Expiration = dto.Expiration, HospitalID = dto.HospitalID, CreateDate = dto.CreateDate, IsRepetition = dto.IsRepetition, Prefix = dto.Prefix }, _transaction);  //卷活动biao

                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                     CreateUserID=dto.CreateUserID,
                    Type = LogType.CouponActivityAdd,
                    Remark = LogType.CouponActivityAdd.ToDescription() + dto.ToJsonString()
                });

                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;

        }

        /// <summary>
        /// 查询所有卷活动
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<CouponActivityInfo>>> Get(CouponActivitySelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<CouponActivityInfo>>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                int startRow = dto.PageSize * (dto.PageNum - 1);
                int endRow = dto.PageSize;

                result.Data = new Pages<IEnumerable<CouponActivityInfo>>();
                string sql = string.Empty;
                string sql2 = string.Empty;

                sql = @"  SELECT sca.ID,sca.Name AS ActivityName,sca.CategoryID,scc.Name AS CouponCategoryName,sca.Amount,sca.Price,sca.Channel,
 Convert(varchar(30),sca.CreateDate,23) CreateDate,Convert(varchar(30),sca.Expiration,23) Expiration,sca.Remark,sca.IsRepetition,sca.Prefix,sca.HospitalID FROM SmartCouponActivity AS sca LEFT JOIN SmartCouponCategory AS scc ON sca.CategoryID=scc.ID WHERE 1=1";

                sql2 = @"SELECT COUNT(sca.ID) AS Count FROM SmartCouponActivity AS sca 
                        LEFT JOIN SmartCouponCategory AS scc ON sca.CategoryID=scc.ID WHERE 1=1";


                if (!string.IsNullOrWhiteSpace(dto.BeginTime) && !string.IsNullOrWhiteSpace(dto.EndTime))
                {
                    sql += @" And sca.CreateDate between '" + dto.BeginTime + "' and '" + dto.EndTime + "'";
                    sql2 += @" And sca.CreateDate between '" + dto.BeginTime + "' and '" + dto.EndTime + "'";
                }

                if (!string.IsNullOrWhiteSpace(dto.Name))
                {
                    sql += " And sca.Name LIKE '%" + dto.Name + "%'";
                    sql2 += @" And sca.Name LIKE '%" + dto.Name + "%'";
                }


                sql2 += " AND  sca.HospitalID=" + dto.HospitalID + "";
                sql += " AND sca.HospitalID=" + dto.HospitalID + "";
                sql += " ORDER by ID desc OFFSET " + startRow + " ROWS FETCH NEXT " + endRow + " ROWS only";

                result.Data.PageDatas = _connection.Query<CouponActivityInfo>(sql);

                result.Data.PageTotals = _connection.Query<int>(sql2, dto).FirstOrDefault();

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据id获取卷活动详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, CouponActivityInfo> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, CouponActivityInfo>();
            TryExecute(() =>
            {
                result.Data = _connection.Query<CouponActivityInfo>("SELECT sca.ID,sca.Name AS ActivityName,sca.Price,sca.Amount,sca.Remark,sca.CategoryID,scc.Name AS CouponCategoryName,sca.Channel, sca.CreateDate, sca.Expiration, sca.IsRepetition, sca.Prefix FROM dbo.SmartCouponActivity AS sca LEFT JOIN SmartCouponCategory AS scc ON sca.CategoryID = scc.ID WHERE sca.ID = @ID", new { ID = id }).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 删除卷活动，以及卷活动详情
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> CouponActivityDelete(CouponActivityDelete dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();

            #region 开始事物操作
            TryTransaction(() =>
            {
                #region 开始更新操作
                string sql = @"delete SmartCouponActivity where ID=@ID
                              DELETE SmartCouponActivityDetail WHERE ActivityID = @ActivityID";

                _connection.Execute(sql, new { ID = dto.ID, ActivityID = dto.ID }, _transaction);

                var temp = new { 编号 = dto.ID };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                     CreateUserID=dto.CreateUserID,
                    Type = LogType.CouponActivityDelete,
                    Remark = LogType.CouponActivityDelete.ToDescription() + temp.ToJsonString()
                });
                #endregion

                result.Message = "删除成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 更新卷活动
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(CouponActivityUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;
            #region 验证
            if (string.IsNullOrWhiteSpace(dto.ActivityName))
            {
                result.Message = "卷活动名称不能为空!";
                return result;
            }
            else if (!string.IsNullOrWhiteSpace(dto.ActivityName) && dto.ActivityName.Length > 20)
            {
                result.Message = "卷活动名称不能超过20个字符!";
                return result;
            }

            if (string.IsNullOrWhiteSpace(dto.Price))
            {
                result.Message = "售价不能为空!";
                return result;
            }

            if (string.IsNullOrWhiteSpace(dto.Amount))
            {
                result.Message = "卷金额不能为空!";
                return result;
            }

            if (string.IsNullOrWhiteSpace(dto.CategoryID))
            {
                result.Message = "请选择卷类型!";
                return result;
            }

            if (string.IsNullOrWhiteSpace(dto.CreateDate))
            {
                result.Message = "生效日期不能为空!";
                return result;
            }
            if (string.IsNullOrWhiteSpace(dto.Expiration))
            {
                result.Message = "失效日期不能为空!";
                return result;
            }
            if (string.IsNullOrWhiteSpace(dto.Channel))
            {
                result.Message = "发放渠道不能为空!";
                return result;
            }

            if (string.IsNullOrWhiteSpace(dto.Prefix))
            {
                result.Message = "代码前缀不能为空!";
                return result;
            }

            if (dto.Remark.IsNullOrEmpty())
            {
                dto.Remark = " ";
            }
            else if (!dto.Remark.IsNullOrEmpty() && dto.Remark.Length > 100)
            {
                result.Message = "备注最多100个字符！";
                return result;
            }
            #endregion

            TryTransaction(() =>
            {
                result.Data = _connection.Execute(@"UPDATE SmartCouponActivity SET Name=@Name,Price=@Price, Amount=@Amount,Channel=@Channel,CreateDate=@CreateDate,
                      Expiration = @Expiration, Remark = @Remark, CategoryID = @CategoryID, IsRepetition = @IsRepetition, Prefix = @Prefix WHERE ID = @ID", new { ID = dto.ID, Name = dto.ActivityName, Price = dto.Price, Amount = dto.Amount, Remark = dto.Remark, CategoryID = dto.CategoryID, Channel = dto.Channel, Expiration = dto.Expiration, CreateDate = dto.CreateDate, IsRepetition = dto.IsRepetition, Prefix = dto.Prefix }, _transaction);  //卷活动biao, _transaction);

                //操作日志记录
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                     CreateUserID=dto.CreateUserID,
                    Type = LogType.CouponActivityUpdate,
                    Remark = LogType.CouponActivityUpdate.ToDescription() + dto.ToJsonString()
                });

                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }
    }
}
