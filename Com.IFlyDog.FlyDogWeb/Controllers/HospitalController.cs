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
    /// 医院控制器
    /// </summary>
    public class HospitalController : Controller
    {
        // GET: Hospital
        public ActionResult Index()
        {
            return View();
        }

        #region 根据id查询医院下的子医院
        /// <summary>
        /// 根据id查询医院下的子医院
        /// </summary>
        /// <returns></returns>
        public async Task<string> HospitalGet()
        {
            var d = new Dictionary<string, string>();
            d.Add("id", "0");
            var result = await WebAPIHelper.Get("/api/Hospital/Get", d);
            return result;
        }
        #endregion
    }
}