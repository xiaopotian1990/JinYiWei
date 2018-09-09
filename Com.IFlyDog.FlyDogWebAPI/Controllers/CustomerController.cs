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
    /// 顾客相关接口
    /// </summary>
    public class CustomerController : ApiController
    {
        /// <summary>
        /// 依赖注入
        /// </summary>
        private ICustomerService _customerService;

        /// <summary>
        /// 依赖注入方法重构
        /// </summary>
        /// <param name="customerService"></param>
        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// 添加顾客
        /// </summary>
        /// <param name="dto">顾客信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, long>> AddAsync(CustomerAdd dto)
        {
            return await _customerService.AddAsync(dto);
        }

        /// <summary>
        /// 顾客识别
        /// </summary>
        /// <param name="name">各种识别码</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<CustomerIdentifyInfo>>> CustomerIdentifyAsync(string name)
        {
            return await _customerService.CustomerIdentifyAsync(name);
        }

        /// <summary>
        /// 查询今日登记顾客
        /// </summary>
        /// <param name="hospitalID">医院ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<CustomerCreateToday>>> CustomerCreateTodayAsync(long hospitalID, CustomerRegisterType type)
        {
            return await _customerService.CustomerCreateTodayAsync(hospitalID, type);
        }

        /// <summary>
        /// 客户管理查询
        /// </summary>
        /// <param name="dto">筛选条件</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<CustomerManager>>>> GetCustomerManager(CustomerSelect dto)
        {
            return await _customerService.GetCustomerManager(dto);
        }
    }
}
