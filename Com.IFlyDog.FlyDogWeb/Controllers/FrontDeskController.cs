using Com.IFlyDog.FlyDogWeb.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Com.IFlyDog.APIDTO;

namespace Com.IFlyDog.FlyDogWeb.Controllers
{
    /// <summary>
    /// 前台接待
    /// </summary>
    public class FrontDeskController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        #region 今日上门

        public virtual async Task<string> GetVisitTodayAsync()
        {
            var hospitalId = IDHelper.GetHospitalID().ToString();
            var dic = new Dictionary<string, string> { { "hospitalID", hospitalId } };
            var result = await WebAPIHelper.Get("/api/FrontDesk/GetVisitTodayAsync", dic);
            return result;

        }
        #endregion

        #region  分诊时获取的顾客信息 

        public virtual async Task<string> GetCustomerInfoBefaultTriageAsync(string customerId)
        {
            var dic = new Dictionary<string, string>
            {
                {"customerID", customerId},
                {"hospitalID", IDHelper.GetHospitalID().ToString()}
            };
            var result = await WebAPIHelper.Get("/api/FrontDesk/GetCustomerInfoBefaultTriageAsync", dic);
            return result;

        }
        #endregion

        #region  今日分诊记录

        public virtual async Task<string> GetTriageTodayAsync()
        {
            var hospitalId = IDHelper.GetHospitalID().ToString();
            var dic = new Dictionary<string, string> { { "hospitalID", hospitalId } };
            var result = await WebAPIHelper.Get("/api/FrontDesk/GetTriageTodayAsync", dic);
            return result;

        }
        #endregion

        #region 添加分诊信息
        [HttpPost]
        public virtual async Task<string> AddTriageAsync(TriageAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/FrontDesk/AddTriageAsync", dto);
            return result;

        }
        #endregion

        #region  今日候诊记录

        public virtual async Task<string> GetWaitTodayAsync()
        {
            var hospitalId = IDHelper.GetHospitalID().ToString();
            var dic = new Dictionary<string, string> { { "hospitalID", hospitalId } };
            var result = await WebAPIHelper.Get("/api/FrontDesk/GetWaitTodayAsync", dic);
            return result;

        }
        #endregion

        #region 添加候诊信息
        [HttpPost]
        public virtual async Task<string> AddWaitAsync(WaitAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/FrontDesk/AddWaitAsync", dto);
            return result;
        }
        #endregion
    }
}