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
    /// 添加卷活动api
    /// </summary>
    public class CouponActivityServiceController : ApiController
    {
        private ICouponActivityService _couponActivityService;

        #region 依赖注入类
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="couponActivityService"></param>
        public CouponActivityServiceController(ICouponActivityService couponActivityService)
        {
            _couponActivityService = couponActivityService;
        }
        #endregion

        /// <summary>
        /// 添加卷活动
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add(CouponActivityAdd dto)
        {
            return _couponActivityService.Add(dto);
        }
        /// <summary>
        /// 查询所有卷活动
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<CouponActivityInfo>>> Get(CouponActivitySelect dto)
        {
            return _couponActivityService.Get(dto);
        }

        /// <summary>
        /// 根据id获取卷活动详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, CouponActivityInfo> GetByID(long id)
        {
            return _couponActivityService.GetByID(id);
        }

        /// <summary>
        /// 删除卷活动，以及卷活动详情
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> CouponActivityDelete(CouponActivityDelete dto)
        {
            return _couponActivityService.CouponActivityDelete(dto);
        }

        /// <summary>
        /// 更新卷活动
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update(CouponActivityUpdate dto)
        {
            return _couponActivityService.Update(dto);
        }



    }
}
