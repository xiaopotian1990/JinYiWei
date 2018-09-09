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
    public class StoreController : Controller
    {
        // GET: Store

        /// <summary>
        /// 店家页面
        /// </summary>
        /// <returns></returns>
        public ActionResult StoreInfo()
        {
            return View();
        }

        /// <summary>
        /// 店铺基础数据页面
        /// </summary>
        /// <returns></returns>
        public ActionResult StoreData(string storeID) {
            return View();
        }

        #region 查询所有数据(分页)
        public async Task<string> StoreGet(StoreSelect dto)
        { 
            dto.HospitalID = IDHelper.GetHospitalID().ToString();
            dto.CrateUserID= IDHelper.GetUserID().ToString();
            var result = await WebAPIHelper.Post("/api/Store/Get",
            dto);
            return result;
        }
        #endregion

        #region 查询所有数据(不分页，主要给弹窗使用)
        public async Task<string> StoreGetNoPage(StoreSelect dto)
        {
            dto.HospitalID = IDHelper.GetHospitalID().ToString();
            dto.CrateUserID= IDHelper.GetUserID().ToString();
            var result = await WebAPIHelper.Post("/api/Store/GetNoPageStoreInfo",
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
        public async Task<string> StoreGetByID(string id)
        {
            var d = new Dictionary<string, string>();
            d.Add("id", id);
            var result = await WebAPIHelper.Get("/api/Store/GetByID", d);
            return result;
        }

        /// <summary>
        ///     修改数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> StoreEdit(StoreUpdate dto)
        {
            dto.HospitalID = IDHelper.GetHospitalID();
            dto.CrateUserID = IDHelper.GetUserID().ToString();
            var result = await WebAPIHelper.Post("/api/Store/Update", dto);
            return result;
        }

        /// <summary>
        ///     添加数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> StoreAdd(StoreAdd dto)
        {
            dto.HospitalID = IDHelper.GetHospitalID().ToString();
            dto.CreateUserID = IDHelper.GetUserID().ToString();
            var result = await WebAPIHelper.Post("/api/Store/Add", dto);
            return result;
        }

        #region 删除数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="smartSupplierDelete"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> StoreDelete(StoreDelete dto)
        {
            dto.HospitalID = IDHelper.GetHospitalID();
            dto.CreateUserID= IDHelper.GetUserID().ToString();
            var result = await WebAPIHelper.Post("/api/Store/Delete", dto);
            return result;
        }
        #endregion


        /// <summary>
        ///     根据店家id获取店家基础信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> GetByIDStoreBasicData(string id)
        {
            var d = new Dictionary<string, string>();
            d.Add("id", id);
            var result = await WebAPIHelper.Get("/api/Store/GetByIDStoreBasicData", d);
            return result;
        }

        /// <summary>
        ///     根据店家id获取店家佣金记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> GetByIDStoreCommissionData(string id)
        {
            var d = new Dictionary<string, string>();
            d.Add("id", id);
            var result = await WebAPIHelper.Get("/api/Store/GetByIDStoreCommissionData", d);
            return result;
        }



        /// <summary>
        ///     根据店家id获取店家客户列表
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> GetByIDStoreManagerData(string id)
        {
            var d = new Dictionary<string, string>();
            d.Add("id", id);
            var result = await WebAPIHelper.Get("/api/Store/GetByIDStoreManagerData", d);
            return result;
        }


        /// <summary>
        ///     根据店家id获取店家回款记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> GetByIDStoreSaleBackData(string id)
        {
            var d = new Dictionary<string, string>();
            d.Add("id", id);
            var result = await WebAPIHelper.Get("/api/Store/GetByIDStoreSaleBackData", d);
            return result;
        }

    }
}