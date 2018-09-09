using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Com.IFlyDog.FlyDogWeb.Helper;
using Com.IFlyDog.APIDTO;

namespace Com.IFlyDog.FlyDogWeb.Controllers
{
    public class AppointmentController : Controller
    {
        // GET: 预约
        public ActionResult Index()
        {
            return View();
        }
        #region 今日新增预约
        public async Task<string> GetAppointmentToday()
        {
            var hospitalId = IDHelper.GetHospitalID().ToString();
            var dic = new Dictionary<string, string> { { "hospitalID", hospitalId } };
            var result = await WebAPIHelper.Get("/api/Appointment/GetAppointmentToday", dic);
            return result;

        }
        #endregion

        #region 今日预约上门

        public async Task<string> GetAppointmentComeToday()
        {
            var hospitalId = IDHelper.GetHospitalID().ToString();
            var dic = new Dictionary<string, string> { { "hospitalID", hospitalId } };
            var result = await WebAPIHelper.Get("/api/Appointment/GetAppointmentComeToday", dic);
            return result;

        }
        #endregion

        /// <summary>
        /// 添加咨询预约
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> AppointmentAdd(AppointmentAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            return await WebAPIHelper.Post("/api/Appointment/AppointmentAdd", dto); ;
        }

        /// <summary>
        /// 获取预约详细信息
        /// </summary>
        /// <param name="ID">预约记录ID</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> GetDetail(long ID)
        {
            var dic = new Dictionary<string, string> { { "ID", ID.ToString() } };
            var result = await WebAPIHelper.Get("/api/Appointment/GetDetail", dic);
            return result;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="dto">修改信息</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> Update(AppointmentUpdate dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            return await WebAPIHelper.Post("/api/Appointment/Update", dto);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dto">删除信息</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> Delete(AppointmentDelete dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            return await WebAPIHelper.Post("/api/Appointment/Delete", dto);
        }
    }
}