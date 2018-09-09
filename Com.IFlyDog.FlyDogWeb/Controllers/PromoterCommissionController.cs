using Com.IFlyDog.APIDTO;
using Com.IFlyDog.FlyDogWeb.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Com.IFlyDog.FlyDogWeb.Controllers
{
    public class PromoterCommissionController : Controller
    {
        /// <summary>
        /// 佣金提现
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CommissionOut(CommissionOut dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/PromoterCommission/CommissionOut", dto);
            return result;
        }
    }
}