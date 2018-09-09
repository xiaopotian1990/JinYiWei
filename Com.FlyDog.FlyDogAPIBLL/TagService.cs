using Com.IFlyDog.CommonDTO;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using Com.JinYiWei.Common.DataAccess;
using Com.JinYiWei.Common.Extensions;
using Com.IFlyDog.APIDTO;
using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.Common;
using Com.JinYiWei.Cache;

namespace Com.FlyDog.FlyDogAPIBLL
{
    /// <summary>
    /// 顾客标签相关服务
    /// </summary>
    public class TagService : BaseService, ITagService
    {
        private RedisStackExchangeHelper _redis = new RedisStackExchangeHelper();
        /// <summary>
        /// 添加顾客标签
        /// </summary>
        /// <param name="dto">顾客标签信息</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(TagAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.Content.IsNullOrEmpty())
            {
                result.Message = "名称不能为空！";
                return result;
            }
            else if (dto.Content.Length > 20)
            {
                result.Message = "名称最多20个字！";
                return result;
            }


            TryTransaction(() =>
            {
                int num = _connection.Query<int>("select count(ID) from SmartUser where ID=@CreateUserID", new { CreateUserID = dto.CreateUserID }, _transaction).FirstOrDefault();

                if (num == 0)
                {
                    result.Message = "操作人ID不存在！";
                    return false;
                }

                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();

                result.Data = _connection.Execute("insert into [SmartTag]([ID],[Content],[Status]) values (@ID,@Content,@Status)",
                    new { ID = id, Content = dto.Content, Status = CommonStatus.Use }, _transaction);

                var temp = new { 编号 = id, 名称 = dto.Content};

                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.TagAdd,
                    Remark = LogType.TagAdd.ToDescription() + temp.ToJsonString()
                });

                CacheDelete.CategoryChange(SelectType.TagCategory);

                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 更新顾客标签信息
        /// </summary>
        /// <param name="dto">顾客标签信息</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(TagUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.Content.IsNullOrEmpty())
            {
                result.Message = "名称不能为空！";
                return result;
            }
            else if (dto.Content.Length > 20)
            {
                result.Message = "名称最多20个字！";
                return result;
            }


            TryTransaction(() =>
            {
                int num = _connection.Query<int>("select count(ID) from SmartUser where ID=@CreateUserID", new { CreateUserID = dto.CreateUserID }, _transaction).FirstOrDefault();

                if (num == 0)
                {
                    result.Message = "操作人ID不存在！";
                    return false;
                }

                result.Data = _connection.Execute("update [SmartTag] set Content = @Content where ID = @ID", dto, _transaction);

                var temp = new { 编号 = dto.ID, 名称 = dto.Content };

                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.TagUpdate,
                    Remark = LogType.TagUpdate.ToDescription() + temp.ToJsonString()
                });

                CacheDelete.CategoryChange(SelectType.TagCategory);

                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 顾客标签停用
        /// </summary>
        /// <param name="dto">参数集</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> StopOrUse(TagStopOrUse dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            TryTransaction(() =>
            {
                int num = _connection.Query<int>("select count(ID) from SmartUser where ID=@CreateUserID", new { CreateUserID = dto.CreateUserID }, _transaction).FirstOrDefault();

                if (num == 0)
                {
                    result.Message = "操作人ID不存在！";
                    return false; 
                }

                result.Data = _connection.Execute("update [SmartTag] set [Status] = @Status where ID = @ID", dto, _transaction);

                AddOperationLog(new SmartOperationLog()
                { ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now, CreateUserID = dto.CreateUserID,
                    Type = LogType.TagStopOrUse,
                    Remark = dto.Status.ToDescription() + "ID：" + dto.ID
                });

                CacheDelete.CategoryChange(SelectType.TagCategory);

                result.Message = dto.Status.ToDescription() + "成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 查询所有顾客标签
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Tag>> Get()
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<Tag>>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<Tag>("SELECT [ID],[Content],[Status] FROM [SmartTag] order by Status desc,Content");
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 查询顾客标签详细
        /// </summary>
        /// <param name="id">顾客标签ID</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, Tag> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, Tag>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<Tag>("SELECT [ID],[Content],[Status] FROM [SmartTag] where ID=@ID", new { ID = id }).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 查询所有可用的标签
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Tag>> GetByIsOk()
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<Tag>>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<Tag>("SELECT [ID],[Content],[Status] FROM [SmartTag] WHERE Status=1");
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
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

            var temp = _redis.StringGet<IEnumerable<Select>>(RedisPreKey.Category + SelectType.TagCategory);
            if (temp != null)
            {
                result.Data = temp;
                return result;
            }

            TryExecute(() =>
            {
                result.Data = _connection.Query<Select>("SELECT [ID],[Content] as Name FROM [SmartTag] where [Status]=@Status order by Name", new { Status = CommonStatus.Use });

                _redis.StringSet(RedisPreKey.Category + SelectType.TagCategory, result.Data);
            });

            return result;
        }
    }
}
