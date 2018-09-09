using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.IFlyDog.FlyDogWeb.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Com.IFlyDog.FlyDogWeb.Controllers
{
    public class PointController : Controller
    {
        /// <summary>
        /// 获取顾客积分信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> GetPointInfo(long customerID)
        {
            var d = new Dictionary<string, string>();
            d.Add("customerID", customerID.ToString());
            var result = await WebAPIHelper.Get("/api/Point/GetPointInfo", d);
            return result;
        }

        /// <summary>
        /// 手动增加扣减积分
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> DeductPoint(DeductPoint dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            dto.Type = PointType.ManualRebate;
            var result = await WebAPIHelper.Post("/api/Point/DeductPoint", dto);
            return result;
        }

        /// <summary>
        /// 手动增加积分
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> AddPoint(DeductPoint dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            dto.Type = PointType.ManualGive;
            var result = await WebAPIHelper.Post("/api/Point/DeductPoint", dto);
            return result;
        }

        /// <summary>
        /// 积分兑换券
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> PointToCoupon(PointToCoupon dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/Point/PointToCoupon", dto);
            return result;
        }
    }
}