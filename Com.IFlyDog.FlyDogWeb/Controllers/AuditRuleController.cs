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
    /// 审核规则控制器
    /// </summary>
    public class AuditRuleController : Controller
    {

        /// <summary>
        /// 审核规则医院列表页
        /// </summary>
        /// <returns></returns>
        public ActionResult AuditRuleHospitalInfo() {
            return View();
        }

        /// <summary>
        /// 审核规则页面
        /// </summary>
        /// <returns></returns>
        // GET: AuditRule
        public ActionResult AuditRuleInfo(string hospitalID)
        {
            return View();
        }

        /// <summary>
        /// 审核规则详细页
        /// </summary>
        /// <returns></returns>
        // GET: AuditRule
        public ActionResult AuditRuleDetailInfo(string type,string auditRuleID)
        {
            return View();
        }


        #region 查询所有审核规则
        /// <summary>
        /// 查询所有审核规则
        /// </summary>
        /// <returns></returns>
        public async Task<string> AuditRuleGet(string hospitalID)
        {
            var d = new Dictionary<string, string>();
            d.Add("hospitalID", hospitalID);
            var result = await WebAPIHelper.Get("/api/AuditRule/Get", d);
            return result;
        }
        #endregion

        #region 根据id获取审核规则详情
        /// <summary>
        ///根据id获取审核规则详情
        /// </summary>
        /// <param name="smartUnitInfo">根据id获取审核规则详情</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> AuditRuleEditGet(string id)
        {
            var d = new Dictionary<string, string>();
            d.Add("id",id);
            var result = await WebAPIHelper.Get("/api/AuditRule/GetByID", d);
            return result;
        }
        #endregion

        #region 新增审核规则
        /// <summary>
        /// 新增审核规则
        /// </summary>
        /// <param name="auditRuleAdd"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> AuditRuleAdd(AuditRuleAdd auditRuleAdd)
        {
            auditRuleAdd.HospitalID = auditRuleAdd.HospitalID;
            auditRuleAdd.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/AuditRule/Add", auditRuleAdd);
            return result;
        }
        #endregion

        #region 更新审核规则
        /// <summary>
        /// 更新审核规则
        /// </summary>
        /// <param name="auditRuleUpdate"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> AuditRuleSubmit(AuditRuleUpdate auditRuleUpdate)
        {
            auditRuleUpdate.CreateUserID = IDHelper.GetUserID();
            auditRuleUpdate.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/AuditRule/Update", auditRuleUpdate);
            return result;
        }
        #endregion

        #region 启用停用审核规则
        /// <summary>
        /// 启用停用审核规则
        /// </summary>
        /// <param name="auditRuleStopOrUse"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> AuditRuleStopOrUse(AuditRuleStopOrUse auditRuleStopOrUse)
        {
            auditRuleStopOrUse.CreateUserID = IDHelper.GetUserID();
            auditRuleStopOrUse.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/AuditRule/StopOrUse", auditRuleStopOrUse);
            return result;
        }
        #endregion
    }
}