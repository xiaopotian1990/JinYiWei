using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    public interface IOrderService
    {
        /// <summary>
        /// 查询套餐
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pym"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<ChargeSet>>> GetChargeSet(string name, string pym);

        /// <summary>
        /// 添加订单
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> Add(OrderAdd dto);

        /// <summary>
        /// 订单删除
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> Delete(DepositOrderDelete dto);
        /// <summary>
        /// 添加订单
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> Update(OrderAdd dto);

        /// <summary>
        /// 预约界面获取已购买项目
        /// </summary>
        /// <param name="customerID"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<AppointCharges>>> GetAppointCharges(long customerID);

        /// <summary>
        /// 获取未完成项目
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<NoDoneOrders>>> GetNoDoneOrders(long hospitalID, long customerID);

        /// <summary>
        /// 查询详细
        /// </summary>
        /// <param name="userID">操作用户ID</param>
        /// <param name="customerID">顾客ID</param>
        /// <param name="orderID">订单ID</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, Order>> GetDetail(long customerID, long orderID);

        /// <summary>
        /// 欠款订单
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<DebtOrders>>> GetDebtOrdes(DebtSelect dto);
    }
}
