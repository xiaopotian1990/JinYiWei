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
    /// 卷活动详情页
    /// </summary>
    public class CouponActivityDetailService : BaseService, ICouponActivityDetailService
    {

        /// <summary>
        /// 查询当前医院下所有卷管理下的卷活动(不分页)
        /// </summary>
        /// <param name="activityID"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<CouponActivityDetailInfo>> GetByActivityID(long hospitalID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<CouponActivityDetailInfo>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<CouponActivityDetailInfo>("SELECT scad.ID,scad.ActivityID,scad.Code,scad.CouponID FROM SmartCouponActivityDetail AS scad LEFT JOIN dbo.SmartCouponActivity AS sac ON sac.ID = scad.ActivityID WHERE sac.HospitalID = @HospitalID", new { HospitalID = hospitalID });
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 添加卷活动详情
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(CouponActivityDetailAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;
            CouponActivityService cas = new CouponActivityService();
            var activData = cas.GetByID(Convert.ToInt64(dto.ActivityID));//得到卷活动详情数据
            var activityDetailData = GetByActivityID(dto.HospitalID);//得到卷活动下的详情数据
            List<CouponActivityDetailInfo> listData = activityDetailData.Data.ToList();
            #region 验证
            if (dto.CodeBegin == 0 || dto.CodeEnd == 0)
            {
                result.Message = "编码范围不能为空!";
                return result;
            }

            if (dto.CodeBegin != 0 || dto.CodeEnd != 0)
            {
                if (dto.CodeBegin > dto.CodeEnd)
                {
                    result.Message = "编码开始范围不能大于结束范围!";
                    return result;
                }

            }

            if (dto.CodeBegin != 0 && dto.CodeEnd != 0)
            {
                string codeBegin = activData.Data.Prefix.Trim() + dto.CodeBegin.ToString();
                string codeEnd = activData.Data.Prefix.Trim() + dto.CodeEnd.ToString();
                if (listData != null && listData.Count > 0)
                {
                    if (listData.Exists(o => o.Code.Trim() == codeBegin))
                    {
                        result.Message = "当前卷活动已存在此范围的编码，不能重复添加!";
                        return result;
                    }

                    if (listData.Exists(o => o.Code.Trim() == codeEnd))
                    {
                        result.Message = "当前卷活动已存在此范围的编码，不能重复添加!";
                        return result;
                    }
                }
            }

            #endregion

            TryTransaction(() =>
        {

            var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();
            List<CouponActivityDetailTemp> details = new List<CouponActivityDetailTemp>();

            for (int i = dto.CodeBegin; i <= dto.CodeEnd; i++)
            {
                details.Add(new CouponActivityDetailTemp()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    ActivityID = dto.ActivityID,
                    Code = activData.Data.Prefix.Trim() + i
                });
            }
            _connection.Execute("insert into SmartCouponActivityDetail(ID,ActivityID,Code,CouponID) VALUES(@ID, @ActivityID, @Code, @CouponID)",
            details, _transaction);  //卷活动biao

            AddOperationLog(new SmartOperationLog()
            {
                ID = id,
                CreateTime = DateTime.Now,
                CreateUserID = dto.CreateUserID,
                Type = LogType.CouponActivityDetailAdd,
                Remark = LogType.CouponActivityDetailAdd.ToDescription() + dto.ToJsonString()
            });

            result.Message = "添加成功";
            result.ResultType = IFlyDogResultType.Success;
            return true;
        });

            return result;

        }

        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, CouponActivityDetailInfo> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, CouponActivityDetailInfo>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<CouponActivityDetailInfo>("SELECT * FROM dbo.SmartCouponActivityDetail WHERE ID=@ID", new { ID = id }).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 查询所有卷活动详情
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<CouponActivityDetailInfo>>> Get(CouponActivityDetailSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<CouponActivityDetailInfo>>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                int startRow = dto.PageSize * (dto.PageNum - 1);
                int endRow = dto.PageSize;

                result.Data = new Pages<IEnumerable<CouponActivityDetailInfo>>();
                string sql = string.Empty;
                string sql2 = string.Empty;

                sql = @"SELECT scad.ID,scad.ActivityID,scad.Code,scad.CouponID,sc.CreateTime,su.Name AS CustomerName,su.Name AS CreateUserName,sh.Name AS HospitalName FROM dbo.SmartCouponActivityDetail AS scad 
                    LEFT JOIN SmartCoupon AS  sc ON scad.CouponID=sc.ID
                    LEFT JOIN dbo.SmartUser AS su ON su.ID=sc.CustomerID
                    LEFT JOIN dbo.SmartUse AS sus ON su.ID=sc.CreateUserID
                    LEFT JOIN dbo.SmartHospital AS sh ON sh.ID=sc.HospitalID
                    WHERE 1=1";

                sql2 = @"SELECT COUNT(scad.ID) AS Count FROM dbo.SmartCouponActivityDetail AS scad 
                            LEFT JOIN SmartCoupon AS  sc ON scad.CouponID=sc.ID
                            LEFT JOIN dbo.SmartUser AS su ON su.ID=sc.CustomerID
                            LEFT JOIN dbo.SmartUse AS sus ON su.ID=sc.CreateUserID
                            LEFT JOIN dbo.SmartHospital AS sh ON sh.ID=sc.HospitalID
                            WHERE 1=1 ";

                //sql2 += " AND sc.Access=5";
                //sql += " AND sc.Access=5"; 这个条件有问题，只要是关联到这个表的，应该都是通过某种途径获取的，不用再加上等于5的判断了
                sql2 += " AND scad.ActivityID='" + dto.CategoryID + "'";
                sql += " AND scad.ActivityID='" + dto.CategoryID + "'";
                sql += " ORDER by scad.Code desc OFFSET " + startRow + " ROWS FETCH NEXT " + endRow + " ROWS only";

                result.Data.PageDatas = _connection.Query<CouponActivityDetailInfo>(sql);

                result.Data.PageTotals = _connection.Query<int>(sql2, dto).FirstOrDefault();

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> CouponActivityDetailDelete(CouponActivityDetailDelete dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();

            var smartCouPonDat = GetByID(Convert.ToInt64(dto.ID));
            if (!string.IsNullOrWhiteSpace(smartCouPonDat.Data.CouponID))
            {
                result.Message = "该卷活动已激活，不能删除!";
                return result;
            }
            #region 开始事物操作
            TryTransaction(() =>
            {
                #region 开始更新操作
                string sql = @"DELETE SmartCouponActivityDetail WHERE ID=@ID";

                _connection.Execute(sql, new { ID = dto.ID }, _transaction);

                var temp = new { 编号 = dto.ID };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.CouponActivityDetailDelete,
                    Remark = LogType.CouponActivityDetailDelete.ToDescription() + temp.ToJsonString()
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
        /// 删除全部未激活的卷活动详情
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> CouponActivityDetailDeleteAll(CouponActivityDetailDelete dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            #region 开始事物操作
            TryTransaction(() =>
            {
                #region 开始更新操作
                string sql = @"DELETE SmartCouponActivityDetail WHERE ActivityID=@ActivityID AND CouponID IS NULL";

                _connection.Execute(sql, new { ActivityID = dto.ActivityID }, _transaction);

                var temp = new { 编号 = dto.ID };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                     CreateUserID=dto.CreateUserID,
                    Type = LogType.CouponActivityDetailDelete,
                    Remark = LogType.CouponActivityDetailDelete.ToDescription() + temp.ToJsonString()
                });
                #endregion

                result.Message = "删除成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            #endregion
            return result;
        }
    }
}
