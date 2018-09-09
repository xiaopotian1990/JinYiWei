using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    public interface IReceptionService
    {
        /// <summary>
        /// 获取今日上门记录
        /// </summary>
        /// <param name="hospitalID">医院ID</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, ReceptionTodayInfo>> GetReceptionTodayAsync(long hospitalID,long userID);
    }
}
