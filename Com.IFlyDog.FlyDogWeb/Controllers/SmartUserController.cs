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
    public class SmartUserController : Controller
    {
        // GET: SmartUser
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 公共选择用户页面
        /// </summary>
        /// <returns></returns>
        public ActionResult UserInfo()
        {
            return View();
        }

        /// <summary>
        /// 查询所有用户信息s(不分页)(当前医院的)
        /// </summary>
        /// <returns></returns>
        public async Task<string> SmartUserGet(SmartUserSelect dto)
        {
            if (!string.IsNullOrWhiteSpace(dto.HospitalID.ToString())&&dto.HospitalID!=0)
            {
                dto.HospitalID = dto.HospitalID;
            }
            else {
                dto.HospitalID = IDHelper.GetHospitalID();
            }
           
            dto.Status = CommonStatus.Use;
            var result = await WebAPIHelper.Post("/api/SmartUser/Get",dto);
            return result;
        }

        /// <summary>
        /// 查询到当前医院所有参与排班的用户
        /// </summary>
        /// <returns></returns>
        public async Task<string> SmartCYPBUserGet(SmartUserSelect dto)
        {

            dto.HospitalID = IDHelper.GetHospitalID();//所属医院先写死
            dto.Status = CommonStatus.Use;
            var result = await WebAPIHelper.Post("/api/SmartUser/GetCYPBUsers", dto);
            return result;
        }

        /// <summary>
        /// 查询所有用户信息s(分页)(查询所有医院的)
        /// </summary>
        /// <returns></returns>
        public async Task<string> SmartUserGetPage(SmartUserSelect dto)
        {
            dto.Status = CommonStatus.Use;
            var result = await WebAPIHelper.Post("/api/SmartUser/GetPages", dto);
            return result;
        }
    }
}