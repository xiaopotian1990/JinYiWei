using Com.IFlyDog.CommonDTO;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using Com.JinYiWei.Common.DataAccess;
using Com.JinYiWei.Common.Extensions;
using Com.IFlyDog.APIDTO;
using Com.FlyDog.IFlyDogAPIBLL;

namespace Com.FlyDog.FlyDogAPIBLL
{
    /// <summary>
    /// 会员权益相关服务
    /// </summary>
    public class EquityService : BaseService, IEquityService
    {
        /// <summary>
        /// 添加会员权益
        /// </summary>
        /// <param name="dto">会员权益信息</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(EquityAdd dto)
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

            if(dto.Type== EquityType.Discount)
            {
                if(dto.Discount==null || dto.Discount <= 0)
                {
                    result.Message = "折扣权益要大于0！";
                    return result;
                }
            }

            if (!dto.Remark.IsNullOrEmpty() && dto.Remark.Length >= 50)
            {
                result.Message = "备注最多50个字！";
                return result;
            }


            TryTransaction(() =>
            {
                int num = _connection.Query<int>("select count(ID) from SmartUser where ID=@CreateUserID", new { CreateUserID = dto.CreateUserID }, _transaction).FirstOrDefault();

                if (num == 0)
                {
                    result.Message = "操作人ID不存在！";
                    return false;
                }

                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();

                result.Data = _connection.Execute("insert into [SmartEquity]([ID],[Name],[Type],[Discount],[Remark],[Status]) values (@ID,@Name,@Type,@Discount,@Remark,@Status)",
                    new { ID = id, Name = dto.Name, Status = CommonStatus.Use, Type=dto.Type, Discount=dto.Discount, Remark=dto.Remark }, _transaction);

                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.EquityAdd,
                    Remark = LogType.EquityAdd.ToDescription() + dto.ToJsonString()
                });

                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 更新会员权益信息
        /// </summary>
        /// <param name="dto">会员权益信息</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(EquityUpdate dto)
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
            if (dto.Type == EquityType.Discount)
            {
                if (dto.Discount == null || dto.Discount <= 0)
                {
                    result.Message = "折扣权益要大于0！";
                    return result;
                }
            }

            if (!dto.Remark.IsNullOrEmpty() && dto.Remark.Length >= 50)
            {
                result.Message = "备注最多50个字！";
                return result;
            }

            TryTransaction(() =>
            {
                int num = _connection.Query<int>("select count(ID) from SmartUser where ID=@CreateUserID", new { CreateUserID = dto.CreateUserID }, _transaction).FirstOrDefault();

                if (num == 0)
                {
                    result.Message = "操作人ID不存在！";
                    return false; 
                }

                result.Data = _connection.Execute("update [SmartEquity] set [Name]=@Name,[Type]=@Type,[Discount]=@Discount,[Remark]=@Remark where ID = @ID", dto, _transaction);

                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.EquityUpdate,
                    Remark = LogType.EquityUpdate.ToDescription() + dto.ToJsonString()
                });

                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 会员权益停用
        /// </summary>
        /// <param name="dto">参数集</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> StopOrUse(EquityStopOrUse dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            TryTransaction(() =>
            {
                int num = _connection.Query<int>("select count(ID) from SmartUser where ID=@CreateUserID", new { CreateUserID = dto.CreateUserID }, _transaction).FirstOrDefault();

                if (num == 0)
                {
                    result.Message = "操作人ID不存在！";
                    return false;
                }

                result.Data = _connection.Execute("update [SmartEquity] set [Status] = @Status where ID = @ID", dto, _transaction);

                AddOperationLog(new SmartOperationLog()
                { ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now, CreateUserID = dto.CreateUserID,
                    Type = LogType.EquityStopOrUse,
                    Remark = dto.Status.ToDescription() + "ID：" + dto.ID
                });

                result.Message = dto.Status.ToDescription() + "成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 查询所有会员权益
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Equity>> Get()
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<Equity>>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<Equity>("SELECT [ID],[Name],[Type],[Discount],[Remark],[Status] FROM [SmartEquity] order by Status desc,Name");
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 查询会员权益详细
        /// </summary>
        /// <param name="id">会员权益ID</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, Equity> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, Equity>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<Equity>("SELECT [ID],[Name],[Type],[Discount],[Remark],[Status] FROM [SmartEquity] where ID=@ID", new { ID = id }).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 只查询可用的会员权益
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Equity>> GetStatusIsTrue()
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<Equity>>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<Equity>("SELECT [ID],[Name],[Type],[Discount],[Remark],[Status] FROM [SmartEquity]WHERE Status=1");
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }
    }
}
