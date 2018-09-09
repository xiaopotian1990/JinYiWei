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
    public class SmartPurchaseController : Controller
    {
        /// <summary>
        /// 进货详情页面（首页数据加载页）
        /// </summary>
        /// <returns></returns>
        // GET: SmartPurchase
        public ActionResult SmartPurchaseInfo()
        {
            return View();
        }

        /// <summary>
        /// 进货详情详细页（加载药物品页面）
        /// </summary>
        /// <returns></returns>
        public ActionResult SmartPurchaseDetailIndex()
        {
            return View();
        }

        /// <summary>
        /// 到期预警页面
        /// </summary>
        /// <returns></returns>
        public ActionResult SmartPurchaseProductWarning() {
            return View();
        }

        #region 查询所有数据
        public async Task<string> SmartPurchaseGet(SmartPurchaseSelect dto)
        {
            dto.CreateUserID = IDHelper.GetUserID().ToString();
            var result = await WebAPIHelper.Post("/api/SmartPurchase/Get",
            dto);
            return result;
        }
        #endregion

        #region 根据医院id查询医院内的进货记录
        public async Task<string> SmartPurchaseByHospitalIDGet(SmartPurchaseSelect dto)
        {
            dto.HospitalID = IDHelper.GetHospitalID().ToString();
            var result = await WebAPIHelper.Post("/api/SmartPurchase/GetByHospitalID",
            dto);
            return result;
        }
        #endregion

        #region 根据医院id查询医院内的进货记录(查询出所有药物品，按照到期时间排序)
        public async Task<string> GetByHospitalIDDataGet(SmartPurchaseSelect dto)
        {
            dto.HospitalID = IDHelper.GetHospitalID().ToString();
            var result = await WebAPIHelper.Post("/api/SmartPurchase/GetByHospitalIDData",
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
        public async Task<string> SmartPurchaseGetByID(SmartPurchaseInfo dto)
        {
            var d = new Dictionary<string, string>();
            d.Add("ID", dto.ID.ToString());

            var result = await WebAPIHelper.Get("/api/SmartPurchase/GetByID", d);
            return result;
        }

        /// <summary>
        ///     修改数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SmartPurchaseEdit(SmartPurchaseUpdate dto)
        {
            dto.CreateUserID = IDHelper.GetUserID().ToString();
            var result = await WebAPIHelper.Post("/api/SmartPurchase/Update", dto);
            return result;
        }

        /// <summary>
        ///     添加数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SmartPurchaseAdd(SmartPurchaseAdd dto)
        {

            dto.CreateUserID = IDHelper.GetUserID().ToString();
            var result = await WebAPIHelper.Post("/api/SmartPurchase/Add", dto);
            return result;
        }

        #region 删除数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="smartSupplierDelete"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SmartPurchaseDelete(SmartPurchaseDelete smartPurchaseDelete)
        {
            smartPurchaseDelete.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/SmartPurchase/Delete", smartPurchaseDelete);
            return result;
        }
        #endregion

        /// <summary>
        /// 进货入库打印页面
        /// </summary>
        /// <returns></returns>
        public ActionResult SmartPurchasePrint(string purchaspID) {
            return View();
        }

        /// <summary>
        ///     打印
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SmartPurchasePrintFun(string purchaspID)
        {
            var d = new Dictionary<string, string>();
            d.Add("purchaspID", purchaspID);
            d.Add("hospitalID",IDHelper.GetHospitalID().ToString());
            var result = await WebAPIHelper.Get("/api/SmartPurchase/SmartPurchasePrint", d);
            return result;
        }
    }
}