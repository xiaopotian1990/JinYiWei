using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    public interface IAppointmentService
    {
        /// <summary>
        /// 添加预约
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> AppointmentAdd(AppointmentAdd dto);

        /// <summary>
        /// 获取今日新增预约
        /// </summary>
        /// <param name="hospitalID">医院ID</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<AppointmentToday>>> GetAppointmentToday(long hospitalID);

        /// <summary>
        /// 获取今日上门预约
        /// </summary>
        /// <param name="hospitalID">医院ID</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<AppointmentComeToday>>> GetAppointmentComeToday(long hospitalID);

        /// <summary>
        /// 获取预约详细信息
        /// </summary>
        /// <param name="ID">预约记录ID</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, AppointmentDetail>> GetDetail(long ID);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="dto">修改信息</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> Update(AppointmentUpdate dto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dto">删除信息</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> Delete(AppointmentDelete dto);
    }
}
