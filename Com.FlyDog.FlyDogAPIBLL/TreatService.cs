using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
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
    public class TreatService : BaseService, ITreatService
    {
        /// <summary>
        /// 系统设置服务
        /// </summary>
        private IOptionService _optionService;
        /// <summary>
        /// 用户服务
        /// </summary>
        private ISmartUserService _userService;
        public TreatService(IOptionService optionService, ISmartUserService userService)
        {
            _optionService = optionService;
            _userService = userService;
        }
        /// <summary>
        /// 添加预约
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Add(TreatAdd dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;


            if (dto.ChargeID == 0)
            {
                result.Message = "请选择预约项目";
                return result;
            }
            if (dto.UserID == 0)
            {
                result.Message = "请选择医生";
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

            await TryExecuteAsync(async () =>
            {
                var temp = await CanTreatAsync(dto.AppointmentDate, dto.AppointmentStartTime, dto.AppointmentEndTime, dto.UserID);
                if (temp.ResultType != IFlyDogResultType.Success)
                {
                    result.Message = temp.Message;
                    return;
                }

                //var num = (await HasCustomerOAuthAsync(dto.CreateUserID, dto.CustomerID)).FirstOrDefault();

                if (!await HasCustomerOAuthAsync(dto.CreateUserID, dto.CustomerID))
                {
                    result.Message = "对不起，您无权操作该用户！";
                    result.ResultType = IFlyDogResultType.NoAuth;
                    return;
                }

                await _connection.ExecuteAsync(
                    @"insert into [SmartTreat]([ID],[CustomerID],[CreateUserID],[CreateTime],[AppointmentDate],[AppointmentStartTime],[AppointmentEndTime],[UserID],[ChargeID],[Status],[Remark],[HospitalID]) 
                    values(@ID,@CustomerID,@CreateUserID,@CreateTime,@AppointmentDate,@AppointmentStartTime,@AppointmentEndTime,@UserID,@ChargeID,@Status,@Remark,@HospitalID)",
                    new
                    {
                        ID = SingleIdWork.Instance(Key.WorkID, Key.DataCenterID).nextId(),
                        CustomerID = dto.CustomerID,
                        CreateUserID = dto.CreateUserID,
                        CreateTime = DateTime.Now,
                        AppointmentDate = dto.AppointmentDate,
                        AppointmentStartTime = dto.AppointmentStartTime,
                        AppointmentEndTime = dto.AppointmentEndTime,
                        UserID = dto.UserID,
                        ChargeID = dto.ChargeID,
                        Remark = dto.Remark,
                        Status = AppointmentStatus.No,
                        HospitalID = dto.HospitalID
                    });

                result.Message = "添加成功";
                result.ResultType = IFlyDogResultType.Success;
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

            await TryExecuteAsync(async () =>
            {
                if (!await HasCustomerOAuthAsync(dto.CreateUserID, dto.CustomerID))
                {
                    result.Message = "对不起，您无权操作该用户！";
                    result.ResultType = IFlyDogResultType.NoAuth;
                    return;
                }

                var count = await _connection.ExecuteAsync(@"delete from [SmartTreat] where [ID]=@ID and CustomerID=@CustomerID and Status=@Status",
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
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Update(TreatUpdate dto)
        {
            var result = new IFlyDogResult<IFlyDogResultType, int>();
            result.ResultType = IFlyDogResultType.Failed;

            if (dto.ChargeID == 0)
            {
                result.Message = "请选择预约项目";
                return result;
            }
            if (dto.UserID == 0)
            {
                result.Message = "请选择医生";
                return result;
            }

            await TryExecuteAsync(async () =>
            {
                var temp = await CanTreatAsync(dto.AppointmentDate, dto.AppointmentStartTime, dto.AppointmentEndTime, dto.UserID, dto.ID);
                if (temp.ResultType != IFlyDogResultType.Success)
                {
                    result.Message = temp.Message;
                    return;
                }


                if (!await HasCustomerOAuthAsync(dto.CreateUserID, dto.CustomerID))
                {
                    result.Message = "对不起，您无权操作该用户！";
                    result.ResultType = IFlyDogResultType.NoAuth;
                    return;
                }

                var status = (await _connection.QueryAsync<AppointmentStatus>("select Status from SmartTreat where [ID]=@ID and CustomerID=@CustomerID", dto)).FirstOrDefault();
                if (status == AppointmentStatus.Yes)
                {
                    result.Message = "对不起，上门预约不允许修改！";
                    return;
                }

                await _connection.ExecuteAsync(
                    @"update [SmartTreat] set AppointmentDate=@AppointmentDate,AppointmentStartTime=@AppointmentStartTime,AppointmentEndTime=@AppointmentEndTime,[UserID]=@UserID,[ChargeID]=@ChargeID,Remark=@Remark 
                    where [ID]=@ID and CustomerID=@CustomerID",
                    dto);

                result.Message = "修改成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 获取预约详细信息
        /// </summary>
        /// <param name="ID">预约记录ID</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, TreatDetail>> GetDetail(long ID)
        {
            var result = new IFlyDogResult<IFlyDogResultType, TreatDetail>();

            await TryExecuteAsync(async () =>
            {
                result.Data = (await _connection.QueryAsync<TreatDetail>(
                    @"select a.ID,a.CustomerID,a.HospitalID,a.CreateTime,a.AppointmentDate,a.AppointmentStartTime,a.AppointmentEndTime,a.Remark,'【'+d.Name+'】【'+b.Name+'】' as CreateUserName,
                    a.ChargeID,e.Name as ChargeName,a.UserID,f.Name as UserName,c.Name as HospitalName 
                    from SmartTreat a
                    left join SmartUser b on a.CreateUserID=b.ID
					left join SmartCharge e on a.ChargeID=e.ID
					left join SmartUser f on a.UserID=f.ID
                    left join SmartHospital c on a.HospitalID=c.ID
                    left join SmartHospital d on b.HospitalID=d.ID
                    where a.ID=@ID", new { ID = ID })).FirstOrDefault();

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 获取预约记录
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<Treat>>> Get(long hospitalID, DateTime startTime, DateTime endTime)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<Treat>>();

            await TryExecuteAsync(async () =>
            {
                result.Data = await _connection.QueryAsync<Treat>(
                    @"select a.ID,a.CustomerID,c.Name as CustomerName,a.CreateTime,a.AppointmentDate,a.AppointmentStartTime,a.AppointmentEndTime,a.Remark,'【'+d.Name+'】【'+b.Name+'】' as CreateUserName,
                    e.Name as ChargeName,f.Name as UserName,a.Status
                    from SmartTreat a
                    left join SmartUser b on a.CreateUserID=b.ID
					left join SmartCustomer c on a.CustomerID=c.ID
					left join SmartCharge e on a.ChargeID=e.ID
					left join SmartUser f on a.UserID=f.ID
                    left join SmartHospital d on b.HospitalID=d.ID
					where a.HospitalID=@HospitalID and a.AppointmentDate between @StartTime and @EndTime", new { HospitalID = hospitalID, StartTime = startTime.Date, EndTime = endTime.Date });

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }

        /// <summary>
        /// 获取预约记录
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<TreatDay>>> GetDayDetail(long hospitalID, DateTime date)
        {
            var result = new IFlyDogResult<IFlyDogResultType, IEnumerable<TreatDay>>();

            await TryExecuteAsync(async () =>
            {
                TreatDay treat = new TreatDay();
                var option = _optionService.Get().Data;
                TimeSpan start = TimeSpan.Parse(option.MakeBeginTimeValue);
                TimeSpan end = TimeSpan.Parse(option.MakeEndTimeValue);
                int time = Convert.ToInt32(option.MakeTimeIntervalValue);

                treat.HeaderList.Add("时间段");
                for (int i = 1; i < (end - start).Minutes / time; i++)
                {
                    treat.HeaderList.Add(start.ToString() + "-" + end.ToString());
                }

                result.Data = await _connection.QueryAsync<TreatDay>(
                    @"SELECT DISTINCT a.ID,a.Name+'  |   '+ case when c.Name is null then '无排班信息' else c.Name end as Name,f.ID as TreatID,g.Name as CustomerName,
                    f.AppointmentStartTime,f.AppointmentEndTime,h.Name as ChargeName,f.Remark
                    FROM dbo.SmartUser a 
                    left JOIN dbo.SmartUserRole d ON a.ID=d.UserID
                    inner JOIN dbo.SmartRole e ON d.RoleID=e.ID AND e.SSYY=1
                    left JOIN SmartShift b ON a.ID=b.UserID and b.ShiftDate=@Date
					left join SmartShiftCategory c on b.CategoryID=c.ID
					left join SmartTreat f on a.ID=f.UserID and f.AppointmentDate=@Date
					left join SmartCustomer g on f.CustomerID=g.ID
					left join SmartCharge h on f.ChargeID=h.ID
					where a.HospitalID=@HospitalID and a.Status=1  ", new { HospitalID = hospitalID, Date = date.Date });

                result.Message = "查询成功";
                result.ResultType = IFlyDogResultType.Success;
            });

            return result;
        }
    }
}
