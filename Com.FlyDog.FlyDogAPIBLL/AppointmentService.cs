using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Com.JinYiWei.Common.DataAccess;
using Com.FlyDog.IFlyDogAPIBLL;
using Com.JinYiWei.Common.Extensions;

namespace Com.FlyDog.FlyDogAPIBLL
{
    /// <summary>
    /// 预约服务
    /// </summary>
    public class AppointmentService : BaseService, IAppointmentService
    {
        /// <summary>
        /// 添加预约
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> AppointmentAdd(AppointmentAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.Content.IsNullOrEmpty())
            {
                result.Message = "描述信息不能为空！";
                return result;
            }
            if (!dto.Content.IsNullOrEmpty() && dto.Content.Length >= 500)
            {
                result.Message = "描述最多500个字符！";
                return result;
            }

            await TryTransactionAsync(async () =>
            {
                var temp = await CanAppointmentTransactionAsync(dto.AppointmentDate, dto.AppointmentStartTime, dto.AppointmentEndTime);
                if (temp.ResultType != IFlyDogResultType.Success)
                {
                    result.Message = temp.Message;
                    return false;
                }

                //var num = (await HasCustomerOAuthAsync(dto.CreateUserID, dto.CustomerID)).FirstOrDefault();

                if (!await HasCustomerOAuthTransactionAsync(dto.CreateUserID, dto.CustomerID))
                {
                    result.Message = "对不起，您无权操作该用户！";
                    result.ResultType = IFlyDogResultType.NoAuth;
                    return false;
                }

                await _connection.ExecuteAsync(
                    @"insert into [SmartAppointment]([ID],[CustomerID],[CreateUserID],[CreateTime],[AppointmentDate],[AppointmentStartTime] ,[AppointmentEndTime],[Content],[Status],[Code],[HospitalID]) 
                    values(@ID,@CustomerID,@CreateUserID,@CreateTime,@AppointmentDate,@AppointmentStartTime,@AppointmentEndTime,@Content,@Status,@Code,@HospitalID)",
                    new
                    {
                        ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                        CustomerID = dto.CustomerID,
                        CreateUserID = dto.CreateUserID,
                        CreateTime = DateTime.Now,
                        AppointmentDate = dto.AppointmentDate.Date,
                        AppointmentStartTime = dto.AppointmentStartTime,
                        AppointmentEndTime = dto.AppointmentEndTime,
                        Content = dto.Content,
                        Status = AppointmentStatus.No,
                        Code = KCAutoNumber.Instance().AppointmentCode(),
                        HospitalID = dto.HospitalID
                    }, _transaction);

                await UpdateCustomerAppointment(dto.CustomerID, dto.AppointmentDate, dto.HospitalID);

                result.Message = "添加预约成功";
                result.ResultType = IFlyDogResultType.Success;
                return true;
            });

            return result;
        }

        /// <summary>
        /// 获取今日新增预约
        /// </summary>
        /// <param name="hospitalID">医院ID</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<AppointmentToday>>> GetAppointmentToday(long hospitalID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<AppointmentToday>>();

