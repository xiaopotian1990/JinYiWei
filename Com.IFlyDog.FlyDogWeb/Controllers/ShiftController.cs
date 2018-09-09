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
    /// 排班管理控制器
    /// </summary>
    public class ShiftController : Controller
    {
        /// <summary>
        /// 排班管理页
        /// </summary>
        /// <returns></returns>
        // GET: Shift
        public ActionResult ShiftInfo()
        {
            return View();
        }

        #region 查询所有排班管理
        /// <summary>
        /// 查询所有排班管理
        /// </summary>
        /// <returns></returns>
        public async Task<string> ShiftGet(string number="0")
        {
            var d = new Dictionary<string, string>();
            d.Add("hospitalID",IDHelper.GetHospitalID().ToString());
            d.Add("number", number);
            var result = await WebAPIHelper.Get("/api/Shift/Get",d);
            return result;
        }
        #endregion

        #region 根据id获取排班信息
        /// <summary>
        ///根据id获取排班信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ShiftEditGet(string id)
        {
            var d = new Dictionary<string, string>();
            d.Add("id", id);
            var result = await WebAPIHelper.Get("/api/Shift/GetByID", d);
            return result;
        }
        #endregion

        #region 新增排班信息
        /// <summary>
        /// 新增排班信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ShiftAdd(ShiftAdd shiftAdd)
        {
            shiftAdd.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Shift/Add", shiftAdd);
            return result;
        }
        #endregion

        #region 更新排班
        /// <summary>
        /// 更新排班
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ShiftSubmit(ShiftUpdate shiftUpdate)
        {
            shiftUpdate.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Shift/Update", shiftUpdate);
            return result;
        }
        #endregion

        #region 删除排班信息
        /// <summary>
        /// 删除排班信息
        /// </summary>
        /// <param name="smartUnitDelete"></param        /// <returns></returns>
        [HttpPost]
        public async Task<string> ShiftDelete(ShiftDelete shiftDelete)
        {
            shiftDelete.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Shift/Delete", shiftDelete);
            return result;
        }
        #endregion
    }
}