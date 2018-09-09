using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.Common.DataAccess;
using Com.JinYiWei.Common.Extensions;
using System;
using System.Collections.Generic;
using Dapper;
using System.Linq;
using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.Common;
using Com.JinYiWei.Cache;

namespace Com.FlyDog.FlyDogAPIBLL
{
    /// <summary>
    /// 投诉类型
    /// </summary>
    public class ComplainCategoryService : BaseService, IComplainCategoryService
    {
        private RedisStackExchangeHelper _redis = new RedisStackExchangeHelper();
        /// <summary>
        /// 添加投诉
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(SmartComplainCategoryAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;
            ///判断DTO是否为空
            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "投诉反馈名称不可为空";
                return result;
            }
            else if (!string.IsNullOrWhiteSpace(dto.Name)&&dto.Name.Length > 20)
            {
                result.Message = "投诉反馈名称不可超过20字!";
                return result;
            }
            if (dto.Remark.IsNullOrEmpty()) {
                dto.Remark = " ";
            }else  if (!dto.Remark.IsNullOrEmpty() && dto.Remark.Length >= 50)
            {
                result.Message = "描述最多50字!";
                return result;
            }

            TryTransaction(() =>
            {
                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();
                result.Data = _connection.Execute("insert into SmartComplainCategory(ID,Name,Status,Remark) values (@ID,@Name,@Status,@Remark)",
                             new { ID = id, Name = dto.Name, Status = CommonStatus.Use, Remark = dto.Remark }, _transaction);
                var temp = new { 编号 = result.Data, 名称 = dto.Name, 备注 = dto.Remark };

                //操作日志记录
                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.ComplainCategoryAdd,
                    Remark = LogType.ComplainCategoryAdd.ToDescription() + temp.ToJsonString()
                });

                CacheDelete.CategoryChange(SelectType.ComplainCategory);

                result.Message = "投诉信息添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }
        /// <summary>
        /// 查询所有投诉
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<SmartComplainCategory>> Get()
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<SmartComplainCategory>>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<SmartComplainCategory>("SELECT [ID],[Name],[Remark],[Status] FROM [SmartComplainCategory] order by Status desc,Name");
                result.Message = "投诉类型查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }
        /// <summary>
        /// 通过ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, SmartComplainCategory> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, SmartComplainCategory>();

            TryExecute(() =>
            {
                //根据ID查询一条数据
                result.Data = _connection.Query<SmartComplainCategory>("SELECT [ID],[Name],[Remark],[Status] FROM [SmartComplainCategory] where ID=@ID", new { ID = id }).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }
        /// <summary>
        /// 停用使用
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> StopOrUse(SmartComplainCategoryStopOrUse dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;


            TryTransaction(() =>
            {
                result.Data = _connection.Execute("update [SmartComplainCategory] set [Status]=@Status where ID = @ComplainID", dto, _transaction);

                //操作日志记录
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.ComplainCategoryStopOrUse,
                    Remark = dto.Status.ToDescription() + "类型ID：" + dto.ComplainID
                });

                CacheDelete.CategoryChange(SelectType.ComplainCategory);

                result.Message = dto.Status.ToDescription() + "成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }
        /// <summary>
        /// 修改投诉
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(SmartComplainCategoryUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "投诉反馈名称不可为空";
                return result;
            }
            else if (!string.IsNullOrWhiteSpace(dto.Name) && dto.Name.Length > 20)
            {
                result.Message = "投诉反馈名称不可超过20字!";
                return result;
            }
            if (dto.Remark.IsNullOrEmpty())
            {
                dto.Remark = " ";
            }
            else if (!dto.Remark.IsNullOrEmpty() && dto.Remark.Length >= 50)
            {
                result.Message = "描述最多50字!";
                return result;
            }


            TryTransaction(() =>
            {
                result.Data = _connection.Execute("update SmartComplainCategory set Name = @Name,  Remark = @Remark where ID = @ID", dto, _transaction);

                var temp = new { 编号 = dto.ID, 名称 = dto.Name, 备注 = dto.Remark };

                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.ComplainCategoryUpdate,
                    Remark = LogType.ComplainCategoryUpdate.ToDescription() + temp.ToJsonString()
                });

                CacheDelete.CategoryChange(SelectType.ComplainCategory);

                result.Message = "投诉类型修改成功";
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

            var temp = _redis.StringGet<IEnumerable<Select>>(RedisPreKey.Category + SelectType.ComplainCategory);
            if (temp != null)
            {
                result.Data = temp;
                return result;
            }

            TryExecute(() =>
            {
                result.Data = _connection.Query<Select>("SELECT [ID],[Name] FROM [SmartComplainCategory] where [Status]=@Status order by Name", new { Status = CommonStatus.Use });

                _redis.StringSet(RedisPreKey.Category + SelectType.ComplainCategory, result.Data);
            });

            return result;
        }
    }
}
