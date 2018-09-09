using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    public interface ICouponService
    {
        /// <summary>
        /// 查询剩余代金券
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<NoDoneCoupons>>> GetNoDoneOrders(long hospitalID, long customerID);

        /// <summary>
        /// 手工赠券
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> SendCoupon(SendCoupon dto);

        /// <summary>
        /// 手动扣减券
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> DeductCoupon(SendCoupon dto);
    }
}
