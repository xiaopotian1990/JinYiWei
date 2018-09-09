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
    /// 报表项目组业务处理
    /// </summary>
    public class ItemGroupService : BaseService, IitemGroupService
    {
        private RedisStackExchangeHelper _redis = new RedisStackExchangeHelper();
        /// <summary>
        /// 新增报表x项目组
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(ItemGroupAdd dto)
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

            if (dto.SortNo.IsNullOrEmpty())
            {
                result.Message = "排序号不能为空！";
                return result;
            }
            #endregion

            #region 开启事物操作
            TryTransaction(() =>
            {

                #region 开始数据操作动作
                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(); //生成id
                result.Data = _connection.Execute("insert into SmartItemGroup(ID,Name,SortNo,Remark) values (@ID,@Name,@SortNo,@Remark)",
                    new { ID = id, Name = dto.Name, SortNo=dto.SortNo, Remark=dto.Remark }, _transaction);

                var temp = new { 编号 = result.Data, 名称 = dto.Name };
                #endregion

                #region 记录日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    CreateUserID=dto.CreateUserID,
                    Type = LogType.ItemGroupAdd,
                    Remark = LogType.ItemGroupAdd.ToDescription() + temp.ToJsonString()
                });
                #endregion
                CacheDelete.CategoryChange(SelectType.ItemGroup);

                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据报表项目组id查询此报表项目组是否被使用
        /// </summary>
        /// <param name="categoryID"></param>
        /// <returns></returns>
        private IFlyDogResult<IFlyDogResultType, int> GetItemGroupIDData(string itemGroupID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();

            #region 开始根据id查询单位信息动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<int>("SELECT COUNT(ID) FROM dbo.SmartItem WHERE GroupID=@GroupID", new { GroupID = itemGroupID }, _transaction).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 删除报表项目组
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Delete(ItemGroupDelete dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            var itemGroupNum = GetItemGroupIDData(dto.ID);

            if (itemGroupNum.Data > 0)
            {
                result.Message = "当前项目组已被使用，不能删除!";
                return result;
            }

            #region 开始事物操作
            TryTransaction(() =>
            {
                #region 开始更新操作
                result.Data = _connection.Execute("delete SmartItemGroup where ID=@ID", dto, _transaction);

                var temp = new { 编号 = dto.ID };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                     CreateUserID=dto.CreateUserID,
                    Type = LogType.ItemGroupDelete,
                    Remark = LogType.ItemGroupDelete.ToDescription() + temp.ToJsonString()
                });
                #endregion

                CacheDelete.CategoryChange(SelectType.ItemGroup);

                result.Message = "删除成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            #endregion
            return result;

        }

        /// <summary>
        /// 查询项目组所属项目
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<ItemInfo>> GetAllGroup()
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<ItemInfo>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<ItemInfo>("SELECT Name,GroupID FROM dbo.SmartItem");
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 查询全部报表项目组
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<ItemGroupInfo>> Get()
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<ItemGroupInfo>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<ItemGroupInfo>("SELECT ID,Name,SortNo,Remark FROM SmartItemGroup");
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion

            var groupData = GetAllGroup(); //查询项目组所包含的项目

            if (groupData != null && groupData.Data.Count() > 0)
            {
                foreach (var item in groupData.Data)
                {
                    foreach (var items in result.Data)
                    {
                        if (item.GroupID == items.ID)
                        {
                            items.ItemChargeName += item.Name + ",";
                        }
                    }

                }
            }

            return result;
        }

        /// <summary>
        /// 根据id获取报表项目组详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, ItemGroupInfo> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, ItemGroupInfo>();

            #region 开始根据id查询单位信息动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<ItemGroupInfo>("SELECT [ID],[Name],SortNo,Remark FROM SmartItemGroup  where ID=@ID", new { ID = id }).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 更新报表项目组
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(ItemGroupUpdate dto)
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

            if (dto.SortNo.IsNullOrEmpty())
            {
                result.Message = "排序号不能为空！";
                return result;
            }
            #endregion

            TryTransaction(() =>
            {
                #region 开始更新操作
                result.Data = _connection.Execute("update SmartItemGroup set Name = @Name,SortNo=@SortNo,Remark=@Remark where ID = @ID", dto, _transaction);

                var temp = new { 编号 = dto.ID, 名称 = dto.Name };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                     CreateUserID=dto.CreateUserID,
                    Type = LogType.ItemGroupUpdate,
                    Remark = LogType.ItemGroupUpdate.ToDescription() + temp.ToJsonString()
                });
                #endregion
                CacheDelete.CategoryChange(SelectType.ItemGroup);
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

            var temp = _redis.StringGet<IEnumerable<Select>>(RedisPreKey.Category + SelectType.ItemGroup);
            if (temp != null)
            {
                result.Data = temp;
                return result;
            }

            TryExecute(() =>
            {
                result.Data = _connection.Query<Select>("SELECT [ID],[Name] FROM SmartItemGroup order by Name");

                _redis.StringSet(RedisPreKey.Category + SelectType.ItemGroup, result.Data);
            });

            return result;
        }
    }
}
