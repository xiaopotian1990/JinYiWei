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
    /// 科室领用控制器
    /// </summary>
    public class UseController : Controller
    {
        /// <summary>
        /// 科室领用详情页面（首页数据加载页）
        /// </summary>
        /// <returns></returns>
        // GET: Use
        public ActionResult UseInfo()
        {
            return View();
        }

        /// <summary>
        /// 科室领用详情详细页
        /// </summary>
        /// <returns></returns>
        public ActionResult UseIndex()
        {
            return View();
        }

        #region 查询所有数据
        public async Task<string> UseGet(UseSelect dto)
        {
            //SmartPurchaseSelect smartSupplierSelect = new SmartPurchaseSelect();
            //dto.HospitalID = 1;

            dto.CreateUserId = IDHelper.GetUserID().ToString();
            var result = await WebAPIHelper.Post("/api/Use/Get",
            dto);
            return result;
        }
        #endregion

        /// <summary>
        ///     通过ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> UseGetByID(UseSelect dto)
        {
            var d = new Dictionary<string, string>();
            d.Add("ID", dto.ID.ToString());

            var result = await WebAPIHelper.Get("/api/Use/GetByID", d);
            return result;
        }

        /// <summary>
        ///     添加数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> UseAdd(UseAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID().ToString();
            //dto.HospitalID = "1";//所属医院id先写死，到时候改成获取的
            var result = await WebAPIHelper.Post("/api/Use/Add", dto);
            return result;
        }

        /// <summary>
        /// 科室领用打印页
        /// </summary>
        /// <returns></returns>
        public ActionResult SmartUsePrint(string UseID) {
            return View();
        }

        /// <summary>
        ///     打印科室领用数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SmartUsePrintFun(string UseID)
        {
            var d = new Dictionary<string, string>();
            d.Add("UseID", UseID);
            d.Add("hospitalID",IDHelper.GetHospitalID().ToString());
            var result = await WebAPIHelper.Get("/api/Use/SmartUsePrint", d);
            return result;
        }
    }
}