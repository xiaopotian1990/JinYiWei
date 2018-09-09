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
    /// 卷活动接口
    /// </summary>
   public interface ICouponActivityService
    {
        /// <summary>
        /// 查询所有卷活动
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<CouponActivityInfo>>> Get(CouponActivitySelect dto);

        /// <summary>
        /// 添加卷活动
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(CouponActivityAdd dto);

        /// <summary>
        /// 修改卷活动
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(CouponActivityUpdate dto);

        /// <summary>
        /// 根据ID获取卷活动详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, CouponActivityInfo> GetByID(long id);

        /// <summary>
        /// 删除卷活动
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> CouponActivityDelete(CouponActivityDelete dto);
    }
}
