using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.WebAPI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Com.IFlyDog.FlyDogWebAPI.Controllers
{
    /// <summary>
    /// 我的审核申请记录api
    /// </summary>
    public class AuditApplyController : ApiController
    {
        private IAuditApplyService _auditApplyService;

        #region 依赖注入类
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="auditApplyService"></param>
        public AuditApplyController(IAuditApplyService auditApplyService)
        {
            _auditApplyService = auditApplyService;
        }
        #endregion

        #region 查询当前用户所有的审核申请
        /// <summary>
        /// 查询当前用户所有的审核申请[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<AuditApplyInfo>>> Get(AuditApplySelect dto)
        {
            return _auditApplyService.Get(dto);
        }
        #endregion

        #region 根据操作i的，类型查询审核详情
        /// <summary>
        /// 根据操作i的，类型查询审核详情
        /// </summary>
        /// <param name="orderID">操作id，目前主要是咨询或者开发人员变更id</param>
        /// <param name="type"> 4 咨询人员变更 5 开发人员变更</param>
        /// <returns></returns>
       [HttpGet]
        public IFlyDogResult<IFlyDogResultType, OwnerShipOrderAudit> GetAuditDetail(long orderID, string type, long hospitalID, long userID)
        {
            return _auditApplyService.GetAuditDetail(orderID, type,hospitalID,userID);
        }
        #endregion

        #region 取消我的审核申请
        /// <summary>
        /// 取消我的审核申请[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">取消我的审核申请</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Delete([FromBody]AuditApplyDelete dto)
        {
            return _auditApplyService.Delete(dto);
        }
        #endregion
    }
}
