using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    public interface ICashierService
    {
        /// <summary>
        /// 待收费列表
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<NoPaidOrders>>> GetNoPaidOrders(long hospitalID);

        /// <summary>
        /// 预收款收银
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> DepositOrderCashier(DepositCashierAdd dto);
        /// <summary>
        /// 获取可退代金券跟预收款
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, CanCashier>> GetCanCashier(long hospitalID, long customerID, long orderID);

        /// <summary>
        /// 订单收银
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> OrderCashier(OrderCashierAdd dto);

        /// <summary>
        /// 退款收银
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> DepositRebateOrderCashier(DepositRebateCashierAdd dto);

        /// <summary>
        /// 退项目单
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> BackOrderCashier(BackCashierAdd dto);

        /// <summary>
        /// 欠款收银
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> DebtCashier(DebtCashierAdd dto);

        /// <summary>
        /// 获取更新收银详细信息
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <param name="cashierID"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, CashierUpdateInfo>> GetCashierUpdateInfo(long cashierID);

        /// <summary>
        /// 订单修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> CashierUpdate(CashierUpdate dto);

        /// <summary>
        /// 今日收银记录
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<Cashier>>> GetCashierToday(long hospitalID);

        /// <summary>
        /// 收银记录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<Cashier>>>> GetCashier(CashierSelect dto);

        /// <summary>
        /// 打印
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, string>> Print(long ID);
    }
}
