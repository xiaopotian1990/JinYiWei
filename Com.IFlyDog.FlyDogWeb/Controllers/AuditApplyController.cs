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
    /// 我的审核申请
    /// </summary>
    public class AuditApplyController : Controller
    {
        /// <summary>
        /// 我的审核申请页面
        /// </summary>
        /// <returns></returns>
        // GET: AuditApply
        public ActionResult AuditApplyInfo()
        {
            return View();
        }

        #region  查询当前用户所有的审核申请
        public async Task<string> AuditApplyGet(AuditApplySelect dto)
        {
            dto.CreateUserId = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/AuditApply/Get",
            dto);
            return result;
        }
        #endregion

        /// <summary>
        ///     根据操作id，类型查询审核申请详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> GetAuditDetail(string orderID,string type)
        {
            var d = new Dictionary<string, string>();
            d.Add("orderID", orderID.ToString());
            d.Add("type", type=="1"?"4":"5");//如果等于1 说明是开发人员变更 否则是咨询人员变更
            d.Add("hospitalID",IDHelper.GetHospitalID().ToString());
            d.Add("userID",IDHelper.GetUserID().ToString());
            var result = await WebAPIHelper.Get("/api/AuditApply/GetAuditDetail", d);
            return result;
        }

        #region 取消我的审核申请
        /// <summary>
        /// 取消我的审核申请
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> AuditApplyDelete(AuditApplyDelete dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/AuditApply/Delete", dto);
            return result;
        }
        #endregion
    }
}