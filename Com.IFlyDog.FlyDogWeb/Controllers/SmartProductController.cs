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
    public class SmartProductController : Controller
    {

        /// <summary>
        /// 药物品信息页面
        /// </summary>
        /// <returns></returns>
        // GET: SmartProduct
        public ActionResult SmartProductInfo()
        {
            return View();
        }
       

        #region 查询数据
        public async Task<string> SmartProductInfoGet(SmartProductSelect dto)
        {
            var result = await WebAPIHelper.Post("/api/SmartProduct/Get",
            dto);
            return result;
        }

        /// <summary>
        /// 查询所有药物品信息
        /// </summary>
        /// <returns></returns>
        public async Task<string> SmartProductInfoGetAll(SmartProductSelect dto)
        {
            var result = await WebAPIHelper.Post("/api/SmartProduct/GetAll", dto);
            return result;
        }

        /// <summary>
        /// 查询所有药物品信息(根据仓库id，以及类型)
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetByWarehouseIdDataAll(SmartProductSelect dto)
        {
            var result = await WebAPIHelper.Post("/api/SmartProduct/GetByWarehouseIDDataAll", dto);
            return result;
        }
        #endregion

        #region 根据id获取药物品信息
        /// <summary>
        /// 根据id获取药物品信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> SmartProductGet(string id)
        {
            var d = new Dictionary<string, string>();
            d.Add("ID", id);
            var result = await WebAPIHelper.Get("/api/SmartProduct/GetByID", d);
            return result;
        }
        #endregion

        #region 修改药物品数据
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <returns></returns>
        public async Task<string> SmartProductEdit(SmartProductUpdate dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/SmartProduct/Update", dto);
            return result;
        }
        #endregion

        #region 添加药物品信息
        /// <summary>
        /// 添加数据
        /// </summary>
        ///  /// <returns></returns>
        public async Task<string> SmartProductAdd(SmartProductAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/SmartProduct/Add", dto);
            return result;
        }
        #endregion
    }
}