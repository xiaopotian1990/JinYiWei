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
using Com.IFlyDog.Common;
using Com.JinYiWei.Cache;

namespace Com.FlyDog.FlyDogAPIBLL
{
    /// <summary>
    /// 班次处理业务逻辑类
    /// </summary>
    public class SmartShiftCategoryService : BaseService, ISmartShiftCategoryService
    {

        private RedisStackExchangeHelper _redis = new RedisStackExchangeHelper();
        /// <summary>
        /// 添加班次处理
        /// </summary>
        /// <param name="dto">添加班次处理dto类</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(SmartShiftCategoryAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;
            #region 数据验证
            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "班次名称不能为空！";
                return result;
            }
            else if (!dto.Name.IsNullOrEmpty() && dto.Name.Length >= 20)
            {
                result.Message = "班次名称最多20个字！";
                return result;
            }
            #endregion

            TryTransaction(() =>
            {
                #region 判断用户权限
                int num = _connection.Query<int>("select count(ID) from SmartUser where ID=@CreateUserID", new { CreateUserID = dto.CreateUserID }, _transaction).FirstOrDefault();

                if (num == 0)
                {
                    result.Message = "操作人ID不存在！";
                    return false;
                }
                #endregion

                #region 开始数据操作动作
                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(); //生成id
                result.Data = _connection.Execute("insert into SmartShiftCategory(ID,Name,Status,Type) values (@ID,@Name,@Status,@Type)",
                    new { ID = id, Name = dto.Name, Type = dto.ShiftCategoryType,Status = dto.ShiftState}, _transaction);

                var temp = new { 编号 = result.Data, 名称 = dto.Name, 班次类型 = dto.ShiftCategoryType, 启用or停用状态 = dto.ShiftState };
                #endregion

                #region 记录日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.SmartShiftCategoryAdd,
                    Remark = LogType.SmartShiftCategoryAdd.ToDescription() + temp.ToJsonString()
                });
                #endregion
                CacheDelete.CategoryChange(SelectType.ShiftCategory);
                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            return result;
        }

        /// <summary>
        /// 查询所有班次信息数据
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<SmartShiftCategoryInfo>> Get()
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<SmartShiftCategoryInfo>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<SmartShiftCategoryInfo>("SELECT [ID],[Name],[Status],[Type] FROM [SmartShiftCategory] order by [Status] desc,[Type],Name");
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据班次id查询班次详细信息
        /// </summary>
        /// <param name="id">班次id</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, SmartShiftCategoryInfo> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, SmartShiftCategoryInfo>();

            #region 开始根据id查询班次信息动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<SmartShiftCategoryInfo>("SELECT [ID],[Name],[Status],[Type] FROM [SmartShiftCategory] where ID=@ID", new { ID = id }).FirstOrDefault();
                result.Message = "班次查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect()
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<Select>>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            var temp = _redis.StringGet<IEnumerable<Select>>(RedisPreKey.Category + SelectType.ShiftCategory);
            if (temp != null)
            {
                result.Data = temp;
                return result;
            }

            TryExecute(() =>
            {
                result.Data = _connection.Query<Select>("SELECT [ID],[Name],[Status],[Type] FROM [SmartShiftCategory]  WHERE Status=1");

                _redis.StringSet(RedisPreKey.Category + SelectType.ShiftCategory, result.Data);
            });

            return result;
        }

        /// <summary>
        /// 启用停用班次信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> SmartShiftCategoryDispose(SmartShiftCategoryDispose dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            #region 开始修改班次状态动作
            TryTransaction(() =>
            {
                #region 验证用户合法性
                int num = _connection.Query<int>("select count(ID) from SmartUser where ID=@CreateUserID", new { CreateUserID = dto.CreateUserID }, _transaction).FirstOrDefault();

                if (num == 0)
                {
                    result.Message = "操作人ID不存在！";
                    return false;
                }
                #endregion

                #region 开始修改班次状态
                result.Data = _connection.Execute("update SmartShiftCategory set [Status] = @Status where ID = @ID", dto, _transaction);

                AddOperationLog(new SmartOperationLog() { ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(), CreateTime = DateTime.Now, CreateUserID = dto.CreateUserID, Type = LogType.SmartShiftCategoryDispose });
                CacheDelete.CategoryChange(SelectType.ShiftCategory);
                result.Message = dto.Status.ToString() + "成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
                #endregion
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 更新班次信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(SmartShiftCategoryUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            #region 数据验证
            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "班次名称不能为空！";
                return result;
            }
            else if (!dto.Name.IsNullOrEmpty() && dto.Name.Length >= 20)
            {
                result.Message = "班次名称最多20个字！";
                return result;
            }
            #endregion

            TryTransaction(() =>
            {
                #region 验证用户合法性
                int num = _connection.Query<int>("select count(ID) from SmartUser where ID=@CreateUserID", new { CreateUserID = dto.CreateUserID }, _transaction).FirstOrDefault();

                if (num == 0)
                {
                    result.Message = "操作人ID不存在！";
                    return false;
                }
                #endregion

                #region 开始更新操作
                result.Data = _connection.Execute("update SmartShiftCategory set Name = @Name, [Status] = @Status, [Type] = @Type where ID = @ID", dto, _transaction);

                var temp = new { 编号 = dto.ID, 名称 = dto.Name, 状态 = dto.Status, 类型 = dto.Type };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.SmartShiftCategoryUpdate,
                    Remark = LogType.SmartShiftCategoryUpdate.ToDescription() + temp.ToJsonString()
                });
                #endregion
                CacheDelete.CategoryChange(SelectType.ShiftCategory);
                result.Message = "班次修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }
    }
}
