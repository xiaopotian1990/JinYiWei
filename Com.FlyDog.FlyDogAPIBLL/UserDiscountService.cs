using Com.FlyDog.IFlyDogAPIBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.Common.Extensions;
using Com.JinYiWei.Common.DataAccess;
using Dapper;

namespace Com.FlyDog.FlyDogAPIBLL
{
    /// <summary>
    /// 用户折扣业务处理
    /// </summary>
    public class UserDiscountService : BaseService, IUserDiscountService
    {
        /// <summary>
        /// 添加用户折扣
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(UserDiscountAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;
            #region 数据验证
            if (dto.UserID.IsNullOrEmpty())
            {
                result.Message = "请选择用户！";
                return result;
            }

            if (dto.Discount.IsNullOrEmpty())
            {
                result.Message = "请输入折扣！";
                return result;
            } else if (!dto.Discount.IsNullOrEmpty()&&int.Parse(dto.Discount)>100) {
                result.Message = "折扣值不能超过100！";
                return result;
            }
            #endregion

            #region 开启事物操作
            TryTransaction(() =>
            {

                #region 开始数据操作动作
                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(); //生成id
                result.Data = _connection.Execute("insert into SmartUserDiscount(ID,UserID,Discount,Status) values (@ID,@UserID,@Discount,@Status)",
                    new { ID = id, UserID = dto.UserID, Discount =dto.Discount, Status = dto.Status }, _transaction);

                var temp = new { 编号 = result.Data };
                #endregion

                #region 记录日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                     CreateUserID=dto.CreateUserID,
                    Type = LogType.UserDiscountAdd,
                    Remark = LogType.UserDiscountAdd.ToDescription() + temp.ToJsonString()
                });
                #endregion
                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 获取全部用户折扣信息
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<UserDiscountInfo>> Get()
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<UserDiscountInfo>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<UserDiscountInfo>("SELECT sud.ID,sud.UserID,su.Name AS UserName,sud.Discount,sud.Status FROM dbo.SmartUserDiscount AS sud inner JOIN dbo.SmartUser AS su ON sud.UserID = su.ID ORDER BY sud.Status DESC");
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 获取全部用户折扣信息（分页）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<UserDiscountInfo>>> GetPage(UserDiscountSelect dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<UserDiscountInfo>>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                int startRow = dto.PageSize * (dto.PageNum - 1);
                int endRow = dto.PageSize;

                result.Data = new Pages<IEnumerable<UserDiscountInfo>>();
                string sql = string.Empty;
                string sql2 = string.Empty;

                sql = @"SELECT sud.ID,sud.UserID,su.Name AS UserName,sud.Discount,sud.Status FROM dbo.SmartUserDiscount AS sud LEFT JOIN dbo.SmartUser AS su ON sud.UserID = su.ID where 1=1";

                sql2 = @" SELECT COUNT(sud.ID)AS Count FROM dbo.SmartUserDiscount AS sud LEFT JOIN dbo.SmartUser AS su ON sud.UserID = su.ID  WHERE 1 = 1";

                sql += " ORDER BY sud.Status DESC OFFSET " + startRow + " ROWS FETCH NEXT " + endRow + " ROWS only";

                result.Data.PageDatas = _connection.Query<UserDiscountInfo>(sql);

                result.Data.PageTotals = _connection.Query<int>(sql2, dto).FirstOrDefault();

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据id获取用户折扣详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, UserDiscountInfo> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, UserDiscountInfo>();

            #region 开始根据id查询单位信息动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<UserDiscountInfo>("SELECT sud.ID,sud.UserID,su.Name AS UserName,sud.Discount,sud.Status FROM dbo.SmartUserDiscount AS sud LEFT JOIN dbo.SmartUser AS su ON sud.UserID = su.ID WHERE sud.ID=@ID", new { ID = id }).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 修改用户折扣信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(UserDiscountUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            #region 数据验证
            if (dto.UserID.IsNullOrEmpty())
            {
                result.Message = "请选择用户！";
                return result;
            }

            if (dto.Discount.IsNullOrEmpty())
            {
                result.Message = "请输入折扣！";
                return result;
            }
            else if (!dto.Discount.IsNullOrEmpty() && Convert.ToDouble(dto.Discount) > 100)
            {
                result.Message = "折扣值不能超过100！";
                return result;
            }
            #endregion

            TryTransaction(() =>
            {
                #region 开始更新操作
                result.Data = _connection.Execute("update SmartUserDiscount set UserID = @UserID,Discount=@Discount,Status=@Status where ID = @ID", new { ID = dto.ID, UserID = dto.UserID, Discount = dto.Discount, Status = dto.Status }, _transaction);

                var temp = new { 编号 = dto.ID };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                     CreateUserID=dto.CreateUserID,
                    Type = LogType.UserDiscountUpdate,
                    Remark = LogType.UserDiscountUpdate.ToDescription() + temp.ToJsonString()
                });
                #endregion

                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }
    }
}
