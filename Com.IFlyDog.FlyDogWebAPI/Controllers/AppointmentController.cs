using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.WebAPI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Com.IFlyDog.FlyDogWebAPI.Controllers
{
    /// <summary>
    /// 预约相关接口
    /// </summary>
    public class AppointmentController : ApiController
    {
        private IAppointmentService _appointmentService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="appointmentService">服务</param>
        public AppointmentController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }
        /// <summary>
        /// 添加预约
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> AppointmentAdd(AppointmentAdd dto)
        {
            return await _appointmentService.AppointmentAdd(dto);
        }

        /// <summary>
        /// 获取今日新增预约
        /// </summary>
        /// <param name="hospitalID">医院ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<AppointmentToday>>> GetAppointmentToday(long hospitalID)
        {
            return await _appointmentService.GetAppointmentToday(hospitalID);
        }

        /// <summary>
        /// 获取今日上门预约
        /// </summary>
        /// <param name="hospitalID">医院ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<AppointmentComeToday>>> GetAppointmentComeToday(long hospitalID)
        {
            return await _appointmentService.GetAppointmentComeToday(hospitalID);
        }

        /// <summary>
        /// 获取预约详细信息
        /// </summary>
        /// <param name="ID">预约记录ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, AppointmentDetail>> GetDetail(long ID)
        {
            return await _appointmentService.GetDetail(ID);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="dto">修改信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Update(AppointmentUpdate dto)
        {
            return await _appointmentService.Update(dto);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dto">删除信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Delete(AppointmentDelete dto)
        {
            return await _appointmentService.Delete(dto);
        }
    }
}
