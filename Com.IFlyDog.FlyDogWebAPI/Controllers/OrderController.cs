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
    /// 订单相关接口
    /// </summary>
    public class OrderController : ApiController
    {
        private IOrderService _orderService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="orderService"></param>
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        /// <summary>
        /// 查询套餐
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pym"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<ChargeSet>>> GetChargeSet(string name = null, string pym = null)
        {
            return await _orderService.GetChargeSet(name, pym);
        }

        /// <summary>
        /// 添加订单
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Add(OrderAdd dto)
        {
            return await _orderService.Add(dto);
        }

        /// <summary>
        /// 订单删除
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Delete(DepositOrderDelete dto)
        {
            return await _orderService.Delete(dto);
        }

        /// <summary>
        /// 添加订单
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Update(OrderAdd dto)
        {
            return await _orderService.Update(dto);
        }

        /// <summary>
        /// 预约界面获取已购买项目
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<AppointCharges>>> GetAppointCharges(long customerID)
        {
            return await _orderService.GetAppointCharges(customerID);
        }

        /// <summary>
        /// 获取未完成项目
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<NoDoneOrders>>> GetNoDoneOrders(long hospitalID, long customerID)
        {
            return await _orderService.GetNoDoneOrders(hospitalID, customerID);
        }

        /// <summary>
        /// 查询详细
        /// </summary>
        /// <param name="customerID">顾客ID</param>
        /// <param name="orderID">订单ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, Order>> GetDetail(long customerID, long orderID)
        {
            return await _orderService.GetDetail(customerID, orderID);
        }

        /// <summary>
        /// 欠款订单
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<DebtOrders>>> GetDebtOrdes(DebtSelect dto)
        {
            return await _orderService.GetDebtOrdes(dto);
        }
    }
}
