using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Com.IFlyDog.FlyDogWeb.Helper;

namespace Com.IFlyDog.FlyDogWeb.Controllers
{
    public class ReceptionController : Controller
    {
        /// 接诊工作台
        public ActionResult Index()
        {
            return View();
        }

        #region 接诊工作台

        public virtual async Task<string> GetReceptionTodayAsync()
        {
            var hospitalId = IDHelper.GetHospitalID().ToString();
            var userId = IDHelper.GetUserID().ToString();
            var dic = new Dictionary<string, string> { { "hospitalID", hospitalId }, { "userID", userId } };
            var result = await WebAPIHelper.Get("/api/Reception/GetReceptionTodayAsync", dic);
            return result;

        }
        #endregion
    }
}