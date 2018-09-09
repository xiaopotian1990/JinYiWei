using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.WebAPI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Com.IFlyDog.FlyDogWebAPI.Controllers
{
    /// <summary>
    /// 微信系统设置api
    /// </summary>
    public class WXOptionController : ApiController
    {
        private IWXOptionService _wXOptionService;

        #region 依赖注入类
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="wXOptionService"></param>
        public WXOptionController(IWXOptionService wXOptionService)
        {
            _wXOptionService = wXOptionService;
        }
        #endregion


        /// <summary>
        /// 微信系统设置级别提点
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> WXOptionInsertPromoteLevel(WXOptionUpdatePromoteLevel dto)
        {
            return _wXOptionService.WXOptionInsertPromoteLevel(dto);
        }


        /// <summary>
        /// 微信系统设置佣金提成
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> WXOptionOpenCommission(WXOptionOpenCommission dto)
        {
            return _wXOptionService.WXOptionOpenCommission(dto);
        }

        /// <summary>
        /// 微信系统设置修改不提点折扣小于
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> WXOptionUpdateNoDiscount(WXOptionUpdateNoDiscount dto)
        {
            return _wXOptionService.WXOptionUpdateNoDiscount(dto);
        }

        /// <summary>
        /// 微信系统设置修改推荐时限
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> WXOptionUpdateRecommenDay(WXOptionUpdateRecommenDay dto)
        {
            return _wXOptionService.WXOptionUpdateRecommenDay(dto);
        }

        /// <summary>
        /// 更新被推荐用户送券
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> WXOptionUpdateUserSendVolume(WXOptionUpdateUserSendVolume dto)
        {
            return _wXOptionService.WXOptionUpdateUserSendVolume(dto);
        }

        #region 获取微信系统所有设置
        /// <summary>
        /// 获取微信系统所有设置[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, WXOptionInfo> Get()
        {
            return _wXOptionService.Get();
        }
        #endregion

        /// <summary>
        /// 微信系统设置默认渠道
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> WXOptionDefaultChannel(WXOptionDefaultChannel dto)
        {
            return _wXOptionService.WXOptionDefaultChannel(dto);
        }

        /// <summary>
        /// 微信系统设置特殊渠道
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> WXOptionSpecialChannel(WXOptionSpecialChannel dto)
        {
            return _wXOptionService.WXOptionSpecialChannel(dto);
        }
    }
}
