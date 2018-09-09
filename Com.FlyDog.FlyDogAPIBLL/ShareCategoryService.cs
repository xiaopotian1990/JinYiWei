using Com.FlyDog.IFlyDogAPIBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.Common.Extensions;
using Dapper;
using Com.JinYiWei.Common.DataAccess;
using Com.IFlyDog.Common;
using Com.JinYiWei.Cache;

namespace Com.FlyDog.FlyDogAPIBLL
{
    /// <summary>
    /// 分享家业务逻辑
    /// </summary>
    public class ShareCategoryService : BaseService, IShareCategoryService
    {
        private RedisStackExchangeHelper _redis = new RedisStackExchangeHelper();
        /// <summary>
        /// 查询当前分享家表中的最大值信息
        /// </summary>
        /// <returns></returns>
        private IFlyDogResult<IFlyDogResultType, ShareCategoryInfo> GetMemberMaxInfo()
        {
            var result = new IFlyDogResult<IFlyDogResultType, ShareCategoryInfo>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<ShareCategoryInfo>("SELECT max([Level]) as MaxLevle,max([Number]) as MaxNumber FROM SmartShareCategory").FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 添加分享家信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(ShareCategoryAdd dto)
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

            if (dto.Icon.IsNullOrEmpty())
            {
                result.Message = "请上传图标！";
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

            var max = GetMemberMaxInfo().Data;
            if (dto.Level <= Convert.ToInt32(max.MaxLevle))
            {
                result.Message = "级别不能小于最高级别" + Convert.ToInt32(max.MaxLevle) + "！";
                return result;
            }
            if (dto.Number <= Convert.ToDecimal(max.MaxNumber))
            {
                result.Message = "升级到达分享数量不能小于当前最高分享数量" + Convert.ToDecimal(max.MaxNumber) + "元！";
                return result;
            }

            TryTransaction(() =>
            {

                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();

                result.Data = _connection.Execute("insert into SmartShareCategory([ID],[Name],[Remark],[Level],Number,[Icon]) values (@ID,@Name,@Remark,@Level,@Number,@Icon)",
                    new { ID = id, Name = dto.Name, Remark = dto.Remark, Level = dto.Level, Number = dto.Number, Icon = dto.Icon }, _transaction);

                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    CreateUserID=dto.CreateUserID,
                    Type = LogType.ShareCategoryAdd,
                    Remark = LogType.ShareCategoryAdd.ToDescription() + dto.ToJsonString()
                });

                CacheDelete.CategoryChange(SelectType.ShareCategory);

                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 根据分享家id查询该分享等级是否被引用
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        private IFlyDogResult<IFlyDogResultType, int> GetByShareCategoryIDData(string shareCategoryID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();

            #region 开始根据id查询
            TryExecute(() =>
            {
                result.Data = _connection.Query<int>("SELECT COUNT(ID) AS Count FROM dbo.SmartCustomer WHERE ShareMemberCategoryID=@ShareMemberCategoryID", new { ShareMemberCategoryID = shareCategoryID }, _transaction).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 删除分享家
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Delete(ShareCategoryDelete dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.ID == "1")
            {
                result.Message = "该分享家等级不允许删除！";
                return result;
            }

            var custormerData = GetByShareCategoryIDData(dto.ID);
            if (custormerData.Data > 0)
            {
                result.Message = "该分享家等级已被使用不能删除！";
                return result;
            }

            TryTransaction(() =>
            {
                result.Data = _connection.Execute("DELETE SmartShareCategory WHERE ID=@ID", new { ID = dto.ID }, _transaction);

                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID=dto.CreateUserID,
                    Type = LogType.ShareCategoryDelete,
                    Remark = LogType.ShareCategoryDelete + "ID：" + dto.ID
                });

                CacheDelete.CategoryChange(SelectType.ShareCategory);

                result.Message = LogType.ShareCategoryDelete.ToDescription() + "成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 获取全部分享家等级信息
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<ShareCategoryInfo>> Get()
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<ShareCategoryInfo>>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<ShareCategoryInfo>("SELECT ID,Name,Remark,Level,Number,Icon FROM dbo.SmartShareCategory ORDER BY Level");
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 根据id查询分享家详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, ShareCategoryInfo> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, ShareCategoryInfo>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<ShareCategoryInfo>("SELECT ID,Name,Remark,Level,Number,Icon FROM dbo.SmartShareCategory WHERE ID=@ID", new { ID = id }).FirstOrDefault();

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 查询当前分享家表中的最大值信息(不包含当前id的) 【这个方法先加上，后期应该可以优化】
        /// </summary>
        /// <returns></returns>
        private IFlyDogResult<IFlyDogResultType, ShareCategoryInfo> GetMemberMaxInfoNotIn(string id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, ShareCategoryInfo>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<ShareCategoryInfo>("SELECT max([Level]) as MaxLevle,max([Number]) as MaxNumber FROM SmartShareCategory WHERE ID NOT IN (@ID)", new { ID = id }).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 更新分享家信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(ShareCategoryUpdate dto)
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

            if (dto.Icon.IsNullOrEmpty())
            {
                result.Message = "请上传图标！";
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

            //var max = GetMemberMaxInfoNotIn(dto.ID).Data;
            //if (!string.IsNullOrWhiteSpace(max.MaxLevle))
            //{
            //    if (dto.Level <= Convert.ToInt32(max.MaxLevle))
            //    {
            //        result.Message = "级别不能小于最高级别" + Convert.ToInt32(max.MaxLevle) + "！";
            //        return result;
            //    }
            //}

            //if (!string.IsNullOrWhiteSpace(max.MaxNumber)) {
            //    if (dto.Number <= Convert.ToDecimal(max.MaxNumber))
            //    {
            //        result.Message = "升级到达分享数量不能小于当前最高分享数量" + Convert.ToDecimal(max.MaxNumber) + "元！";
            //        return result;
            //    }
            //} 显著是
           

            TryTransaction(() =>
            {

                string sql = "update SmartShareCategory set Name=@Name,Remark=@Remark,Level=@Level,Number=@Number,Icon=@Icon where ID = @ID";

                if (dto.ID == "1")//等于1的时候说明是要更新系统设置的分享家等级，只能更新名称和图标，其他不能更新
                {
                    sql = "update SmartShareCategory set Name=@Name,Icon=@Icon where ID = @ID";
                }

                result.Data = _connection.Execute(sql, dto, _transaction);


                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID=dto.CreateUserID,
                    Type = LogType.ShareCategoryUpdate,
                    Remark = LogType.ShareCategoryUpdate.ToDescription() + dto.ToJsonString()
                });

                CacheDelete.CategoryChange(SelectType.ShareCategory);

                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect()
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<Select>>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            var temp = _redis.StringGet<IEnumerable<Select>>(RedisPreKey.Category + SelectType.ShareCategory);
            if (temp != null)
            {
                result.Data = temp;
                return result;
            }

            TryExecute(() =>
            {
                result.Data = _connection.Query<Select>("SELECT [ID],[Name] FROM [SmartShareCategory] order by Level");

                _redis.StringSet(RedisPreKey.Category + SelectType.ShareCategory, result.Data);
            });

            return result;
        }
    }
}
