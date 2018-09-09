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
    /// 审核工作台api相关接口
    /// </summary>
    public class AuditWorkbenchController : ApiController
    {
        private IAuditWorkbenchService _auditWorkbenchService;

        #region 依赖注入类
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="auditWorkbenchService"></param>
        public AuditWorkbenchController(IAuditWorkbenchService auditWorkbenchService)
        {
            _auditWorkbenchService = auditWorkbenchService;
        }
        #endregion

        #region 审核操作
        /// <summary>
        /// 审核操作[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">审核操作</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> AuditOperationAdd([FromBody]AuditOperationAdd dto)
        {
            return _auditWorkbenchService.AuditOperationAdd(dto);
        }
        #endregion

        /// <summary>
        /// 查询所有待审核信息[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<AuditWorkbenchInfo>>> GetAllAudit([FromBody]AuditWorkbenchSelect dto)
        {
            return _auditWorkbenchService.GetAllAudit(dto);
        }

        /// <summary>
        /// 查询所有审核记录信息[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<AuditRecordInfo>>> GetAuditRecord([FromBody]AuditRecordSelect dto)
        {
            return _auditWorkbenchService.GetAuditRecord(dto);
        }

        /// <summary>
        /// 点击查询查询此类型的审核用户信息
        /// </summary>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, AuditUserInfo> GetByType(AuditUserSelect dto)
        {
            return _auditWorkbenchService.GetByType(dto);
        }

        /// <summary>
        /// 点击审核跳转到审核界面如果是开发人员或者咨询人员需要传递类型 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type">4 开发人员  5咨询人员</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, AuditOrderInfo> GetAuditOrderInfo(long id, string type)
        {
            return _auditWorkbenchService.GetAuditOrderInfo(id, type);
        }
        }
}
