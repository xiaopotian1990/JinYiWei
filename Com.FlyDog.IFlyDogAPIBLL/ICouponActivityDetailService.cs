using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    /// <summary>
    /// 卷活动管理
    /// </summary>
   public interface ICouponActivityDetailService
    {
        /// <summary>
        /// 查询所有卷活动详细信息
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<CouponActivityDetailInfo>>> Get(CouponActivityDetailSelect dto);

        /// <summary>
        /// 根据卷活动管理id查询当前卷管理下所有的卷活动
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<CouponActivityDetailInfo>> GetByActivityID(long activityID);

        /// <summary>
        /// 添加卷活动
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(CouponActivityDetailAdd dto);

        /// <summary>
        /// 删除卷活动详情
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> CouponActivityDetailDelete(CouponActivityDetailDelete dto);

        /// <summary>
        /// 删除全部未激活的卷活动详情
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> CouponActivityDetailDeleteAll(CouponActivityDetailDelete dto);
    }
}
