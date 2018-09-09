using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.IFlyDog.FlyDogWeb.Helper;

namespace Com.IFlyDog.FlyDogWeb.Controllers
{
    public class CouponController : Controller
    {
        // GET: Coupon
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 获取顾客券剩余信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> GetNoDoneCoupon(long customerID)
        {
            var dic = new Dictionary<string, string>
            {
                {"customerID", customerID.ToString()},
                {"hospitalID", IDHelper.GetHospitalID().ToString()}
            };
            var result = await WebAPIHelper.Get("/api/Coupon/GetNoDoneOrders", dic);
            return result;
        }
      
        /// <summary>
        /// 手动增加券
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SendCoupon(SendCoupon dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();         
            var result = await WebAPIHelper.Post("/api/Coupon/SendCoupon", dto);
            return result;
        }
        /// <summary>
        /// 手动扣减券
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> DeductCoupon(SendCoupon dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID(); 
            var result = await WebAPIHelper.Post("/api/Coupon/DeductPoint", dto);
            return result;
        }
    }
}