using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.FlyDogWeb.Helper;

namespace Com.IFlyDog.FlyDogWeb.Controllers
{
    public class DeptSendController : Controller
    {
        // 科室发料
        public ActionResult Index()
        {
            return View();
        }

        #region 科室API
        /// <summary>
        /// 科室发料请求记录-待发货
        /// </summary>
        public async Task<string> GetDeptSend()
        {
            var dic = new Dictionary<string, string>
            {
                { "hospitalID", IDHelper.GetHospitalID().ToString() },
                { "userID", IDHelper.GetUserID().ToString() }

            };
            var result = await WebAPIHelper.Get("/api/DeptSend/GetDeptSendInfo", dic);
            return result;
        }

        /// <summary>
        /// 科室确认发货
        /// </summary>
        public async Task<string> Send(DeptSendAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/DeptSend/Send", dto);
            return result;
        }

        /// <summary>
        /// 今日发货记录
        /// </summary>
        public async Task<string> GetDeptSendToday()
        {
            var dic = new Dictionary<string, string>
            {
                { "hospitalID", IDHelper.GetHospitalID().ToString() },
                { "userID", IDHelper.GetUserID().ToString() }

            };
            var result = await WebAPIHelper.Get("/api/DeptSend/GetDeptSendToday", dic);
            return result;
        }
        #endregion

    }
}