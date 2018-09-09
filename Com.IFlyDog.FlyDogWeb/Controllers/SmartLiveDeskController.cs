using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Com.IFlyDog.FlyDogWeb.Helper;

namespace Com.IFlyDog.FlyDogWeb.Controllers
{
    public class SmartLiveDeskController : Controller
    {
        // 接诊工作台 
        public ActionResult Index()
        {
            return View();
        }

        //接诊查询当天人员
        public async Task<string> GetReceptionTodayAsync()
        {
            var dic = new Dictionary<string, string>
            {
                { "hospitalID", IDHelper.GetHospitalID().ToString() },
                { "userID", IDHelper.GetUserID().ToString() }
            };
            var result = await WebAPIHelper.Get("/api/Reception/GetReceptionTodayAsync", dic);
            return result;
        }

    }
}