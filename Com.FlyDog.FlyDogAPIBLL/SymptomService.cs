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
    public class SymptomService : BaseService, ISymptomService
    {
        private RedisStackExchangeHelper _redis = new RedisStackExchangeHelper();
        /// <summary>
        /// 添加症状
        /// </summary>
        /// <param name="dto">症状信息</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(SymptomAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "症状名称不能为空！";
                return result;
            }
            else if (dto.Name.Length > 20)
            {
                result.Message = "症状名称最多20个字！";
                return result;
            }

            if (dto.Remark.IsNullOrEmpty()) {
                dto.Remark = " ";
            }else if (!dto.Remark.IsNullOrEmpty() && dto.Remark.Length >= 50)
            {
                result.Message = "备注最多50个字！";
                return result;
            }

            TryTransaction(()=>
            {
                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();
                result.Data = _connection.Execute("insert into [SmartSymptom](ID,Name,[Status],SortNo,Remark) values (@ID,@Name,@Status,@SortNo,@Remark)",
                new { ID = id, Name = dto.Name, Status = CommonStatus.Use, SortNo = dto.SortNo, Remark = dto.Remark }, _transaction);

                var temp = new { 编号 = result.Data, 名称 = dto.Name, 排序 = dto.SortNo, 备注 = dto.Remark };

                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                     CreateUserID=dto.CreateUserID,
                    Type = LogType.SymptomAdd,
                    Remark = LogType.SymptomAdd.ToDescription() + temp.ToJsonString()
                });

                CacheDelete.CategoryChange(SelectType.Symptom);

                result.Message = "症状添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            return result;
        }

        /// <summary>
        /// 症状修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(SymptomUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "症状名称不能为空！";
                return result;
            }
            else if (dto.Name.Length > 20)
            {
                result.Message = "症状名称最多20个字！";
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
                result.Data = _connection.Execute("update [SmartSymptom] set Name = @Name, SortNo = @SortNo, Remark = @Remark where ID = @ID", dto, _transaction);

                var temp = new { 编号 = dto.ID, 名称 = dto.Name, 排序 = dto.SortNo, 备注 = dto.Remark };

                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                     CreateUserID=dto.CreateUserID,
                    Type = LogType.SymptomUpdate,
                    Remark = LogType.SymptomUpdate.ToDescription() + temp.ToJsonString()
                });

                CacheDelete.CategoryChange(SelectType.Symptom);

                result.Message = "症状修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 症状使用停用
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> StopOrUse(SymptomStopOrUse dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            TryTransaction(() =>
            {
                result.Data = _connection.Execute("update [SmartSymptom] set [Status] = @Status where ID = @SymptomID", dto, _transaction);

                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                     CreateUserID=dto.CreateUserID,
                    Type = LogType.SymptomStopOrUse,
                    Remark = dto.Status.ToDescription() + "症状ID：" + dto.SymptomID
                });

                CacheDelete.CategoryChange(SelectType.Symptom);

                result.Message = dto.Status.ToDescription() + "成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });
            return result;
        }

        /// <summary>
        /// 查询所有症状
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Symptom>> Get()
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<Symptom>>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<Symptom>("SELECT [ID],[Name],[Remark],[Status],[SortNo] FROM [SmartSymptom] order by Status desc,SortNo,Name");
                result.Message = "症状查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;

        }

        /// <summary>
        /// 检测症状用接口
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<ItemGetSymptomInfo>> ItemGetSymptom()
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<ItemGetSymptomInfo>>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<ItemGetSymptomInfo>("select a.Name AS SymptonName,a.Status,c.Name AS ItemName FROM SmartSymptom a LEFT JOIN SmartItemSymptomDetail b ON b.SymptomID = a.ID LEFT JOIN dbo.SmartItem c ON b.ItemID = c.ID  ORDER BY a.Status DESC");
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 查询所有症状
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, Symptom> GetByID(long id)
        {      
            var result = new IFlyDogResult<IFlyDogResultType, Symptom>();
            TryExecute(() =>
            {
                result.Data = _connection.Query<Symptom>("SELECT [ID],[Name],[Remark],[Status],[SortNo] FROM [SmartSymptom] where ID=@ID", new { ID = id }).FirstOrDefault();
                result.Message = "症状查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });
            return result;
        }

        /// <summary>
        /// 查询所有可用的症状信息
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Symptom>> GetStateIsOk()
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<Symptom>>();
            TryExecute(() =>
            {
                    result.Data = _connection.Query<Symptom>("SELECT [ID],[Name],[Remark],[Status],[SortNo] FROM [SmartSymptom] WHERE ID NOT IN (SELECT SymptomID FROM SmartItemSymptomDetail) AND Status=1");
                    result.Message = "症状查询成功";
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

            var temp = _redis.StringGet<IEnumerable<Select>>(RedisPreKey.Category + SelectType.Symptom);
            if (temp != null)
            {
                result.Data = temp;
                return result;
            }

            TryExecute(() =>
            {
                result.Data = _connection.Query<Select>("SELECT [ID],[Name] FROM [SmartSymptom] where [Status]=@Status order by SortNo,Name", new { Status = CommonStatus.Use });

                _redis.StringSet(RedisPreKey.Category + SelectType.Symptom, result.Data);
            });

            return result;
        }
    }
}
