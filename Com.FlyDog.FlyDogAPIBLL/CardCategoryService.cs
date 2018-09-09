using Com.IFlyDog.CommonDTO;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using Com.JinYiWei.Common.DataAccess;
using Com.JinYiWei.Common.Extensions;
using Com.IFlyDog.APIDTO;
using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.Common;
using Com.JinYiWei.Cache;

namespace Com.FlyDog.FlyDogAPIBLL
{
    /// <summary>
    /// 银行卡相关服务
    /// </summary>
    public class CardCategoryService : BaseService, ICardCategoryService
    {
        private RedisStackExchangeHelper _redis = new RedisStackExchangeHelper();
        /// <summary>
        /// 添加银行卡
        /// </summary>
        /// <param name="dto">银行卡信息</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(CardCategoryAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "名称不能为空！";
                return result;
            }
            else if (!string.IsNullOrWhiteSpace(dto.Name) && dto.Name.Length > 20)
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
                result.Message = "备注最多50个字！";
                return result;
            }


            TryTransaction(() =>
            {
                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();

                result.Data = _connection.Execute("insert into [SmartCardCategory]([ID],[Name],[Remark],[Status]) values (@ID,@Name,@Remark,@Status)",
                    new { ID = id, Name = dto.Name, Status = CommonStatus.Use, Remark = dto.Remark }, _transaction);

                var temp = new { 编号 = id, 名称 = dto.Name, 备注 = dto.Remark };

                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.CardCategoryAdd,
                    Remark = LogType.CardCategoryAdd.ToDescription() + temp.ToJsonString()
                });

                CacheDelete.CategoryChange(SelectType.CardCategory);

                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 更新银行卡信息
        /// </summary>
        /// <param name="dto">银行卡信息</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(CardCategoryUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "名称不能为空！";
                return result;
            }
            else if (!string.IsNullOrWhiteSpace(dto.Name) && dto.Name.Length > 20)
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
                result.Message = "备注最多50个字！";
                return result;
            }

            TryTransaction(() =>
            {
                result.Data = _connection.Execute("update [SmartCardCategory] set Name = @Name,  Remark = @Remark where ID = @ID", dto, _transaction);

                var temp = new { 编号 = dto.ID, 名称 = dto.Name, 备注 = dto.Remark };

                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.CardCategoryUpdate,
                    Remark = LogType.CardCategoryUpdate.ToDescription() + temp.ToJsonString()
                });

                CacheDelete.CategoryChange(SelectType.CardCategory);

                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 银行卡使用停用
        /// </summary>
        /// <param name="dto">参数集</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> StopOrUse(CardCategoryStopOrUse dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            TryTransaction(() =>
            {
                result.Data = _connection.Execute("update [SmartCardCategory] set [Status] = @Status where ID = @ID", dto, _transaction);

                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.CardCategoryStopOrUse,
                    Remark = dto.Status.ToDescription() + "ID：" + dto.ID
                });

                CacheDelete.CategoryChange(SelectType.CardCategory);

                result.Message = dto.Status.ToDescription() + "成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 查询所有银行卡
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<CardCategory>> Get()
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<CardCategory>>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<CardCategory>("SELECT [ID],[Name],[Remark],[Status] FROM [SmartCardCategory] order by Status desc,Name");
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 查询银行卡详细
        /// </summary>
        /// <param name="id">银行卡ID</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, CardCategory> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, CardCategory>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<CardCategory>("SELECT [ID],[Name],[Remark],[Status] FROM [SmartCardCategory] where ID=@ID", new { ID = id }).FirstOrDefault();
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

            var temp = _redis.StringGet<IEnumerable<Select>>(RedisPreKey.Category + SelectType.CardCategory);
            if (temp != null)
            {
                result.Data = temp;
                return result;
            }

            TryExecute(() =>
            {
                result.Data = _connection.Query<Select>("SELECT [ID],[Name] FROM [SmartCardCategory] where [Status]=@Status order by Name", new { Status = CommonStatus.Use });

                _redis.StringSet(RedisPreKey.Category + SelectType.CardCategory, result.Data);
            });

            return result;
        }
    }
}
