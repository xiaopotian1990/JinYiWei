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
    /// <summary>
    /// 知识分类
    /// </summary>
    public class KnowledgeCategoryService : BaseService, IKnowledgeCategoryService
    {
        private RedisStackExchangeHelper _redis = new RedisStackExchangeHelper();
        /// <summary>
        /// 添加知识分类
        /// </summary>
        /// <param name="dto">知识分类</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(KnowledgeCategoryAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "名称不能为空！";
                return result;
            }
            else if (dto.Name.Length > 20)
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
                result.Message = "备注最多50个字符！";
                return result;
            }

            TryTransaction(() =>
            {

                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();

                result.Data = _connection.Execute("insert into SmartKnowledgeCategory(ID,Name,Remark,OpenStatus) values (@ID,@Name,@Remark,@OpenStatus)",
                    new { ID = id, Name = dto.Name, Remark = dto.Remark, OpenStatus = dto.OpenStatus }, _transaction);

                var temp = new { 编号 = result.Data, 名称 = dto.Name, 备注 = dto.Remark };

                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    CreateUserID=dto.CreateUserID,
                    Type = LogType.KnowledgeCategoryAdd,
                    Remark = LogType.KnowledgeCategoryAdd.ToDescription() + temp.ToJsonString()
                });

                CacheDelete.CategoryChange(SelectType.KnowledgeCategory);

                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            return result;
        }

        /// <summary>
        /// 知识分类修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(KnowledgeCategoryUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "名称不能为空！";
                return result;
            }
            else if (dto.Name.Length > 20)
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
                result.Message = "备注最多50个字符！";
                return result;
            }

            TryTransaction(() =>
            {
                result.Data = _connection.Execute("update SmartKnowledgeCategory set Name = @Name, Remark = @Remark,OpenStatus=@OpenStatus where ID = @ID", new { Name = dto.Name, Remark = dto.Remark, OpenStatus = dto.OpenStatus, ID = dto.ID }, _transaction);

                var temp = new { 编号 = dto.ID, 名称 = dto.Name, 备注 = dto.Remark };

                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                     CreateUserID=dto.CreateUserID,
                    Type = LogType.KnowledgeCategoryUpdate,
                    Remark = LogType.KnowledgeCategoryUpdate.ToDescription() + temp.ToJsonString()
                });

                CacheDelete.CategoryChange(SelectType.KnowledgeCategory);

                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 查询所有知识分类
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<KnowledgeCategory>> Get()
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<KnowledgeCategory>>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            TryExecute(() =>
            {
                result.Data = _connection.Query<KnowledgeCategory>("SELECT [ID],[Name],[Remark],[OpenStatus] FROM [SmartKnowledgeCategory] order by [OpenStatus] desc,Name");
            });

            return result;
        }

        /// <summary>
        /// 查询ID知识分类
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, KnowledgeCategory> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, KnowledgeCategory>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<KnowledgeCategory>("SELECT [ID],[Name],[Remark],[OpenStatus] FROM [SmartKnowledgeCategory] where ID=@ID", new { ID = id }).FirstOrDefault();
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

            var temp = _redis.StringGet<IEnumerable<Select>>(RedisPreKey.Category + SelectType.KnowledgeCategory);
            if (temp != null)
            {
                result.Data = temp;
                return result;
            }

            TryExecute(() =>
            {
                result.Data = _connection.Query<Select>("SELECT [ID],[Name] FROM [SmartKnowledgeCategory] where [OpenStatus]=@Status order by Name", new { Status = CommonStatus.Use });

                _redis.StringSet(RedisPreKey.Category + SelectType.KnowledgeCategory, result.Data);
            });

            return result;
        }

     
    }
}
