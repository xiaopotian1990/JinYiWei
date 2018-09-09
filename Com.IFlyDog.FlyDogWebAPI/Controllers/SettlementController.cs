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
    /// 结算相关接口
    /// </summary>
    public class SettlementController : ApiController
    {
        private ISettlementService _settlement;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="settlement"></param>
        public SettlementController(ISettlementService settlement)
        {
            _settlement = settlement;
        }

        /// <summary>
        /// 结算时查询出的收银信息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, CashierOfUserInfo>> GetCashier(long userID)
        {
            return await _settlement.GetCashier(userID);
        }

        /// <summary>
        /// 结算
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> AddSettlement(SettlementAdd dto)
        {
            return await _settlement.AddSettlement(dto);
        }

        /// <summary>
        /// 结算记录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<Settlement>>>> Get(SettlementSelect dto)
        {
            return await _settlement.Get(dto);
        }
    }
}
