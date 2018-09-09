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
    /// 报表项目页面
    /// </summary>
    public class SmartItemController : Controller
    {
        /// <summary>
        /// 报表项目
        /// </summary>
        /// <returns></returns>
        // GET: SmartItem
        public ActionResult SmartItemInfo()
        {
            return View();
        }


        #region 查询所有报表项目
        /// <summary>
        /// 查询所有报表项目
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SmartItemPost()
        {
            var result = await WebAPIHelper.Get("/api/SmartItem/Get", new Dictionary<string, string>());
            return result;
        }
        #endregion


        #region 检测症状
        /// <summary>
        /// 检测症状
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SmartChickSymptomPost()
        {
            var result = await WebAPIHelper.Get("/api/Symptom/ItemGetSymptom", new Dictionary<string, string>());
            return result;
        }
        #endregion

        #region 检测收费项目
        /// <summary>
        /// 检测收费项目
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SmartChickChargePost(ChargeSelect dto)
        {
            var result = await WebAPIHelper.Post("/api/Charge/GetCheckCharge", dto);
            return result;
        }
        #endregion

        #region 根据id查询报表项目
        /// <summary>
        ///根据id查询报表项目
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SmartItemEditGet(string id)
        {
            var d = new Dictionary<string, string>();
            d.Add("id", id);
            var result = await WebAPIHelper.Get("/api/SmartItem/GetByID", d);
            return result;
        }
        #endregion

        #region 新增报表项目
        /// <summary>
        /// 新增报表项目
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SmartItemAdd(SmartItemAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/SmartItem/Add", dto);
            return result;
        }
        #endregion

        #region 更新报表项目
        /// <summary>
        /// 更新报表项目
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SmartItemSubmit(SmartItemUpdate dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/SmartItem/Update", dto);
            return result;
        }
        #endregion

        #region 删除报表项目
        /// <summary>
        /// 删除报表项目 dto
        /// </summary>
        /// <returns></returns>

        public async Task<string> SmartItemDelete(string id)
        {
            SmartItemDelete sid = new SmartItemDelete();
            sid.ID = id;
            sid.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/SmartItem/Delete", sid);
            return result;
        }
        #endregion

    }
}