using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    public interface ISettlementService
    {
        /// <summary>
        /// 结算时查询出的收银信息
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, CashierOfUserInfo>> GetCashier(long userID);

        /// <summary>
        /// 结算
        /// </summary>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> AddSettlement(SettlementAdd dto);

        /// <summary>
        /// 结算记录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<Settlement>>>> Get(SettlementSelect dto);
    }
}
