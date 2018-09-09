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
    /// 关系管理业务逻辑
    /// </summary>
    public class RelationService : BaseService, IRelationService
    {
        private RedisStackExchangeHelper _redis = new RedisStackExchangeHelper();
        /// <summary>
        /// 添加关系
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(RelationAdd dto)
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
                result.Data = _connection.Execute("insert into SmartRelation(ID,Name,Remark) values (@ID,@Name,@Remark)",
                    new { ID = id, Name = dto.Name, Remark = dto.Remark }, _transaction);

                var temp = new { 编号 = result.Data, 名称 = dto.Name };
                #endregion

                #region 记录日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    CreateUserID=dto.CreateUserID,
                    Type = LogType.RelationAdd,
                    Remark = LogType.RelationAdd.ToDescription() + temp.ToJsonString()
                });
                #endregion

                CacheDelete.CategoryChange(SelectType.Relation);

                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            #endregion
            return result;
        }


        /// <summary>
        /// 根据关系id查询此关系是否被引用，如果有引用则不能删除s
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        private IFlyDogResult<IFlyDogResultType, int> GetByRelationIDData(string relationID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();

            #region 开始根据id查询信息动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<int>("SELECT COUNT(ID) FROM dbo.SmartCustomerRelation WHERE RelationID=@RelationID", new { RelationID = relationID }, _transaction).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }


        /// <summary>
        /// 删除关系
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Delete(RelationDelete dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;
            var RelationNum = GetByRelationIDData(dto.ID);
            if (RelationNum.Data > 0)
            {
                result.Message = "当前关系已经被使用，不能删除!";
                return result;
            }

            #region 开始事物操作
            TryTransaction(() =>
            {
                #region 开始更新操作
                result.Data = _connection.Execute("DELETE SmartRelation WHERE ID=@ID", new { ID = dto.ID }, _transaction);
                var temp = new { 编号 = dto.ID };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID=dto.CreateUserID,
                    Type = LogType.RelationDelete,
                    Remark = LogType.RelationDelete.ToDescription() + temp.ToJsonString()
                });
                #endregion

                CacheDelete.CategoryChange(SelectType.Relation);

                result.Message = "删除成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 获取全部关系信息
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<RelationInfo>> Get()
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<RelationInfo>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<RelationInfo>("SELECT [ID],[Name],[Remark] FROM SmartRelation ");
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据id查询关系详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, RelationInfo> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, RelationInfo>();

            #region 开始根据id查询单位信息动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<RelationInfo>("SELECT [ID],[Name],[Remark] FROM SmartRelation where ID=@ID", new { ID = id }).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 更新关系
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(RelationUpdate dto)
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
                result.Data = _connection.Execute("update SmartRelation set Name = @Name,Remark=@Remark where ID = @ID", dto, _transaction);

                var temp = new { 编号 = dto.ID, 名称 = dto.Name };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID=dto.CreateUserID,
                    Type = LogType.RelationUpdate,
                    Remark = LogType.RelationUpdate.ToDescription() + temp.ToJsonString()
                });
                #endregion

                CacheDelete.CategoryChange(SelectType.Relation);

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

            var temp = _redis.StringGet<IEnumerable<Select>>(RedisPreKey.Category + SelectType.Relation);
            if (temp != null)
            {
                result.Data = temp;
                return result;
            }

            TryExecute(() =>
            {
                result.Data = _connection.Query<Select>("SELECT [ID],[Name] FROM [SmartRelation] order by Name");

                _redis.StringSet(RedisPreKey.Category + SelectType.Relation, result.Data);
            });

            return result;
        }
    }
}
