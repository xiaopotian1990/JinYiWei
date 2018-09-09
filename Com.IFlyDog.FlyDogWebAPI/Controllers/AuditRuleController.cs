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
    /// 审核规则api
    /// </summary>
    public class AuditRuleController : ApiController
    {
        private IAuditRuleService _auditRuleService;

        #region 依赖注入类
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="auditRuleService"></param>
        public AuditRuleController(IAuditRuleService auditRuleService)
        {
            _auditRuleService = auditRuleService;
        }
        #endregion

        /// <summary>
        /// 新增审核规则时查询当前医院是否已经存在相应的审核规则，如果存在则不能重复添加
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, int> GetByHtData(string hospitalID, int Type)
        {
            return _auditRuleService.GetByHtData(hospitalID, Type);
        }

        #region 添加审核规则
            /// <summary>
            /// 添加审核规则[所属角色("CRM")]
            /// </summary>
            /// <param name="dto">单位信息</param>
            /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add([FromBody]AuditRuleAdd dto)
        {
            return _auditRuleService.Add(dto);
        }
        #endregion

        #region 修改审核规则
        /// <summary>
        /// 修改审核规则[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">修改审核规则</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update([FromBody]AuditRuleUpdate dto)
        {
            return _auditRuleService.Update(dto);
        }
        #endregion

        #region 查询所有审核规则信息
        /// <summary>
        /// 查询所有审核规则信息[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<AuditRuleInfo>> Get(long hospitalID)
        {
            return _auditRuleService.Get(hospitalID);
        }
        #endregion

        #region 根据id获取审核规则详情
        /// <summary>
        /// 根据id获取审核规则详情
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, AuditRuleInfo> GetByID(long id)
        {
            return _auditRuleService.GetByID(id);
        }
        #endregion

        #region 启用停用审核规则
        /// <summary>
        /// 启用停用审核规则[所属角色("CRM")]
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> StopOrUse([FromBody]AuditRuleStopOrUse dto)
        {
            return _auditRuleService.StopOrUse(dto);
        }
        #endregion
    }
}
