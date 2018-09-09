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
    /// 退预收款相关接口
    /// </summary>
    public class DepositRebateOrderController : ApiController
    {
        private IDepositRebateOrderService _depositRebateOrderService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="depositRebateOrderService"></param>
        public DepositRebateOrderController(IDepositRebateOrderService depositRebateOrderService)
        {
            _depositRebateOrderService = depositRebateOrderService;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Add(DepositRebateOrderAdd dto)
        {
            return await _depositRebateOrderService.Add(dto);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Delete(DepositOrderDelete dto)
        {
            return await _depositRebateOrderService.Delete(dto);
        }

        /// <summary>
        /// 查询详细
        /// </summary>
        /// <param name="userID">操作用户ID</param>
        /// <param name="customerID">顾客ID</param>
        /// <param name="orderID">订单ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, DepositRebateOrder>> GetDetail(long userID, long customerID, long orderID)
        {
            return await _depositRebateOrderService.GetDetail(userID, customerID, orderID);
        }

        /// <summary>
        /// 获取可退代金券跟预收款
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, CanRebate>> GetCanRebate(long hospitalID, long customerID)
        {
            return await _depositRebateOrderService.GetCanRebate(hospitalID, customerID);
        }
    }
}
