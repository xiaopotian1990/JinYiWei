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
    /// 预收款设置
    /// </summary>
    public class DepositChargeController : Controller
    {
        /// <summary>
        /// 预收款设置页面
        /// </summary>
        /// <returns></returns>
        // GET: DepositCharge
        public ActionResult DepositChargeInfo()
        {
            return View();
        }

        #region 查询数据
        public async Task<string> DepositChargeInfoGet()
        {
            var result = await WebAPIHelper.Get("/api/DepositCharge/Get",
             new Dictionary<string, string>());
            return result;
        }
        #endregion

        #region 根据id获取信息
        /// <summary>
        /// 根据id获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> DepositChargeGet(string id)
        {
            var d = new Dictionary<string, string>();
            d.Add("ID", id);
            var result = await WebAPIHelper.Get("/api/DepositCharge/GetByID", d);
            return result;
        }
        #endregion

        #region 修改数据
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <returns></returns>
        public async Task<string> DepositChargeEdit(DepositChargeUpdate dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/DepositCharge/Update", dto);
            return result;
        }
        #endregion

        /// <summary>
        /// 添加数据
        /// </summary>
        ///  /// <returns></returns>
        public async Task<string> DepositChargeAdd(DepositChargeAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/DepositCharge/Add", dto);
            return result;
        }

    }
}