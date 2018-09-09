using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Com.IFlyDog.FlyDogWeb.Helper;

namespace Com.IFlyDog.FlyDogWeb.Controllers
{
    public class TreatSchedulerController : Controller
    {
        // GET: 治疗预约
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 查询治疗预约
        /// </summary>
        /// <param name="hospitalId"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        public async Task<string> TreatGet(string hospitalId,string startTime, string endTime)
         {
            var dic = new Dictionary<string, string>
            {
                {"hospitalID",hospitalId == "" ? IDHelper.GetHospitalID().ToString():hospitalId},
                {"startTime", startTime},
                 {"endTime", endTime},
            };

            var result = await WebAPIHelper.Get("/api/Treat/Get", dic);
            return result;
        }
    }
}