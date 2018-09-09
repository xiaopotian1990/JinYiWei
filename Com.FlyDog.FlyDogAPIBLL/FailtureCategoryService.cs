using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.Common;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.Cache;
using Com.JinYiWei.Common.DataAccess;
using Com.JinYiWei.Common.Extensions;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.FlyDog.FlyDogAPIBLL
{
    class FailtureCategoryService : BaseService, IFailtureCategoryService
    {
        private RedisStackExchangeHelper _redis = new RedisStackExchangeHelper();
        /// <summary>
        /// 添加未成交类型
        /// </summary>
        /// <param name="dto">症状信息</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(FailtureCategoryAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "未成交类型名称不能为空！";
                return result;
            }
            else if (!string.IsNullOrWhiteSpace(dto.Name) && dto.Name.Length > 20)
            {
                result.Message = "未成交类型名称最多20个字！";
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
                result.Data = _connection.Execute("insert into [SmartFailtureCategory](ID,Name,[Status],Remark) values (@ID,@Name,@Status,@Remark)",
                    new { ID = id, Name = dto.Name, Status = CommonStatus.Use, Remark = dto.Remark }, _transaction);

                var temp = new { 编号 = result.Data, 名称 = dto.Name, 备注 = dto.Remark };

                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.FailtureCategoryAdd,
                    Remark = LogType.FailtureCategoryAdd.ToDescription() + temp.ToJsonString()
                });

                CacheDelete.CategoryChange(SelectType.FailtureCategory);

                result.Message = "未成交类型添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 未成交类型修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(FailtureCategoryUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "未成交类型名称不能为空！";
                return result;
            }
            else if (!string.IsNullOrWhiteSpace(dto.Name) &&dto.Name.Length > 20)
            {
                result.Message = "未成交类型名称最多20个字！";
                return result;
            }

            if (dto.Remark.IsNullOrEmpty()) {
                dto.Remark = " ";
            }else if (!dto.Remark.IsNullOrEmpty() && dto.Remark.Length >= 50)
            {
                result.Message = "备注最多50个字！";
                return result;
            }

            TryTransaction(() =>
            {
                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();
                result.Data = _connection.Execute("update [SmartFailtureCategory] set Name = @Name, Remark = @Remark where ID = @ID", dto, _transaction);

                var temp = new { 编号 = dto.ID, 名称 = dto.Name, 备注 = dto.Remark };

                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.FailtureCategoryUpdate,
                    Remark = LogType.FailtureCategoryUpdate.ToDescription() + temp.ToJsonString()
                });

                CacheDelete.CategoryChange(SelectType.FailtureCategory);

                result.Message = "未成交类型修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result; ;
        }

        /// <summary>
        /// 未成交类型使用停用
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> StopOrUse(FailtureCategoryStopOrUse dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            TryTransaction(() =>
            {
                result.Data = _connection.Execute("update [SmartFailtureCategory] set [Status] = @Status where ID = @FailtureCategoryID", dto, _transaction);

                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.FailtureCategoryStopOrUse,
                    Remark = dto.Status.ToDescription() + "未成交类型ID：" + dto.FailtureCategoryID
                });

                CacheDelete.CategoryChange(SelectType.FailtureCategory);

                result.Message = dto.Status.ToDescription() + "成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 查询所有未成交类型
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<FailtureCategory>> Get()
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<FailtureCategory>>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<FailtureCategory>("SELECT [ID],[Name],[Remark],[Status] FROM [SmartFailtureCategory] order by Status desc,Name");
                result.Message = "未成交类型查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 根据ID查询详细未成交类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, FailtureCategory> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, FailtureCategory>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<FailtureCategory>("SELECT [ID],[Name],[Remark],[Status] FROM [SmartFailtureCategory] where ID=@ID", new { ID = id }).FirstOrDefault();
                result.Message = "未成交类型查询成功";
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

            var temp = _redis.StringGet<IEnumerable<Select>>(RedisPreKey.Category + SelectType.FailtureCategory);
            if (temp != null)
            {
                result.Data = temp;
                return result;
            }

            TryExecute(() =>
            {
                result.Data = _connection.Query<Select>("SELECT [ID],[Name] FROM [SmartFailtureCategory] where [Status]=@Status order by Name", new { Status = CommonStatus.Use });

                _redis.StringSet(RedisPreKey.Category + SelectType.FailtureCategory, result.Data);
            });

            return result;
        }
    }
}
