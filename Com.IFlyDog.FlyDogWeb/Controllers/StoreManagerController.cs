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
    /// 店家负责人控制器
    /// </summary>
    public class StoreManagerController : Controller
    {
       /// <summary>
       /// 店家负责人用户展示页
       /// </summary>
       /// <returns></returns>
        public ActionResult StoreUserManagerInfo()
        {
            return View();
        }

        /// <summary>
        /// 店家负责人店铺展示页
        /// </summary>
        /// <returns></returns>
        public ActionResult StoreManagerInfo()
        {
            return View();
        }

        #region 根据医院id查询当前医院所有的店铺负责人信息
        /// <summary>
        ///根据医院id查询当前医院所有的店铺负责人信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> GetByHospitalIDData()
        {
            var d = new Dictionary<string, string>();
            d.Add("hospitalID", IDHelper.GetHospitalID().ToString());
            d.Add("userID",IDHelper.GetUserID().ToString());
            var result = await WebAPIHelper.Get("/api/StoreManager/GetByHospitalID", d);
            return result;
        }
        #endregion

        #region 根据用户id查询用户所管辖的店铺信息
        /// <summary>
        ///根据用户id查询用户所管辖的店铺信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> GetUserIDData(string userID)
        {
            var d = new Dictionary<string, string>();
            d.Add("userID", userID);
            var result = await WebAPIHelper.Get("/api/StoreManager/GetUserID", d);
            return result;
        }
        #endregion

        #region 新增店铺负责人信息
        /// <summary>
        /// 新增店铺负责人信息
        /// </summary>
        /// <param name="storeManagerAdd"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> StoreManagerAdd(StoreManagerAdd storeManagerAdd)
        {
            storeManagerAdd.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/StoreManager/Add", storeManagerAdd);
            return result;
        }
        #endregion

        #region 删除店铺负责人下店铺
        /// <summary>
        /// 删除店铺负责人下店铺
        /// </summary>
        /// <param name="storeManagerDelete"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> StoreManagerDelete(StoreManagerDelete storeManagerDelete)
        {
            storeManagerDelete.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/StoreManager/Delete", storeManagerDelete);
            return result;
        }
        #endregion

        #region 删除店铺负责人
        /// <summary>
        /// 删除店铺负责人
        /// </summary>
        /// <param name="storeManagerDelete"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> StoreManagerUserDelete(StoreManagerDelete storeManagerDelete)
        {
            storeManagerDelete.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/StoreManager/DeleteByUserID", storeManagerDelete);
            return result;
        }
        #endregion
    }
}