            await TryExecuteAsync(async () =>
            {
                result.Data = await _connection.QueryAsync<AppointmentToday>(
                    @"SELECT a.ID,a.CustomerID,b.Name as CustomerName,a.AppointmentDate,a.AppointmentStartTime,a.AppointmentEndTime,a.Content,a.CreateTime,'咨询预约' as Type
                    FROM [SmartAppointment] a
                    left join SmartCustomer b on a.CustomerID = b.ID
                    where a.HospitalID = @HospitalID and DateDiff(dd,a.CreateTime,getdate())=0
					union all
					SELECT a.ID,a.CustomerID,b.Name as CustomerName,a.AppointmentDate,a.AppointmentStartTime,a.AppointmentEndTime,'治疗项目：'+d.Name+'<br/>'+'预约医生'+c.Name,a.CreateTime,'治疗预约' as Type
                    FROM SmartTreat a
                    left join SmartCustomer b on a.CustomerID = b.ID
                    left join SmartUser c on c.ID = a.UserID
					left join SmartCharge d on a.ChargeID=d.ID
                    where a.HospitalID = @HospitalID and DateDiff(dd,a.CreateTime,getdate())=0", new { HospitalID = hospitalID });

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 获取今日上门预约
        /// </summary>
        /// <param name="hospitalID">医院ID</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<AppointmentComeToday>>> GetAppointmentComeToday(long hospitalID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<AppointmentComeToday>>();

            await TryExecuteAsync(async () =>
            {
                result.Data = await _connection.QueryAsync<AppointmentComeToday>(
                    @"SELECT a.ID,a.CustomerID,b.Name as CustomerName,a.AppointmentStartTime,a.AppointmentEndTime,a.Content,a.CreateTime,'咨询预约' as Type,replace(b.Mobile,substring(b.Mobile,4,4),'****') as Mobile
                    FROM [SmartAppointment] a
                    left join SmartCustomer b on a.CustomerID = b.ID
                    where a.HospitalID = @HospitalID and DateDiff(dd,a.AppointmentDate,getdate())=0 and a.Status=@Status
					union all
					SELECT a.ID,a.CustomerID,b.Name as CustomerName,a.AppointmentStartTime,a.AppointmentEndTime,'治疗项目：'+d.Name+'<br/>'+'预约医生'+c.Name,a.CreateTime,'治疗预约' as Type,replace(b.Mobile,substring(b.Mobile,4,4),'****') as Mobile
                    FROM SmartTreat a
                    left join SmartCustomer b on a.CustomerID = b.ID
                    left join SmartUser c on c.ID = a.UserID
					left join SmartCharge d on a.ChargeID=d.ID
                    where a.HospitalID = @HospitalID and DateDiff(dd,a.AppointmentDate,getdate())=0 and a.Status=@Status", new { HospitalID = hospitalID, Status = AppointmentStatus.No });

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dto">删除信息</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Delete(AppointmentDelete dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            if (dto.ID == 0 || dto.CustomerID == 0)
            {
                result.Message = "请选择预约记录或者顾客";
                result.ResultType = IFlyDogResultType.Failed;
                return result;
            }

            await TryExecuteAsync(async () =>
            {
                if (!await HasCustomerOAuthAsync(dto.CreateUserID, dto.CustomerID))
                {
                    result.Message = "对不起，您无权操作该用户！";
                    result.ResultType = IFlyDogResultType.NoAuth;
                    return;
                }

                var count = await _connection.ExecuteAsync(@"delete from [SmartAppointment] where [ID]=@ID and CustomerID=@CustomerID and Status=@Status",
                    new { Status = AppointmentStatus.No, ID = dto.ID, CustomerID = dto.CustomerID });
                if (count == 0)
                {
                    result.Message = "删除失败，该预约已经完成或者预约不存在！";
                    return;
                }

                result.Data = count;
                result.Message = "删除成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="dto">修改信息</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Update(AppointmentUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            await TryTransactionAsync(async () =>
            {
                var temp = await CanAppointmentTransactionAsync(dto.AppointmentDate, dto.AppointmentStartTime, dto.AppointmentEndTime);
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

                var status = (await _connection.QueryAsync<AppointmentStatus>("select Status from SmartAppointment where [ID]=@ID and CustomerID=@CustomerID", dto, _transaction)).FirstOrDefault();
                if (status == AppointmentStatus.Yes)
                {
                    result.Message = "对不起，上门预约不允许修改！";
                    return false;
                }

                await _connection.ExecuteAsync(
                    @"update [SmartAppointment] set AppointmentDate=@AppointmentDate,AppointmentStartTime=@AppointmentStartTime,AppointmentEndTime=@AppointmentEndTime,Content=@Content 
                    where [ID]=@ID and CustomerID=@CustomerID",
                    dto, _transaction);

                await UpdateCustomerAppointment(dto.CustomerID, dto.AppointmentDate);

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
        public async Task<IFlyDogResult<IFlyDogResultType, AppointmentDetail>> GetDetail(long ID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, AppointmentDetail>();

            await TryExecuteAsync(async () =>
            {
                result.Data = (await _connection.QueryAsync<AppointmentDetail>(
                    @"select a.ID,a.CustomerID,a.HospitalID,a.CreateTime,a.AppointmentDate,a.AppointmentStartTime,a.AppointmentEndTime,a.Content,'【'+d.Name+'】【'+b.Name+'】' as CreateUserName,c.Name as HospitalName from SmartAppointment a
                    left join SmartUser b on a.CreateUserID=b.ID
                    left join SmartHospital c on a.HospitalID=c.ID
                    left join SmartHospital d on b.HospitalID=d.ID
                    where a.ID=@ID", new { ID = ID })).FirstOrDefault();

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }


        private async Task UpdateCustomerAppointment(long customerID, DateTime time, long hospitalID = 0)
        {
            var appointmentTime = (await _connection.QueryAsync<DateTime?>(@"select LastAppointmentTime from SmartCustomer where ID=@ID", new { ID = customerID }, _transaction)).FirstOrDefault();
            bool isUpdate = false;
            if (appointmentTime == null)
            {
                isUpdate = true;
            }
            else
            {
                if (appointmentTime <= DateTime.Today)
                {
                    isUpdate = true;
                }
                else
                {
                    if (appointmentTime > time.Date)
                    {
                        isUpdate = true;
                    }
                }
            }

            if (isUpdate)
            {
                if (hospitalID == 0)
                {
                    await _connection.ExecuteAsync(@"update SmartCustomer set LastAppointmentTime=@LastAppointmentTime where ID=@ID",
                    new { LastAppointmentTime = time.Date, ID = customerID }, _transaction);
                }
                else
                {
                    await _connection.ExecuteAsync(@"update SmartCustomer set LastAppointmentTime=@LastAppointmentTime,LastAppointmentHospitalID=@LastAppointmentHospitalID where ID=@ID",
                    new { LastAppointmentTime = time.Date, LastAppointmentHospitalID = hospitalID, ID = customerID }, _transaction);
                }

            }
        }
    }
}
