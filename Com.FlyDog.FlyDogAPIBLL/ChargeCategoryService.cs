using Com.FlyDog.IFlyDogAPIBLL;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// 项目分类业务逻辑
    /// </summary>
    public class ChargeCategoryService : BaseService, IChargeCategoryService
    {
        private RedisStackExchangeHelper _redis = new RedisStackExchangeHelper();

        /// <summary>
        /// 添加项目分类
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(ChargeCategoryAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;
            #region 数据验证
            if (dto.Name.IsNullOrEmpty())
            {               
                result.Message = "名称不能为空！";
                return result;
            }
            else if (!dto.Name.IsNullOrEmpty() && dto.Name.Length >= 20)
            {
                result.Message = "名称最多20个字符！";
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
                result.Data = _connection.Execute("insert into SmartChargeCategory(ID,Name,ParentID,SortNo,Remark) values (@ID,@Name,@ParentID,@SortNo,@Remark)",
                    new { ID = id, Name = dto.Name, ParentID = dto.ParentID, SortNo = dto.SortNo, Remark = dto.Remark }, _transaction);

                var temp = new { 编号 = result.Data, 名称 = dto.Name, 排序 = dto.SortNo, 备注 = dto.Remark };
                #endregion

                #region 记录日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.ChargeCategoryAdd,
                    Remark = LogType.ChargeCategoryAdd.ToDescription() + temp.ToJsonString()
                });
                #endregion

                CacheDelete.CategoryChange(SelectType.ChargeCategory);

                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据项目分类id，查询此项目分类id是否被使用，
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        private IFlyDogResult<IFlyDogResultType, int> GetByCategoryIDData(string categoryID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();

            #region 开始根据id查询单位信息动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<int>("SELECT COUNT(ID) FROM dbo.SmartCharge WHERE CategoryID=@CategoryID", new { CategoryID = categoryID }, _transaction).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据项目分类id查询分类，如果有子分类了则不能删除
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        private IFlyDogResult<IFlyDogResultType, int> GetByPIDData(string categoryID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();

            #region 开始根据id查询单位信息动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<int>("SELECT COUNT(ID) FROM dbo.SmartChargeCategory WHERE ParentID=@ParentID", new { ParentID = categoryID }, _transaction).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 删除项目分类，如果这个项目分类已经被使用了，则不能删除
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Delete(ChargeCategoryDelete dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            var categoryNum = GetByCategoryIDData(dto.ID);
            var categoryPidNum = GetByPIDData(dto.ID);
            if (categoryNum.Data > 0)
            {
                result.Message = "此分类已被使用，不能删除！";
                return result;
            }

            if (categoryPidNum.Data>0) {
                result.Message = "此分类已有子分类，不能删除！";
                return result;
            }

            #region 开始事物操作
            TryTransaction(() =>
            {
                #region 开始更新操作
                result.Data = _connection.Execute("delete SmartChargeCategory where ID=@ID", dto, _transaction);

                var temp = new { 编号 = dto.ID };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.ChargeCategoryDelete,
                    Remark = LogType.ChargeCategoryDelete.ToDescription() + temp.ToJsonString()
                });
                #endregion

                CacheDelete.CategoryChange(SelectType.ChargeCategory);

                result.Message = "删除成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 查询全部项目分类
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<ChargeCategoryInfo>> Get()
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<ChargeCategoryInfo>>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            var temp = _redis.StringGet<IEnumerable<ChargeCategoryInfo>>(RedisPreKey.Category + SelectType.ChargeCategory);
            if (temp != null)
            {
                result.Data = temp;
                return result;
            }

            #region 开始查询数据动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<ChargeCategoryInfo>(
                    @"with tree as
                      (
                      select [ID],[Name],[ParentID],[Remark],SortNo,Cast(RANK() OVER(order by SortNo,Name) as nvarchar(4000)) Code,cast('' as varchar) as prex from SmartChargeCategory WHERE ParentID=0
                      union all
                      select a.[ID],a.Name,a.[ParentID],a.[Remark],a.SortNo,b.Code +Cast(RANK() OVER(order by a.SortNo,a.Name) as nvarchar(4000)),cast(b.prex+'··' as varchar)
                      from SmartChargeCategory a,tree b where a.ParentID=b.ID
                       )
                      select ID,prex+Name as Name,ParentID,Remark,SortNo from tree order by Code OPTION(MAXRECURSION 0)");

                _redis.StringSet(RedisPreKey.Category + SelectType.ChargeCategory, result.Data);
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据id查询单个分类详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, ChargeCategoryInfo> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, ChargeCategoryInfo>();

            #region 开始根据id查询单位信息动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<ChargeCategoryInfo>("SELECT [ID],[Name],[SortNo],ParentID,[Remark] FROM SmartChargeCategory where ID=@ID", new { ID = id }).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 更新项目分类
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(ChargeCategoryUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            #region 数据验证
            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "名称不能为空！";
                return result;
            }
            else if (!dto.Name.IsNullOrEmpty() && dto.Name.Length >= 20)
            {
                result.Message = "名称最多20个字符！";
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
                result.Data = _connection.Execute(@"update SmartChargeCategory set Name = @Name,ParentID=@ParentID,SortNo=@SortNo,Remark=@Remark where ID = @ID", dto, _transaction);

                var temp = new { 编号 = dto.ID, 名称 = dto.Name, 排序 = dto.SortNo, 备注 = dto.Remark };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.ChargeCategoryUpdate,
                    Remark = LogType.ChargeCategoryUpdate.ToDescription() + temp.ToJsonString()
                });
                #endregion

                CacheDelete.CategoryChange(SelectType.ChargeCategory);

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
