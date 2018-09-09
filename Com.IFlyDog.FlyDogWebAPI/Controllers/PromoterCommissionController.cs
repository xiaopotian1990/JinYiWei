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
    /// 推荐佣金相关接口
    /// </summary>
    public class PromoterCommissionController : ApiController
    {
        private IPromoterCommissionService _promoterCommissionService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="promoterCommissionService"></param>
        public PromoterCommissionController(IPromoterCommissionService promoterCommissionService)
        {
            _promoterCommissionService = promoterCommissionService;
        }

        /// <summary>
        /// 提现
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> CommissionOut(CommissionOut dto)
        {
            return await _promoterCommissionService.CommissionOut(dto);
        }
    }
}
