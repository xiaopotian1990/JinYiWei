using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    public interface IDepositService
    {
        /// <summary>
        /// 添加预收款
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> AddOrder(DepositOrderAdd dto);

        /// <summary>
        /// 预收款删除
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> Delete(DepositOrderDelete dto);

        /// <summary>
        /// 查询详细
        /// </summary>
        /// <param name="userID">操作用户ID</param>
        /// <param name="customerID">顾客ID</param>
        /// <param name="orderID">订单ID</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, DepositOrder>> GetDetail(long orderID);

        /// <summary>
        /// 查询剩余预收款
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <param name="customerID"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<NoDoneDeposits>>> GetNoDoneOrders(long hospitalID, long customerID);

        /// <summary>
        /// 添加预收款界面获取可购买的预收款
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<DepositChargeHospitalUse>>> GetAllDeposit(long hospitalID);
    }
}
