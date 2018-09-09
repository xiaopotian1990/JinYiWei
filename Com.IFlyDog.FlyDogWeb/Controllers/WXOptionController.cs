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
    public class WXOptionController : Controller
    {
        /// <summary>
        /// 微信系统设置页面
        /// </summary>
        /// <returns></returns>
        // GET: WXOption
        public ActionResult WXOptionInfo()
        {
            return View();
        }

        #region 查询所有微信系统设置
        /// <summary>
        /// 查询所有微信系统设置
        /// </summary>
        /// <returns></returns>
        public async Task<string> WXOptionGet()
        {
            var result = await WebAPIHelper.Get("/api/WXOption/Get", new Dictionary<string, string>());
            return result;
        }
        #endregion

        #region 微信系统设置级别提点
        /// <summary>
        /// 微信系统设置级别提点
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> WXOptionInsertPromoteLevel(WXOptionUpdatePromoteLevel dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/WXOption/WXOptionInsertPromoteLevel", dto);
            return result;
        }
        #endregion


        #region 微信系统设置佣金提成
        /// <summary>
        /// 微信系统设置佣金提成
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> WXOptionOpenCommission(WXOptionOpenCommission dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/WXOption/WXOptionOpenCommission", dto);
            return result;
        }
        #endregion

        #region 微信系统设置修改不提点折扣小于
        /// <summary>
        /// 微信系统设置修改不提点折扣小于
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> WXOptionUpdateNoDiscount(WXOptionUpdateNoDiscount dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/WXOption/WXOptionUpdateNoDiscount", dto);
            return result;
        }
        #endregion

        #region 微信系统设置修改推荐时限
        /// <summary>
        /// 微信系统设置修改推荐时限
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> WXOptionUpdateRecommenDay(WXOptionUpdateRecommenDay dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/WXOption/WXOptionUpdateRecommenDay", dto);
            return result;
        }
        #endregion

        #region 更新被推荐用户送券
        /// <summary>
        /// 更新被推荐用户送券
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> WXOptionUpdateUserSendVolume(WXOptionUpdateUserSendVolume dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/WXOption/WXOptionUpdateUserSendVolume", dto);
            return result;
        }
        #endregion

        #region 微信系统设置默认渠道
        /// <summary>
        /// 微信系统设置默认渠道
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> WXOptionDefaultChannel(WXOptionDefaultChannel dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/WXOption/WXOptionDefaultChannel", dto);
            return result;
        }
        #endregion

        #region 微信系统设置特殊渠道
        /// <summary>
        /// 微信系统设置特殊渠道
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> WXOptionSpecialChannel(WXOptionSpecialChannel dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/WXOption/WXOptionSpecialChannel", dto);
            return result;
        }
        #endregion
    }
}