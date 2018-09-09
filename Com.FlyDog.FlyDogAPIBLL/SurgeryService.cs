using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.Common.DataAccess;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.FlyDogAPIBLL
{
    public class SurgeryService : BaseService, ISurgeryService
    {
        /// <summary>
        /// 添加预约
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Add(SurgeryAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;


            if (dto.ChargeIDS.Count() == 0)
            {
                result.Message = "请选择预约项目";
                return result;
            }
            if (dto.UserID == 0)
            {
                result.Message = "请选择医生";
                return result;
            }
            if (!string.IsNullOrWhiteSpace(dto.Remark)&&dto.Remark.Length>50)
            {
                result.Message = "描述备注不能超过50个字符";
                return result;
            }

            dto.AppointmentDate = dto.AppointmentDate.Date;

            await TryTransactionAsync(async () =>
            {
                var temp = await CanSurgeryTransactionAsync(dto.AppointmentDate, dto.AppointmentStartTime, dto.AppointmentEndTime, dto.UserID);
                if (temp.ResultType != IFlyDogResultType.Success)
                {
                    result.Message = temp.Message;
                    return false;
                }

                if (!await HasCustomerOAuthTransactionAsync(dto.CreateUserID, dto.CustomerID))
                {
                    result.Message = "对不起，您无权操作该用户！";
                    result.ResultType = IFlyDogResultType.NoAuth;
                    return false;
                }

                var id = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId();

                Task task1 = _connection.ExecuteAsync(
                    @"insert into [SmartSurgery]([ID],[CustomerID],[CreateUserID],[CreateTime],[AppointmentDate],[AppointmentStartTime],[AppointmentEndTime],[UserID],[AnesthesiaType],[Status],[Remark],[HospitalID]) 
                    values(@ID,@CustomerID,@CreateUserID,@CreateTime,@AppointmentDate,@AppointmentStartTime,@AppointmentEndTime,@UserID,@AnesthesiaType,@Status,@Remark,@HospitalID)",
                    new
                    {
                        ID = id,
                        CustomerID = dto.CustomerID,
                        CreateUserID = dto.CreateUserID,
                        CreateTime = DateTime.Now.Date,
                        AppointmentDate = dto.AppointmentDate,
                        AppointmentStartTime = dto.AppointmentStartTime,
                        AppointmentEndTime = dto.AppointmentEndTime,
                        UserID = dto.UserID,
                        AnesthesiaType = dto.AnesthesiaType,
                        Remark = dto.Remark,
                        Status = SurgeryStatus.NoDone,
                        HospitalID = dto.HospitalID
                    }, _transaction);

                Task task2 = _connection.ExecuteAsync(@"insert into [SmartSurgeryDetail]([ID],[SurgeryID],[ChargeID]) values(@ID,@SurgeryID,@ChargeID)",
                      dto.ChargeIDS.Select(u => new
                      {
                          ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                          SurgeryID = id,
                          ChargeID = u
                      }), _transaction, 30, CommandType.Text);

                await Task.WhenAll(task1, task2);

                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dto">删除信息</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Delete(SurgeryDelete dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            if (dto.ID == 0 || dto.CustomerID == 0)
            {
                result.Message = "请选择预约记录或者顾客";
                result.ResultType = IFlyDogResultType.Failed;
                return result;
            }

            await TryTransactionAsync(async () =>
            {
                if (!await HasCustomerOAuthTransactionAsync(dto.CreateUserID, dto.CustomerID))
                {
                    result.Message = "对不起，您无权操作该用户！";
                    result.ResultType = IFlyDogResultType.NoAuth;
                    return false;
                }


                var status = (await _connection.QueryAsync<SurgeryStatus>("select Status from SmartSurgery where [ID]=@ID ", dto, _transaction)).FirstOrDefault();
                if (status == SurgeryStatus.Done)
                {
                    result.Message = "对不起，已完成手术不允许修改！";
                    return false;
                }
                else if (status == SurgeryStatus.Doing)
                {
                    result.Message = "对不起，正在进行中的手术不允许修改！";
                    return false;
                }

                Task task1 = _connection.ExecuteAsync(@"delete from [SmartSurgery] where [ID]=@ID ",
                    new { ID = dto.ID }, _transaction);
                Task task2 = _connection.ExecuteAsync(@"delete from [SmartSurgeryDetail] where [SurgeryID]=@ID ",
                    new { ID = dto.ID }, _transaction);

                await Task.WhenAll(task1, task2);

                result.Message = "删除成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="dto">修改信息</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Update(SurgeryUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.ChargeIDS.Count() == 0)
            {
                result.Message = "请选择预约项目";
                return result;
            }
            if (dto.UserID == 0)
            {
                result.Message = "请选择医生";
                return result;
            }

            dto.AppointmentDate = dto.AppointmentDate.Date;

            await TryTransactionAsync(async () =>
            {
                var temp = await CanSurgeryTransactionAsync(dto.AppointmentDate, dto.AppointmentStartTime, dto.AppointmentEndTime, dto.UserID, dto.ID);
                if (temp.ResultType != IFlyDogResultType.Success)
                {
                    result.Message = temp.Message;
                    return false;
                }

                if (!await HasCustomerOAuthTransactionAsync(dto.CreateUserID, dto.CustomerID))
                {
                    result.Message = "对不起，您无权操作该用户！";
                    result.ResultType = IFlyDogResultType.NoAuth;
                    return false;
                }

                var status = (await _connection.QueryAsync<SurgeryStatus>("select Status from SmartSurgery where [ID]=@ID", dto, _transaction)).FirstOrDefault();
                if (status == SurgeryStatus.Done)
                {
                    result.Message = "对不起，已完成手术不允许修改！";
                    return false;
                }
                else if (status == SurgeryStatus.Doing)
                {
                    result.Message = "对不起，正在进行中的手术不允许修改！";
                    return false;
                }

                Task task1 = _connection.ExecuteAsync(
                    @"update [SmartSurgery] set AppointmentDate=@AppointmentDate,AppointmentStartTime=@AppointmentStartTime,AppointmentEndTime=@AppointmentEndTime,UserID=@UserID,
                    AnesthesiaType=@AnesthesiaType,Remark=@Remark where ID=@ID",
                    dto, _transaction);

                await _connection.ExecuteAsync(@"delete from [SmartSurgeryDetail] where [SurgeryID]=@ID ",
                    new { ID = dto.ID }, _transaction);

                Task task3 = _connection.ExecuteAsync(@"insert into [SmartSurgeryDetail]([ID],[SurgeryID],[ChargeID]) values(@ID,@SurgeryID,@ChargeID)",
                     dto.ChargeIDS.Select(u => new
                     {
                         ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                         SurgeryID = dto.ID,
                         ChargeID = u
                     }), _transaction, 30, CommandType.Text);

                await Task.WhenAll(task1, task3);

                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 获取预约详细信息
        /// </summary>
        /// <param name="ID">预约记录ID</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, SurgeryDetail>> GetDetail(long ID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, SurgeryDetail>();

            await TryExecuteAsync(async () =>
            {
                var list = new Dictionary<string, SurgeryDetail>();
                var temp = (await _connection.QueryAsync<SurgeryDetail, ChargeTemp, SurgeryDetail>(
                    @"select a.ID,a.CustomerID,a.HospitalID,a.CreateTime,a.AppointmentDate,a.AppointmentStartTime,a.AppointmentEndTime,a.Remark,'【'+d.Name+'】【'+b.Name+'】' as CreateUserName,
                    a.AnesthesiaType,a.UserID,f.Name as UserName,c.Name as HospitalName,g.ChargeID,e.Name as ChargeName 
                    from SmartSurgery a
					inner join SmartSurgeryDetail g on a.ID=g.SurgeryID
                    inner join SmartUser b on a.CreateUserID=b.ID
					inner join SmartCharge e on g.ChargeID=e.ID
					inner join SmartUser f on a.UserID=f.ID
                    inner join SmartHospital c on a.HospitalID=c.ID
                    inner join SmartHospital d on b.HospitalID=d.ID
					where a.ID=@ID",
                    (surgery, charge) =>
                    {
                        SurgeryDetail detail = new SurgeryDetail();
                        if (!list.TryGetValue(surgery.ID, out detail))
                        {
                            list.Add(surgery.ID, detail = surgery);
                        }
                        if (charge != null)
                            detail.Charges.Add(charge);
                        return surgery;
                    }, new { ID = ID }, null, true, splitOn: "ChargeID")).FirstOrDefault();

                result.Data = list.Values.FirstOrDefault(); ;

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 手术排台
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<Surgery>>> Get(long hospitalID, DateTime date)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<Surgery>>();

            await TryExecuteAsync(async () =>
            {
                var list = new Dictionary<string, Surgery>();
                var temp = await _connection.QueryAsync<Surgery, ChargeTemp, Surgery>(
                    @"select a.ID,a.CustomerID,c.Name as CustomerName,a.CreateTime,a.AppointmentStartTime,a.AppointmentEndTime,a.Remark,'【'+d.Name+'】【'+b.Name+'】' as CreateUserName,
                    a.AnesthesiaType,f.Name as UserName,a.StartTime,a.EndTime,a.Status,g.ChargeID,e.Name as ChargeName
                    from SmartSurgery a
					inner join SmartSurgeryDetail g on a.ID=g.SurgeryID
                    inner join SmartUser b on a.CreateUserID=b.ID
					inner join SmartCharge e on g.ChargeID=e.ID
					inner join SmartUser f on a.UserID=f.ID
                    inner join SmartHospital d on b.HospitalID=d.ID
                    inner join SmartCustomer c on a.CustomerID=c.ID
					where a.HospitalID=@HospitalID and a.AppointmentDate=@AppointmentDate order by a.AppointmentStartTime desc",
                    (surgery, charge) =>
                    {
                        Surgery detail = new Surgery();
                        if (!list.TryGetValue(surgery.ID, out detail))
                        {
                            list.Add(surgery.ID, detail = surgery);
                        }
                        if (charge != null)
                            detail.Charges.Add(charge);
                        return surgery;
                    }, new { HospitalID = hospitalID, AppointmentDate = date.Date }, null, true, splitOn: "ChargeID");

                result.Data = list.Values; ;

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 开始结束手术
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Done(SugeryDone dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.Status != 1 && dto.Status != 2)
            {
                result.Message = "状态错误！";
                return result;
            }

            await TryExecuteAsync(async () =>
            {
                SurgeryStatus status = (await _connection.QueryAsync<SurgeryStatus>("select Status from [SmartSurgery] where ID=@ID", new { ID = dto.ID })).FirstOrDefault();
                if (dto.Status == 1)
                {
                    if (status == SurgeryStatus.Doing)
                    {
                        result.Message = "手术正在进行中，无法重新开始！";
                        return;
                    }
                    else if (status == SurgeryStatus.Done)
                    {
                        result.Message = "手术已完成，无法重新开始！";
                        return;
                    }
                }
                else
                {
                    if (status == SurgeryStatus.NoDone)
                    {
                        result.Message = "手术还没开始，请先开始手术！";
                        return;
                    }
                    else if (status == SurgeryStatus.Done)
                    {
                        result.Message = "手术已完成，无法重新结束！";
                        return;
                    }
                }


                string sql_where = ",[StartTime]=@Time";
                result.Message = "手术开始成功！";
                if (dto.Status == 2)
                {
                    sql_where = ",[EndTime]=@Time";
                    result.Message = "手术结束成功！";
                }

                result.ResultType = IFlyDogResultType.Success;
                result.Data = await _connection.ExecuteAsync(
                    string.Format(@"update [SmartSurgery] set Status=@Status{0} where ID=@ID", sql_where), dto);
            });

            return result;
        }
    }
}
