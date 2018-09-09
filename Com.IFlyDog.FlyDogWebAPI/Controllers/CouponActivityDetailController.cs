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
    /// 卷活动详情
    /// </summary>
    public class CouponActivityDetailController : ApiController
    {
        private ICouponActivityDetailService _couponActivityDetailService;

        #region 依赖注入类
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="couponActivityDetailService"></param>
        public CouponActivityDetailController(ICouponActivityDetailService couponActivityDetailService)
        {
            _couponActivityDetailService = couponActivityDetailService;
        }
        #endregion

        /// <summary>
        /// 添加卷活动详情
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add(CouponActivityDetailAdd dto)
        {
            return _couponActivityDetailService.Add(dto);
        }

        /// <summary>
        /// 查询所有卷活动详情
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<CouponActivityDetailInfo>>> Get(CouponActivityDetailSelect dto)
        {
            return _couponActivityDetailService.Get(dto);
        }

        /// <summary>
        /// 单个删除卷活动详情
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> CouponActivityDetailDelete(CouponActivityDetailDelete dto)
        {
            return _couponActivityDetailService.CouponActivityDetailDelete(dto);
        }

        /// <summary>
        /// 删除全部未激活的卷活动详情
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> CouponActivityDetailDeleteAll(CouponActivityDetailDelete dto)
        {
            return _couponActivityDetailService.CouponActivityDetailDeleteAll(dto);
        }
    }
}
