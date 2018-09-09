using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    public interface IPointService
    {
        /// <summary>
        /// 获取顾客积分信息
        /// </summary>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, CustomerPointInfo>> GetPointInfo(long customerID);

        /// <summary>
        /// 手动增加扣减积分
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> DeductPoint(DeductPoint dto);

        /// <summary>
        /// 积分兑换券
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> PointToCoupon(PointToCoupon dto);
    }
}
