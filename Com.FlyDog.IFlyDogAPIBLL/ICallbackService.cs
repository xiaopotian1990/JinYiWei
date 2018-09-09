using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    public interface ICallbackService
    {
        /// <summary>
        /// 客户档案里面添加回访
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> CallbackAdd(CallbackAdd dto);

        /// <summary>
        /// 回访工作台回访
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> CallbackAddByDesk(CallbackAddByDesk dto);

        /// <summary>
        /// 客户档案里面添加回访提醒
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> CallbackRemindAdd(CallbackRemindAdd dto);

        /// <summary>
        /// 客户档案里面添加回访计划
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> CallbackPlanAdd(CallbackPlanAdd dto);

        /// <summary>
        /// 回访工作台查询
        /// </summary>
        /// <param name="dto">查询条件</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<Callback>>>> CallbackSelect(CallbackSelect dto);

        /// <summary>
        /// 修改回访
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> UpdateCallback(CallbackUpdate dto);

        /// <summary>
        /// 修改回访提醒
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> UpdateCallbackRemind(CallbackRemindUpdate dto);

        /// <summary>
        /// 获取回放详细，在回访工作台点击回访查询出来
        /// </summary>
        /// <param name="ID">回访记录ID</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, CallbackDetail>> GetCallbackDetail(long ID);

        /// <summary>
        /// 个人回访情况
        /// </summary>
        /// <param name="dto">查询条件</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<Callback>>> GetCallbackByCustomerID(long customerID);

        /// <summary>
        /// 获取可使用的回访计划
        /// </summary>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<CallbackSet>>> GetCallbackSet();

        /// <summary>
        /// 获取回访计划详细内容
        /// </summary>
        /// <param name="setID">回访计划ID</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<CallbackSetDetail>>> GetCallbackSetDetail(long setID);

        /// <summary>
        /// 更新回访提醒之前获取回访提醒详细
        /// </summary>
        /// <param name="ID">回访记录ID</param>
        /// <param name="userID">回访人</param>
        /// <param name="customerID">顾客ID</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, CallbackRemindDetail>> GetCallbackRemindDetail(long ID, long userID, long customerID);

        /// <summary>
        /// 更新回访之前获取回访详细
        /// </summary>
        /// <param name="ID">回访记录ID</param>
        /// <param name="userID">回访人</param>
        /// <param name="customerID">顾客ID</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, CallbackUpdateDetail>> GetCallbackUpdateDetail(long ID, long userID, long customerID);
    }
}
