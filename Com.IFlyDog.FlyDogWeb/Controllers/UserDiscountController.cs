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
    /// 用户折扣管理控制器
    /// </summary>
    public class UserDiscountController : Controller
    {
        /// <summary>
        /// 折扣管理页
        /// </summary>
        /// <returns></returns>
        // GET: UserDiscount
        public ActionResult UserDiscountInfo()
        {
            return View();
        }

        #region 查询所有用户折扣信息
        /// <summary>
        /// 查询所有用户折扣信息
        /// </summary>
        /// <returns></returns>
        public async Task<string> UserDiscountGet()
        {
            var result = await WebAPIHelper.Get("/api/UserDiscount/Get", new Dictionary<string, string>());
            return result;
        }
        #endregion

        #region 查询所有用户折扣信息（分页）
        /// <summary>
        /// 查询所有用户折扣信息
        /// </summary>
        /// <returns></returns>
        public async Task<string> UserDiscountGetPage(UserDiscountSelect dto)
        {
            var result = await WebAPIHelper.Post("/api/UserDiscount/GetPage",
            dto);
            return result;
        }
        #endregion

        #region 根据id查询用户折扣详情
        /// <summary>
        ///根据id查询用户折扣详情
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> UserDiscountEditGet(string id)
        {
            var d = new Dictionary<string, string>();
            d.Add("id", id);
            var result = await WebAPIHelper.Get("/api/UserDiscount/GetByID", d);
            return result;
        }
        #endregion

        #region 新增用户折扣信息
        /// <summary>
        /// 新增用户折扣信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> UserDiscountAdd(UserDiscountAdd userDiscountAdd)
        {
            userDiscountAdd.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/UserDiscount/Add", userDiscountAdd);
            return result;
        }
        #endregion

        #region 更新用户折扣信息
        /// <summary>
        /// 更新用户折扣信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> UserDiscountSubmit(UserDiscountUpdate userDiscountUpdate)
        {
            userDiscountUpdate.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/UserDiscount/Update", userDiscountUpdate);
            return result;
        }
        #endregion
    }
}