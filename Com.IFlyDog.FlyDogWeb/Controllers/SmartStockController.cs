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
    /// 库存查询
    /// </summary>
    public class SmartStockController : Controller
    {
        /// <summary>
        /// 库存管理
        /// </summary>
        /// <returns></returns>
        // GET: SmartStock
        public ActionResult SmartStockInfo()
        {
            return View();
        }

        #region 查询库存管理
        /// <summary>
        /// 查询库存管理
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<string> SmartStockInfoGet(SmartStockSelect dto)
        {
            dto.CreateUserId = IDHelper.GetUserID().ToString();
            var result = await WebAPIHelper.Post("/api/SmartStock/Get",
            dto);
            return result;
        }
        #endregion

        /// <summary>
        /// 查询库存管理中的商品，按照有效期排序
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<string> SmartStockGetByHospotaltionGet(SmartStockSelect dto)
        {
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/SmartStock/GetByHospitalIDData",
            dto);
            return result;
        }



        /// <summary>
        /// 根据条件查询库存的药物品信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<string> SmartStockConditionGet(SmartStockSelect dto)
        {
            var result = await WebAPIHelper.Post("/api/SmartStock/GetByConditionData",
           dto);
            return result;
        }
    }
}