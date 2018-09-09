using Com.FlyDog.IFlyDogAPIBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Dapper;
using Com.JinYiWei.Common.DataAccess;
using Com.JinYiWei.Common.Extensions;
using Com.IFlyDog.Common;
using Com.JinYiWei.Cache;

namespace Com.FlyDog.FlyDogAPIBLL
{
    class SmartProductCategoryService : BaseService, ISmartProductCategoryService
    {
        private RedisStackExchangeHelper _redis = new RedisStackExchangeHelper();
        /// <summary>
        /// 添加药物品类型
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(SmartProductCategoryAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;
            #region 数据验证
            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "药物品类型名称不能为空！";
                return result;
            }
            else if (!dto.Name.IsNullOrEmpty() && dto.Name.Length >= 20)
            {
                result.Message = "药物品类型名称最多20个字符！";
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

            #endregion

            #region 开启事物操作
            TryTransaction(() =>
            {
                #region 开始数据操作动作
                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(); //生成id
                result.Data = _connection.Execute("insert into SmartProductCategory(ID,Name,PID,SortNo,Remark) values (@ID,@Name,@PID,@SortNo,@Remark)",
                    new { ID = id, Name = dto.Name, PID = dto.PID, SortNo = dto.SortNo, Remark = dto.Remark }, _transaction);

                var temp = new { 编号 = result.Data, 名称 = dto.Name, 排序 = dto.SortNo, 备注 = dto.Remark };
                #endregion

                #region 记录日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.SmartProductCategoryAdd,
                    Remark = LogType.SmartProductCategoryAdd.ToDescription() + temp.ToJsonString()
                });
                #endregion

                CacheDelete.CategoryChange(SelectType.ProductCategory);

                result.Message = "药物品添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据分类查询商品中是否有使用
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        private IFlyDogResult<IFlyDogResultType, int> GetByCategoryIDData(string categoryID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();

            #region 开始根据id查询单位信息动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<int>("SELECT COUNT(ID) FROM dbo.SmartProduct WHERE CategoryID=@CategoryID", new { CategoryID = categoryID }, _transaction).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据分类id查询是否有子分类
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        private IFlyDogResult<IFlyDogResultType, int> GetByPIDData(string categoryID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();

            #region 开始根据id查询单位信息动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<int>("SELECT COUNT(ID) FROM dbo.SmartProductCategory WHERE PID=@PID", new { PID = categoryID }, _transaction).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 删除药物品类型
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Delete(SmartProductCategoryDelete dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;
            var productNum = GetByCategoryIDData(dto.ID);
            var categoryNum = GetByPIDData(dto.ID);



            if (productNum.Data > 0)
            {//说明此分类下已经有子分类了，不能删除
                result.Message = "此分类已经有子分类，不能删除！";
                return result;
            }

            if (productNum.Data > 0)
            {
                result.Message = "此分类已被使用，不能删除！";
                return result;

            }
            #region 开始事物操作
            TryTransaction(() =>
            {
                #region 开始更新操作
                result.Data = _connection.Execute("delete SmartProductCategory where ID=@ID", dto, _transaction);

                var temp = new { 编号 = dto.ID };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.SmartProductCategoryDelete,
                    Remark = LogType.SmartProductCategoryDelete.ToDescription() + temp.ToJsonString()
                });
                #endregion

                CacheDelete.CategoryChange(SelectType.ProductCategory);

                result.Message = "药物品删除成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 获取全部药物品信息
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<SmartProductCategoryInfo>> Get()
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<SmartProductCategoryInfo>>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            var temp = _redis.StringGet<IEnumerable<SmartProductCategoryInfo>>(RedisPreKey.Category + SelectType.ProductCategory);
            if (temp != null)
            {
                result.Data = temp;
                return result;
            }

            #region 开始查询数据动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<SmartProductCategoryInfo>(
                    @"with tree as
                    (
                    select [ID],[Name],[PID],[Remark],SortNo,Cast(RANK() OVER(order by SortNo,Name) as nvarchar(4000)) Code,cast('' as varchar) as prex from SmartProductCategory WHERE PID=0
                    union all
                    select a.[ID],a.Name,a.[PID],a.[Remark],a.SortNo,b.Code +Cast(RANK() OVER(order by a.SortNo,a.Name) as nvarchar(4000)),cast(b.prex+'··' as varchar)
                    from SmartProductCategory a,tree b where a.PID=b.ID
                    )
                    select ID,prex+Name as Name,PID,Remark,SortNo from tree order by Code OPTION(MAXRECURSION 0)");

                _redis.StringSet(RedisPreKey.Category + SelectType.ProductCategory, result.Data);
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据id查询单个药物品详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, SmartProductCategoryInfo> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, SmartProductCategoryInfo>();

            #region 开始根据id查询单位信息动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<SmartProductCategoryInfo>("SELECT [ID],[Name],[SortNo],[PID],[Remark] FROM [SmartProductCategory] where ID=@ID", new { ID = id }).FirstOrDefault();
                result.Message = "药物品查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 更新药物品信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(SmartProductCategoryUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            #region 数据验证
            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "药物品类型名称不能为空！";
                return result;
            }
            else if (!dto.Name.IsNullOrEmpty() && dto.Name.Length >= 20)
            {
                result.Message = "药物品类型名称最多20个字！";
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
            #endregion

            TryTransaction(() =>
            {
                #region 开始更新操作
                result.Data = _connection.Execute(@"update SmartProductCategory set Name = @Name,PID=@PID,SortNo=@SortNo,Remark=@Remark where ID = @ID", dto, _transaction);

                var temp = new { 编号 = dto.ID, 名称 = dto.Name, 排序 = dto.SortNo, 备注 = dto.Remark };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.SmartProductCategoryUpdate,
                    Remark = LogType.SmartProductCategoryUpdate.ToDescription() + temp.ToJsonString()
                });
                #endregion

                CacheDelete.CategoryChange(SelectType.ProductCategory);

                result.Message = "药物品修改成功";
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

            var categorys = this.Get();
            if (categorys.ResultType != IFlyDogResultType.Success)
            {
                result.Message = "查询失败";
                result.ResultType = IFlyDogResultType.Failed;
                return result;
            }

            result.Data = categorys.Data.Select(u => new Select() { ID = u.ID, Name = u.Name });

            return result;
        }
    }
}
