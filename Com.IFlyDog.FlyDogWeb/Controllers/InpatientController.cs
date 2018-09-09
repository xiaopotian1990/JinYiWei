using Com.IFlyDog.APIDTO;
using Com.IFlyDog.FlyDogWeb.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Com.IFlyDog.FlyDogWeb.Controllers
{
    /// <summary>
    /// 住院工作台
    /// </summary>
    public class InpatientController : Controller
    {
        // GET: Inpatient
        public ActionResult Inpatient()
        {
            return View();
        }

        /// <summary>
        /// 住院工作台住院列表
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        public async Task<string> GetIn(string id)
        {
            var dic = new Dictionary<string, string> { { "hospitalID", IDHelper.GetHospitalID().ToString() } };
            var result = await WebAPIHelper.Get("/api/Inpatient/GetIn", dic);
            return result;
        }

        /// <summary>
        /// 住院
        /// </summary>
        /// <param name="dto">住院信息</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> In(InpatientAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/Inpatient/In", dto);
            return result;
        }

        /// <summary>
        /// 住院工作台住院列表
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> Out(Inpatientout dto)
        {
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/Inpatient/Out", dto);
            return result;
        }
    }
}