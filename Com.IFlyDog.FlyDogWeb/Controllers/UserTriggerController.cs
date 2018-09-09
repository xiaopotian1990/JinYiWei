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
    /// 用户通知
    /// </summary>
    public class UserTriggerController : Controller
    {
        // GET: UserTrigger
        public ActionResult UserTriggerInfo()
        {
            return View();
        }

        #region 查询所有用户通知
        /// <summary>
        /// 查询所有用户通知
        /// </summary>
        /// <returns></returns>
        public async Task<string> UserTriggerGetData()
        {
            var result = await WebAPIHelper.Get("/api/UserTrigger/Get", new Dictionary<string, string>());
            return result;
        }
        #endregion


        #region 根据id查询详情
        /// <summary>
        ///根据id查询详情
        /// </summary>
        /// <param name="id">根据id查询详情</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> UserTriggerGet(string id)
        {
            var d = new Dictionary<string, string>();
            d.Add("id", id);
            var result = await WebAPIHelper.Get("/api/UserTrigger/GetByID", d);
            return result;
        }
        #endregion

        #region 新增用户通知
        /// <summary>
        /// 新增用户通知
        /// </summary>
        /// <param name="userTriggerAdd"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> UserTriggerAdd(UserTriggerAdd userTriggerAdd)
        {
            userTriggerAdd.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/UserTrigger/Add", userTriggerAdd);
            return result;
        }
        #endregion

        #region 更新用户通知
        /// <summary>
        /// 更新用户通知
        /// </summary>
        /// <param name="smartUnitUpdate"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> UserTriggerSubmit(UserTriggerUpdate userTriggerUpdate)
        {
            userTriggerUpdate.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/UserTrigger/Update", userTriggerUpdate);
            return result;
        }
        #endregion

        #region 删除用户通知
        /// <summary>
        /// 删除用户通知
        /// </summary>
        /// <param name="userTriggerDelete"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> UserTriggerDelete(UserTriggerDelete userTriggerDelete)
        {
            userTriggerDelete.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/UserTrigger/Delete", userTriggerDelete);
            return result;
        }
        #endregion
    }
}