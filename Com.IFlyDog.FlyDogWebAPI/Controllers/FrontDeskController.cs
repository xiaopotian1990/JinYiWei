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
    /// 前台接待相关接口
    /// </summary>
    public class FrontDeskController : ApiController
    {
        private IFrontDeskService _frontDeskService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="frontDeskService"></param>
        public FrontDeskController(IFrontDeskService frontDeskService)
        {
            _frontDeskService = frontDeskService;
        }

        /// <summary>
        /// 添加候诊
        /// </summary>
        /// <param name="dto">候诊信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> AddWaitAsync(WaitAdd dto)
        {
            return await _frontDeskService.AddWaitAsync(dto);
        }

        /// <summary>
        /// 获取今日候诊列表
        /// </summary>
        /// <param name="hospitalID">医院ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<Wait>>> GetWaitTodayAsync(long hospitalID)
        {
            return await _frontDeskService.GetWaitTodayAsync(hospitalID);
        }

        /// <summary>
        /// 分诊时查询出顾客粗略信息
        /// </summary>
        /// <param name="customerID">顾客ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, CustomerInfoBefaultTriage>> GetCustomerInfoBefaultTriageAsync(long customerID, long hospitalID)
        {
            return await _frontDeskService.GetCustomerInfoBefaultTriageAsync(customerID, hospitalID);
        }

        /// <summary>
        /// 分诊
        /// </summary>
        /// <param name="dto">分诊信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> AddTriageAsync(TriageAdd dto)
        {
            return await _frontDeskService.AddTriageAsync(dto);
        }

        /// <summary>
        /// 获取今日候诊列表
        /// </summary>
        /// <param name="hospitalID">医院ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<TriageToday>>> GetTriageTodayAsync(long hospitalID)
        {
            return await _frontDeskService.GetTriageTodayAsync(hospitalID);
        }

        /// <summary>
        /// 获取今日上门记录
        /// </summary>
        /// <param name="hospitalID">医院ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<VisitToday>>> GetVisitTodayAsync(long hospitalID)
        {
            return await _frontDeskService.GetVisitTodayAsync(hospitalID);
        }
    }
}
