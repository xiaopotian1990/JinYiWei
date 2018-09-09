using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.WebAPI.Filters;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;

namespace Com.IFlyDog.FlyDogWebAPI.Controllers
{
    /// <summary>
    /// 回访相关接口
    /// </summary>
    public class CallbackController : ApiController
    {
        private ICallbackService _callbackService;

        /// <summary>
        /// 依赖注入
        /// </summary>
        /// <param name="callbackService"></param>
        public CallbackController(ICallbackService callbackService)
        {
            _callbackService = callbackService;
        }

        /// <summary>
        /// 客户档案里面添加回访
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> CallbackAdd(CallbackAdd dto)
        {
            return await _callbackService.CallbackAdd(dto);
        }

        /// <summary>
        /// 回访工作台回访
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> CallbackAddByDesk(CallbackAddByDesk dto)
        {
            return await _callbackService.CallbackAddByDesk(dto);
        }

        /// <summary>
        /// 客户档案里面添加回访提醒
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> CallbackRemindAdd(CallbackRemindAdd dto)
        {
            return await _callbackService.CallbackRemindAdd(dto);
        }

        /// <summary>
        /// 客户档案里面添加回访计划
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> CallbackPlanAdd(CallbackPlanAdd dto)
        {
            return await _callbackService.CallbackPlanAdd(dto);
        }

        /// <summary>
        /// 回访工作台查询
        /// </summary>
        /// <param name="dto">查询条件</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<Callback>>>> CallbackSelect(CallbackSelect dto)
        {
            return await _callbackService.CallbackSelect(dto);
        }

        /// <summary>
        /// 修改回访
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> UpdateCallback(CallbackUpdate dto)
        {
            return await _callbackService.UpdateCallback(dto);
        }

        /// <summary>
        /// 修改回访提醒
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> UpdateCallbackRemind(CallbackRemindUpdate dto)
        {
            return await _callbackService.UpdateCallbackRemind(dto);
        }

        /// <summary>
        /// 获取回放详细，在回访工作台点击回访查询出来
        /// </summary>
        /// <param name="ID">回访记录ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, CallbackDetail>> GetCallbackDetail(long ID)
        {
            return await _callbackService.GetCallbackDetail(ID);
        }

        /// <summary>
        /// 个人回访情况
        /// </summary>
        /// <param name="customerID">顾客ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<Callback>>> GetCallbackByCustomerID(long customerID)
        {
            return await _callbackService.GetCallbackByCustomerID(customerID);
        }

        /// <summary>
        /// 获取可使用的回访计划
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<CallbackSet>>> GetCallbackSet()
        {
            return await _callbackService.GetCallbackSet();
        }

        /// <summary>
        /// 获取回访计划详细内容
        /// </summary>
        /// <param name="setID">回访计划ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<CallbackSetDetail>>> GetCallbackSetDetail(long setID)
        {
            return await _callbackService.GetCallbackSetDetail(setID);
        }

        /// <summary>
        /// 更新回访提醒之前获取回访提醒详细
        /// </summary>
        /// <param name="ID">回访记录ID</param>
        /// <param name="userID">回访人</param>
        /// <param name="customerID">顾客ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, CallbackRemindDetail>> GetCallbackRemindDetail(long ID, long userID, long customerID)
        {
            return await _callbackService.GetCallbackRemindDetail(ID, userID, customerID);
        }

        /// <summary>
        /// 更新回访之前获取回访详细
        /// </summary>
        /// <param name="ID">回访记录ID</param>
        /// <param name="userID">回访人</param>
        /// <param name="customerID">顾客ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, CallbackUpdateDetail>> GetCallbackUpdateDetail(long ID, long userID, long customerID)
        {
            return await _callbackService.GetCallbackUpdateDetail(ID, userID, customerID);
        }
    }
}
