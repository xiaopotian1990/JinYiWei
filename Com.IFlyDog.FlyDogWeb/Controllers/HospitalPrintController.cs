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
    /// 打印设置控制器
    /// </summary>
    public class HospitalPrintController : Controller
    {

        /// <summary>
        ///打印设置页
        /// </summary>
        /// <returns></returns>
        // GET: HospitalPrint
        public ActionResult HospitalPrintInfo()
        {
            return View();
        }

        /// <summary>
        /// /打印设置页
        /// </summary>
        /// <returns></returns>
        public ActionResult HospitalPrintSet(string hospitalID) {
            return View();
        }

        #region 查询所有打印设置
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<string> HospitalPrintGet(string hospitalID)
        {
            var d = new Dictionary<string, string>();
            d.Add("hospitalID", hospitalID);
            var result = await WebAPIHelper.Get("/api/HospitalPrint/Get",d);
            return result;
        }
        #endregion

        #region 根据id查询打印设置详情
        /// <summary>
        ///根据id查询打印设置详情
        /// </summary>
        /// <param name="id">根据id查询打印设置详情</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> HospitalPrintEditGet(string id)
        {
            var d = new Dictionary<string, string>();
            d.Add("id", id);
            var result = await WebAPIHelper.Get("/api/HospitalPrint/GetByID", d);
            return result;
        }
        #endregion

        #region 更新打印设置
        /// <summary>
        /// 更新打印设置
        /// </summary>
        /// <param name="hospitalPrintUpdate"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> HospitalPrintSubmit(HospitalPrintUpdate hospitalPrintUpdate)
        {
            hospitalPrintUpdate.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/HospitalPrint/Update", hospitalPrintUpdate);
            return result;
        }
        #endregion

    }
}