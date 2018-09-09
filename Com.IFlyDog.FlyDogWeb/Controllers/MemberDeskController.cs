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
    /// <summary>
    /// 会员工作台
    /// </summary>
    public class MemberDeskController : Controller
    {
        // GET: MemberDesk
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 查询会员
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> Get(MemberDeskCustomerSelect dto)
        {
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/MemberDesk/Get", dto);
            return result;
        }

        /// <summary>
        /// 获取最近七日生日顾客
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> GetBirthday()
        {
            var d = new Dictionary<string, string>();
            d.Add("hospitalID", IDHelper.GetHospitalID().ToString());
            var result = await WebAPIHelper.Get("/api/MemberDesk/GetBirthday", d);
            return result;
        }
    }
}