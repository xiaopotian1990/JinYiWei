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
    public class OwnerShipOrderController : Controller
    {
        /// <summary>
        /// 变更页面
        /// </summary>
        /// <returns></returns>
        // GET: OwnerShipOrder
        public ActionResult OwnerShipOrderInfo()
        {
            return View();
        }

        #region 咨询/开发人员变更申请 加载
        /// <summary>
        ///咨询/开发人员变更申请 加载
        /// </summary>
        /// <param name="smartUnitInfo">根据单位信息id查询单位详情</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> GetCustomerUserInfoGet(CustomerUserSelect dto)
        {
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/OwnerShipOrder/GetCustomerUserInfo", dto);
            return result;
        }
        #endregion

        #region 添加/编辑咨询人员变更申请
        /// <summary>
        /// 添加/编辑咨询人员变更申请
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CustomerConsultanAdd(CustomerConsultanAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            dto.Type = 2;
            var result = await WebAPIHelper.Post("/api/OwnerShipOrder/CustomerConsultanAdd", dto);
            return result;
        }
        #endregion

        #region  添加/ 编辑开发人员变更申请
        /// <summary>
        ///  添加/ 编辑开发人员变更申请
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CustomerDeveloperAdd(CustomerDeveloperAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            dto.Type = 1;
            var result = await WebAPIHelper.Post("/api/OwnerShipOrder/CustomerDeveloperAdd", dto);
            return result;
        }
        #endregion
    }
}