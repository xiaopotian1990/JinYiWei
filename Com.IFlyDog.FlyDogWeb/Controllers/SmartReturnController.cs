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
    /// 退货信息控制器
    /// </summary>
    public class SmartReturnController : Controller
    {

        /// <summary>
        ///退货详情页面（首页数据加载页）
        /// </summary>
        /// <returns></returns>
        // GET: SmartReturn
        public ActionResult SmartReturnInfo()
        {
            return View();
        }

        /// <summary>
        /// 退货详情详细页
        /// </summary>
        /// <returns></returns>
        public ActionResult SmartReturnIndex() {
            return View();
        }

        #region 查询所有数据
        public async Task<string> SmartReturnGet(SmartReturnSelect dto)
        {
            dto.CreateUserId = IDHelper.GetUserID().ToString();
            var result = await WebAPIHelper.Post("/api/SmartReturn/Get",
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
        public async Task<string> SmartReturnGetByID(SmartReturnInfo dto)
        {
            var d = new Dictionary<string, string>();
            d.Add("ID", dto.ID.ToString());

            var result = await WebAPIHelper.Get("/api/SmartReturn/GetByID", d);
            return result;
        }

        /// <summary>
        ///     修改数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SmartReturnEdit(SmartReturnUpdate dto)
        {
            dto.CreateUserID = IDHelper.GetUserID().ToString();
            var result = await WebAPIHelper.Post("/api/SmartReturn/Update", dto);
            return result;
        }

        /// <summary>
        ///     添加数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SmartReturnAdd(SmartReturnAdd dto)
        {

            dto.CreateUserID = IDHelper.GetUserID().ToString();
            var result = await WebAPIHelper.Post("/api/SmartReturn/Add", dto);
            return result;
        }

        #region 删除数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="smartSupplierDelete"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SmartReturnDelete(SmartReturnDelete dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/SmartReturn/Delete", dto);
            return result;
        }
        #endregion

        /// <summary>
        /// 退货出库打印页
        /// </summary>
        /// <returns></returns>
        public ActionResult SmartReturnPrint(string returnID) {
            return View();
        }

        /// <summary>
        /// 打印退货出库数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SmartReturnPrintFun(string returnID)
        {
            var d = new Dictionary<string, string>();
            d.Add("returnID", returnID);
            d.Add("hospitalID",IDHelper.GetHospitalID().ToString());
            var result = await WebAPIHelper.Get("/api/SmartReturn/SmartReturnPrint", d);
            return result;
        }
    }
}