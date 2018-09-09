using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    public interface IFrontDeskService
    {
        /// <summary>
        /// 添加候诊
        /// </summary>
        /// <param name="dto">候诊信息</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> AddWaitAsync(WaitAdd dto);

        /// <summary>
        /// 获取今日候诊列表
        /// </summary>
        /// <param name="hospitalID">医院ID</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<Wait>>> GetWaitTodayAsync(long hospitalID);

        /// <summary>
        /// 分诊时查询出顾客粗略信息
        /// </summary>
        /// <param name="customerID">顾客ID</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, CustomerInfoBefaultTriage>> GetCustomerInfoBefaultTriageAsync(long customerID, long hospitalID);

        /// <summary>
        /// 分诊
        /// </summary>
        /// <param name="dto">分诊信息</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> AddTriageAsync(TriageAdd dto);

        /// <summary>
        /// 获取今日候诊列表
        /// </summary>
        /// <param name="hospitalID">医院ID</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<TriageToday>>> GetTriageTodayAsync(long hospitalID);

        /// <summary>
        /// 获取今日上门记录
        /// </summary>
        /// <param name="hospitalID">医院ID</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<VisitToday>>> GetVisitTodayAsync(long hospitalID);
    }
}
