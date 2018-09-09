using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.WebAPI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Com.IFlyDog.FlyDogWebAPI.Controllers
{
    /// <summary>
    /// 代金券相关接口
    /// </summary>
    public class CouponController : ApiController
    {
        private ICouponService _couponService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="couponService"></param>
        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }

        /// <summary>
        /// 查询剩余代金券
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<NoDoneCoupons>>> GetNoDoneOrders(long hospitalID, long customerID)
        {
            return await _couponService.GetNoDoneOrders(hospitalID, customerID);
        }

        /// <summary>
        /// 手工赠券
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> SendCoupon(SendCoupon dto)
        {
            return await _couponService.SendCoupon(dto);
        }

        /// <summary>
        /// 手动扣减券
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> DeductPoint(SendCoupon dto)
        {
            return await _couponService.DeductCoupon(dto);
        }
    }
}
