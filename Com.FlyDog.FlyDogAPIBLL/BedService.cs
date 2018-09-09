using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.Common.DataAccess;
using Com.JinYiWei.Common.Extensions;
using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Com.FlyDog.FlyDogAPIBLL
{
    public class BedService : BaseService, IBedService
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Add(BedAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "床位名称不能为空！";
                return result;
            }
            else if (dto.Name.Length > 20)
            {
                result.Message = "床位名称最多20个字！";
                return result;
            }

            if (!dto.Remark.IsNullOrEmpty() && dto.Remark.Length >= 50)
            {
                result.Message = "备注最多50个字！";
                return result;
            }


            TryTransaction(() =>
            {
                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();
                result.Data = _connection.Execute("insert into [SmartBed]([ID],[Name],[Status],[Remark],[Usage],[HospitalID]) values (@ID,@Name,@Status,@Remark,@Usage,@HospitalID)",
                    new { ID = id, Name = dto.Name, Status = CommonStatus.Use, Usage = BedStatus.Free, Remark = dto.Remark, HospitalID = dto.HospitalID }, _transaction);

                var temp = new { 编号 = result.Data, 名称 = dto.Name, 备注 = dto.Remark };

                AddOperationLog(new SmartOperationLog()
                {
                    ID = id,
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.BedAdd,
                    Remark = LogType.BedAdd.ToDescription() + temp.ToJsonString()
                });
                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 部门修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> Update(BedAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.Name.IsNullOrEmpty())
            {
                result.Message = "床位名称不能为空！";
                return result;
            }
            else if (dto.Name.Length > 20)
            {
                result.Message = "床位名称最多20个字！";
                return result;
            }

            if (!dto.Remark.IsNullOrEmpty() && dto.Remark.Length >= 50)
            {
                result.Message = "备注最多50个字！";
                return result;
            }


            TryTransaction(() =>
            {

                result.Data = _connection.Execute(
                    @"update [SmartBed] set Name = @Name, Remark = @Remark where ID = @ID", dto, _transaction);

                var temp = new { 编号 = dto.ID, 名称 = dto.Name, 备注 = dto.Remark };

                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.BedUpdate,
                    Remark = LogType.BedUpdate.ToDescription() + temp.ToJsonString()
                });

                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 查询当前医院下的所有直接部门
        /// </summary>
        /// <param name="hospitalID">所属医院ID</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Bed>> Get(long hospitalID, CommonStatus status = CommonStatus.All)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<Bed>>();
            result.Message = "部门查询成功";
            result.ResultType = IFlyDogResultType.Success;

            string sql_where = "";
            if (status != CommonStatus.All)
            {
                sql_where += " and Status=@Status ";
            }

            TryExecute(() =>
            {
                result.Data = _connection.Query<Bed>(
                    string.Format("SELECT [ID],[Name],[Status],[Remark],[Usage] FROM [SmartBed] WHERE HospitalID=@HospitalID {0} order by [Status] desc", sql_where), new { HospitalID = hospitalID, Status = status });
            });

            return result;
        }

        /// <summary>
        /// 根据ID获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, Bed> GetByID(long id)
        {
            var result = new IFlyDogResult<IFlyDogResultType, Bed>();

            TryExecute(() =>
            {
                result.Data = _connection.Query<Bed>(@"SELECT [ID],[Name],[Status],[Remark],[Usage] FROM [SmartBed] 
                                                     where ID=@ID", new { ID = id }).FirstOrDefault();
                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect(long hospitalID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<Select>>();
            result.Message = "查询成功";
            result.ResultType = IFlyDogResultType.Success;

            TryExecute(() =>
            {
                result.Data = _connection.Query<Select>("SELECT [ID],[Name] FROM [SmartBed] where HospitalID=@HospitalID and [Status]=@Status and Usage=@Usage",
                    new { HospitalID = hospitalID, Status = CommonStatus.Use, Usage = BedStatus.Free });

            });

            return result;
        }

        /// <summary>
        /// 使用停用
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> StopOrUse(BedStop dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();


            TryTransaction(() =>
            {
                var status = _connection.Query<BedStatus>(@"select Usage from SmartBed where ID = @ID", dto, _transaction).FirstOrDefault();

                if (status == BedStatus.Use && dto.Status == CommonStatus.Stop)
                {
                    result.Message = "床位正在使用中，不能停用！";
                    result.ResultType = IFlyDogResultType.Failed;
                    return false;
                }
                result.Data = _connection.Execute(
                    @"update [SmartBed] set Status = @Status where ID = @ID", dto, _transaction);

                AddOperationLog(new SmartOperationLog()
                {
                    ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                    CreateTime = DateTime.Now,
                    CreateUserID = dto.CreateUserID,
                    Type = LogType.BedStopOrUse,
                    Remark = LogType.BedStopOrUse.ToDescription() + dto.ToJsonString()
                });

                result.Message = LogType.BedStopOrUse.ToDescription() + "成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }
    }
}
