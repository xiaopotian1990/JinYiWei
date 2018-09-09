using Com.FlyDog.IFlyDogAPIBLL;
using System;
using System.Collections.Generic;
using System.Linq;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.Common.Extensions;
using Com.JinYiWei.Common.DataAccess;
using Dapper;
using Com.IFlyDog.Common;
using Com.JinYiWei.Cache;

namespace Com.FlyDog.FlyDogAPIBLL
{
    public class PositionService : BaseService, IPositionService
    {
        private RedisStackExchangeHelper _redis = new RedisStackExchangeHelper();
        /// <summary>
        /// 添加岗位分工
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(PositionAdd dto)
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
                result.Data = _connection.Execute("insert into SmartPosition(ID,Name,Remark,Status) values (@ID,@Name,@Remark,@Status)",
                    new { ID = id, Name = dto.Name, Remark = dto.Remark, Status = dto.Status }, _transaction);

                var temp = new { 编号 = result.Data, 名称 = dto.Name };
                #endregion

                #region 记录日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.PositionAdd,
                    Remark = LogType.PositionAdd.ToDescription() + temp.ToJsonString()
                });
                #endregion

                CacheDelete.CategoryChange(SelectType.Position);

                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 获取全部岗位分工信息
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<PositionInfo>> Get()
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<PositionInfo>>();

            #region 开始查询数据动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<PositionInfo>("SELECT [ID],[Name],[Remark],[Status] FROM SmartPosition ORDER BY Status DESC");
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 根据id获取详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, PositionInfo> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, PositionInfo>();

            #region 开始根据id查询单位信息动作
            TryExecute(() =>
            {
                result.Data = _connection.Query<PositionInfo>("SELECT [ID],[Name],[Remark],[Status] FROM SmartPosition where ID=@ID", new { ID = id }).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            #endregion
            return result;
        }

        /// <summary>
        /// 更新岗位分工
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(PositionUpdate dto)
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
                result.Data = _connection.Execute("update SmartPosition set Name = @Name,Remark=@Remark,Status=@Status where ID = @ID", dto, _transaction);

                var temp = new { 编号 = dto.ID, 名称 = dto.Name };
                #endregion

                #region 写入日志
                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.PositionUpdate,
                    Remark = LogType.PositionUpdate.ToDescription() + temp.ToJsonString()
                });
                #endregion

                CacheDelete.CategoryChange(SelectType.Position);

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

            var temp = _redis.StringGet<IEnumerable<Select>>(RedisPreKey.Category + SelectType.Position);
            if (temp != null)
            {
                result.Data = temp;
                return result;
            }

            TryExecute(() =>
            {
                result.Data = _connection.Query<Select>("SELECT [ID],[Name] FROM [SmartPosition] where [Status]=@Status order by Name", new { Status = CommonStatus.Use });

                _redis.StringSet(RedisPreKey.Category + SelectType.Position, result.Data);
            });

            return result;
        }
    }
}
