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
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.FlyDogAPIBLL
{
    public class ChannelService : BaseService, IChannelService
    {
        private RedisStackExchangeHelper _redis = new RedisStackExchangeHelper();
        /// <summary>
        /// 添加渠道
        /// </summary>
        /// <param name="dto">渠道信息</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(ChannelAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "渠道名称不能为空！";
                return result;
            }
            else if (dto.Name.Length > 20)
            {
                result.Message = "渠道名称最多20个字！";
                return result;
            }

            if (dto.Remark.IsNullOrEmpty())
            {
                dto.Remark = " ";
            }
            else if (!dto.Remark.IsNullOrEmpty() && dto.Remark.Length > 50)
            {
                result.Message = "备注最多50个字！";
                return result;
            }


            TryTransaction(() =>
            {

                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();

                result.Data = _connection.Execute("insert into [SmartChannel](ID,Name,[Status],SortNo,Remark) values (@ID,@Name,@Status,@SortNo,@Remark)",
                    new { ID = id, Name = dto.Name, Status = CommonStatus.Use, SortNo = dto.SortNo, Remark = dto.Remark }, _transaction);

                var temp = new { 编号 = id, 名称 = dto.Name, 排序 = dto.SortNo, 备注 = dto.Remark };

                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.ChannelAdd,
                    Remark = LogType.ChannelAdd.ToDescription() + temp.ToJsonString()
                });

                CacheDelete.CategoryChange(SelectType.Channel);

                result.Message = "渠道添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 渠道修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(ChannelUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "渠道名称不能为空！";
                return result;
            }
            else if (dto.Name.Length > 20)
            {
                result.Message = "渠道名称最多20个字！";
                return result;
            }

            if (dto.Remark.IsNullOrEmpty())
            {
                dto.Remark = " ";
            }
            else if (!dto.Remark.IsNullOrEmpty() && dto.Remark.Length > 50)
            {
                result.Message = "备注最多50个字！";
                return result;
            }

            TryTransaction(() =>
            {
                result.Data = _connection.Execute("update [SmartChannel] set Name = @Name, SortNo = @SortNo, Remark = @Remark where ID = @ID", dto, _transaction);

                var temp = new { 编号 = dto.ID, 名称 = dto.Name, 排序 = dto.SortNo, 备注 = dto.Remark };

                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.ChannelUpdate,
                    Remark = LogType.ChannelUpdate.ToDescription() + temp.ToJsonString()
                });

                CacheDelete.CategoryChange(SelectType.Channel);

                result.Message = "渠道修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 渠道使用停用
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> StopOrUse(ChannelStopOrUse dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            TryTransaction(() =>
            {
                result.Data = _connection.Execute("update [SmartChannel] set [Status] = @Status where ID = @ChannelID", dto, _transaction);

                AddOperationLog(new SmartOperationLog() { ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(), CreateTime = DateTime.Now, CreateUserID = dto.CreateUserID, Type = LogType.ChannelStopOrUse, Remark = dto.Status.ToDescription() + "渠道ID：" + dto.ChannelID });

                CacheDelete.CategoryChange(SelectType.Channel);

                result.Message = dto.Status.ToDescription() + "成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 查询所有渠道
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Channel>> Get()
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<Channel>>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<Channel>("SELECT [ID],[Name],[Remark],[Status],[SortNo] FROM [SmartChannel] order by Status desc,SortNo,Name");
                result.Message = "渠道查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 查询所有可使用的渠道
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Channel>> GetByIsOk() {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<Channel>>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<Channel>("SELECT [ID],[Name],[Remark],[Status],[SortNo] FROM [SmartChannel] WHERE Status=1 AND ID NOT IN(SELECT ChannelID FROM dbo.SmartChannelGroupDetail)");
                result.Message = "渠道查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 查询所有渠道
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, Channel> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, Channel>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<Channel>("SELECT [ID],[Name],[Remark],[Status],[SortNo] FROM [SmartChannel] where ID=@ID", new { ID = id }).FirstOrDefault();
                result.Message = "渠道查询成功";
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

            var temp = _redis.StringGet<IEnumerable<Select>>(RedisPreKey.Category + SelectType.Channel);
            if (temp != null)
            {
                result.Data = temp;
                return result;
            }

            TryExecute(() =>
            {
                result.Data = _connection.Query<Select>("SELECT [ID],[Name] FROM [SmartChannel] where [Status]=@Status order by SortNo,Name", new { Status = CommonStatus.Use });

                _redis.StringSet(RedisPreKey.Category + SelectType.Channel, result.Data);
            });

            return result;
        }
    }
}
