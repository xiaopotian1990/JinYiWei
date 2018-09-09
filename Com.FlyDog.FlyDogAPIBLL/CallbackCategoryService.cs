using Com.FlyDog.IFlyDogAPIBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.Common.DataAccess;
using Dapper;
using Com.IFlyDog.APIDTO;
using Com.JinYiWei.Common.Extensions;
using Com.IFlyDog.Common;
using Com.JinYiWei.Cache;

namespace Com.FlyDog.FlyDogAPIBLL
{
    /// <summary>
    /// 回访类型设置
    /// </summary>
    public class CallbackCategoryService : BaseService, ICallbackCategoryService
    {
        private RedisStackExchangeHelper _redis = new RedisStackExchangeHelper();
        /// <summary>
        /// 回访类型添加
        /// </summary>
        public IFlyDogResult<IFlyDogResultType, int> Add(SmartCallbackCategoryAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "名称不能为空！";
                return result;
            }
            else if (!dto.Name.IsNullOrEmpty() && dto.Name.Length > 20)
            {
                result.Message = "名称最多20个字！";
                return result;
            }

            if (dto.Remark.IsNullOrEmpty())
            {
                dto.Remark = " ";
            }
            else if (!dto.Remark.IsNullOrEmpty() && dto.Remark.Length >= 50)
            {
                result.Message = "备注最多50个字！";
                return result;
            }

            TryTransaction(() =>
            {
                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();
                result.Data = _connection.Execute("insert into [SmartCallbackCategory](ID,Name,[Status],Remark) values (@ID,@Name,@Status,@Remark)",
                    new { ID = id, Name = dto.Name, Status = CommonStatus.Use, Remark = dto.Remark }, _transaction);

                var temp = new { 编号 = result.Data, 名称 = dto.Name, 备注 = dto.Remark };

                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.CallbackCategoryAdd,
                    Remark = LogType.CallbackCategoryAdd.ToDescription() + temp.ToJsonString()
                });

                CacheDelete.CategoryChange(SelectType.CallbackCategory);

                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 回访类型查询全部
        /// </summary>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<SmartCallbackCategory>> Get()
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<SmartCallbackCategory>>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            TryExecute(() =>
            {
                result.Data = _connection.Query<SmartCallbackCategory>("SELECT [ID],[Name],[Status],[Remark] FROM [SmartCallbackCategory] order by Status desc,Name");
            });
            return result;
        }

        /// <summary>
        /// 回访类型通过ID查询一条数据
        /// </summary>
        public IFlyDogResult<IFlyDogResultType, SmartCallbackCategory> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, SmartCallbackCategory>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<SmartCallbackCategory>("SELECT  [ID],[Name],[Status],[Remark] FROM [SmartCallbackCategory] where ID=@ID", new { ID = id }).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 回访类型停用及使用
        /// </summary>
        public IFlyDogResult<IFlyDogResultType, int> StopOrUse(SmartCallbackCategoryStopOrUse dto)
        {

            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            TryTransaction(() =>
            {
                result.Data = _connection.Execute("update [SmartCallbackCategory] set [Status]=@Status where ID = @CallbackID", dto, _transaction);

                //操作日志记录
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.CallbackCategoryStopOrUse,
                    Remark = dto.Status.ToDescription() + "类型ID：" + dto.CallbackID
                });

                CacheDelete.CategoryChange(SelectType.CallbackCategory);

                result.Message = dto.Status.ToDescription() + "成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 回访类型编辑
        /// </summary>
        public IFlyDogResult<IFlyDogResultType, int> Update(SmartCallbackCategoryUpdate dto)
        {

            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "名称不能为空！";
                return result;
            }
            else if (!dto.Name.IsNullOrEmpty() && dto.Name.Length > 20)
            {
                result.Message = "名称最多20个字！";
                return result;
            }

            if (dto.Remark.IsNullOrEmpty())
            {
                dto.Remark = " ";
            }
            else if (!dto.Remark.IsNullOrEmpty() && dto.Remark.Length >= 50)
            {
                result.Message = "备注最多50个字！";
                return result;
            }

            TryTransaction(() =>
            {

                result.Data = _connection.Execute("update SmartCallbackCategory set Name = @Name,  Remark = @Remark where ID = @ID", dto, _transaction);

                var temp = new { 编号 = dto.ID, 名称 = dto.Name, 备注 = dto.Remark };

                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.CallbackCategoryUpdate,
                    Remark = LogType.CallbackCategoryUpdate.ToDescription() + temp.ToJsonString()
                });

                CacheDelete.CategoryChange(SelectType.CallbackCategory);

                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

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

            var temp = _redis.StringGet<IEnumerable<Select>>(RedisPreKey.Category + SelectType.CallbackCategory);
            if (temp != null)
            {
                result.Data = temp;
                return result;
            }

            TryExecute(() =>
            {
                result.Data = _connection.Query<Select>("SELECT [ID],[Name] FROM [SmartCallbackCategory] where [Status]=@Status order by Name", new { Status = CommonStatus.Use });

                _redis.StringSet(RedisPreKey.Category + SelectType.CallbackCategory, result.Data);
            });

            return result;
        }
    }
}
