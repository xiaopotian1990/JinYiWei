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
    /// 收银相关API
    /// </summary>
    public class CashierController : ApiController
    {
        private ICashierService _cashierService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cashierService"></param>
        public CashierController(ICashierService cashierService)
        {
            _cashierService = cashierService;
        }

        /// <summary>
        /// 待收费列表
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<NoPaidOrders>>> GetNoPaidOrders(long hospitalID)
        {
            return await _cashierService.GetNoPaidOrders(hospitalID);
        }

        /// <summary>
        /// 预收款收银
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> DepositOrderCashier(DepositCashierAdd dto)
        {
            return await _cashierService.DepositOrderCashier(dto);
        }

        /// <summary>
        /// 获取订单收银时可使用的券跟预收款
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <param name="customerID"></param>
        /// <param name="orderID"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, CanCashier>> GetCanCashier(long hospitalID, long customerID, long orderID)
        {
            return await _cashierService.GetCanCashier(hospitalID, customerID, orderID);
        }

        /// <summary>
        /// 订单收银
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> OrderCashier(OrderCashierAdd dto)
        {
            return await _cashierService.OrderCashier(dto);
        }

        /// <summary>
        /// 退款收银
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> DepositRebateOrderCashier(DepositRebateCashierAdd dto)
        {
            return await _cashierService.DepositRebateOrderCashier(dto);
        }

        /// <summary>
        /// 退项目单
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> BackOrderCashier(BackCashierAdd dto)
        {
            return await _cashierService.BackOrderCashier(dto);
        }

        /// <summary>
        /// 欠款收银
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> DebtCashier(DebtCashierAdd dto)
        {
            return await _cashierService.DebtCashier(dto);
        }

        /// <summary>
        /// 获取更新收银详细信息
        /// </summary>
        /// <param name="cashierID"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, CashierUpdateInfo>> GetCashierUpdateInfo(long cashierID)
        {
            return await _cashierService.GetCashierUpdateInfo(cashierID);
        }

        /// <summary>
        /// 订单修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> CashierUpdate(CashierUpdate dto)
        {
            return await _cashierService.CashierUpdate(dto);
        }

        /// <summary>
        /// 今日收银记录
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<Cashier>>> GetCashierToday(long hospitalID)
        {
            return await _cashierService.GetCashierToday(hospitalID);
        }

        /// <summary>
        /// 收银记录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<Cashier>>>> GetCashier(CashierSelect dto)
        {
            return await _cashierService.GetCashier(dto);
        }

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, string>> Print(long ID)
        {
            return await _cashierService.Print(ID);
        }
    }
}
