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
    public class SmartWarehouseController : Controller
    {
        /// <summary>
        /// 仓库管理页面(查询全部)
        /// </summary>
        /// <returns></returns>
        // GET: SmartWarehouse
        public ActionResult SmartWarehouseInfo()
        {
            return View();
        }

        /// <summary>
        /// 添加仓库管理详细
        /// </summary>
        /// <returns></returns>
        public ActionResult SmartWarehouseDetaiIndex()
        {
            return View();
        }

        /// <summary>
        /// 查询仓库管理全部
        /// </summary>
        /// <returns></returns>
        public async Task<string> SmartWarehouseGet()
        {
            string hospitalID = IDHelper.GetHospitalID().ToString();
            var d = new Dictionary<string, string>();
            d.Add("hospitalId", hospitalID);
            var result = await WebAPIHelper.Get("/api/SmartWarehouse/Get", d);
            return result;
        }

        /// <summary>
        /// 根据医院id查询医院下的仓库
        /// </summary>
        /// <param name="hospitalId"></param>
        /// <returns></returns>
        public async Task<string> SmartWarehouseGetByHospitalId(string hospitalId)
        {
            var d = new Dictionary<string, string>();
            d.Add("hospitalId", hospitalId);
            var result = await WebAPIHelper.Get("/api/SmartWarehouse/Get", d);
            return result;
        }

        /// <summary>
        ///     通过医院id查询医院下的仓库
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> WarehouseGetByHospitalID(SmartWarehouseInfo dto)
        {
            var d = new Dictionary<string, string>();
            d.Add("hospitalId", dto.HospitalID); //所属医院id先写死

            var result = await WebAPIHelper.Get("/api/SmartWarehouse/Get", d);
            return result;
        }

        /// <summary>
        ///     通过ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SmartWarehouseGetByID(SmartWarehouseInfo dto)
        {
            var d = new Dictionary<string, string>();
            d.Add("ID", dto.ID);

            var result = await WebAPIHelper.Get("/api/SmartWarehouse/GetByID", d);
            return result;
        }

        /// <summary>
        ///     修改数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SmartWarehouseEdit(SmartWarehouseUpdate dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID().ToString();
            var result = await WebAPIHelper.Post("/api/SmartWarehouse/Update", dto);
            return result;
        }

        /// <summary>
        ///     停用数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<string> SmartWarehouseDisable(SmartWarehouseStopOrUse dto)
        {
            var d = new Dictionary<string, string>();
            d.Add("CreateUserID", IDHelper.GetUserID().ToString());
            var result = WebAPIHelper.Post("/api/SmartWarehouse/StopOrUse", d);
            return result;
        }

        /// <summary>
        ///     添加数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SmartWarehouseAdd(SmartWarehouseAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID().ToString();
            var result = await WebAPIHelper.Post("/api/SmartWarehouse/Add", dto);
            return result;
        }

        #region 删除仓库数据
        /// <summary>
        /// 删除仓库数据
        /// </summary>
        /// <param name="smartWarehouseDelete"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SmartWarehouseDelete(SmartWarehouseDelete smartWarehouseDelete)
        {
            string userId = IDHelper.GetUserID().ToString();
            var dic = new Dictionary<string, string>();
            dic.Add("CreateUserID", userId);
            dic.Add("ID", smartWarehouseDelete.ID.ToString());
            dic.Add("HospitalID",IDHelper.GetHospitalID().ToString());
            var result = await WebAPIHelper.Post("/api/SmartWarehouse/Delete", dic);
            return result;
        }
        #endregion

    }
}