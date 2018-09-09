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
    /// 预收款相关接口
    /// </summary>
    public class DepositController : ApiController
    {
        private IDepositService _depositService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="depositService"></param>
        public DepositController(IDepositService depositService)
        {
            _depositService = depositService;
        }

        /// <summary>
        /// 添加预收款
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> AddOrder(DepositOrderAdd dto)
        {
            return await _depositService.AddOrder(dto);
        }

        /// <summary>
        /// 预收款删除
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Delete(DepositOrderDelete dto)
        {
            return await _depositService.Delete(dto);
        }

        /// <summary>
        /// 查询详细
        /// </summary>
        /// <param name="orderID">订单ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, DepositOrder>> GetDetail(long orderID)
        {
            return await _depositService.GetDetail(orderID);
        }

        /// <summary>
        /// 查询剩余预收款
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<NoDoneDeposits>>> GetNoDoneOrders(long hospitalID, long customerID)
        {
            return await _depositService.GetNoDoneOrders(hospitalID, customerID);
        }

        /// <summary>
        /// 添加预收款界面获取可购买的预收款
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<DepositChargeHospitalUse>>> GetAllDeposit(long hospitalID)
        {
            return await _depositService.GetAllDeposit(hospitalID);
        }
    }
}
