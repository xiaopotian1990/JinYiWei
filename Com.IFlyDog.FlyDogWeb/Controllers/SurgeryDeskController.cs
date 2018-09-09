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
    public class SurgeryDeskController : Controller
    {
        // 手术工作台
        public ActionResult Index()
        {
            return View();
        }

        #region 查询手术

        /// <summary> 
        /// 查询手术
        /// </summary>
        /// <returns></returns>
        public async Task<string> SurgeryGet(string datetime)
        {
            var dic = new Dictionary<string, string>
            {
                {"hospitalID", IDHelper.GetHospitalID().ToString()},
                {"date", datetime},
            };

            var result = await WebAPIHelper.Get("/api/Surgery/Get", dic);
            return result;
        }
        #endregion

        #region 开始结束手术
        /// <summary> 
        /// 开始结束手术
        /// </summary>
        /// <returns></returns>
        public async Task<string> SugeryDone(SugeryDone dto)
        {
            var result = await WebAPIHelper.Post("/api/Surgery/Done", dto);
            return result;
        }
        #endregion


    }
}