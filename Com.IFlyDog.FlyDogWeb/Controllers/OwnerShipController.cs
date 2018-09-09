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
    public class OwnerShipController : Controller
    {
        /// <summary>
        /// 客户所属权页面
        /// </summary>
        /// <returns></returns>
        // GET: OwnerShip
        public ActionResult OwnerShipInfo()
        {
            return View();
        }

        #region 查询当前医院客户归属权管理
        /// <summary>
        /// 查询当前医院客户归属权管理
        /// </summary>
        /// <returns></returns>
        public async Task<string> OwnerShipGet()
        {
            var d = new Dictionary<string, string>();
            d.Add("hospitalID", IDHelper.GetHospitalID().ToString());
            var result = await WebAPIHelper.Get("/api/OwnerShip/Get", d);
            return result;
        }
        #endregion

        #region 批量设置咨询人员归属权
        /// <summary>
        /// 批量设置咨询人员归属权
        /// </summary>
        /// <param name="smartUnitAdd"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> BatchConsultantUserAdd(BatchConsultantUser dto)
         {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/OwnerShip/BatchConsultantUserAdd", dto);
            return result;
        }
        #endregion

        #region 批量设置开发人员归属权
        /// <summary>
        /// 批量设置开发人员归属权
        /// </summary>
        /// <param name="smartUnitAdd"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> BatchDeveloperUserAdd(BatchDeveloperUser dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/OwnerShip/BatchDeveloperUserAdd", dto);
            return result;
        }
        #endregion

        #region 单个添加咨询人员客户归属权
        /// <summary>
        /// 单个添加咨询人员客户归属权
        /// </summary>
        /// <param name="smartUnitAdd"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SingleConsultantUserUpdateAdd(SingleConsultantUserUpdate dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/OwnerShip/SingleConsultantUserUpdateAdd", dto);
            return result;
        }
        #endregion

        #region 单个添加开发人员客户归属权
        /// <summary>
        /// 单个添加开发人员客户归属权
        /// </summary>
        /// <param name="smartUnitAdd"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SingleDeveLoperUserUpdateAdd(SingleDeveLoperUserUpdate dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/OwnerShip/SingleDeveLoperUserUpdateAdd", dto);
            return result;
        }
        #endregion
    }
}