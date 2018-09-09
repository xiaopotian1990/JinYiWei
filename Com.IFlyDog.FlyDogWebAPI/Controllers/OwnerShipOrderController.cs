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
    /// 客户添加/编辑开发咨询人员信息
    /// </summary>
    public class OwnerShipOrderController : ApiController
    {
        private IOwnerShipOrderService _ownerShipOrderService;

        #region 依赖注入类
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="ownerShipOrderService"></param>s
        public OwnerShipOrderController(IOwnerShipOrderService ownerShipOrderService)
        {
            _ownerShipOrderService = ownerShipOrderService;
        }
        #endregion


        #region 添加/编辑咨询人员变更申请
        /// <summary>
        /// 添加/编辑咨询人员变更申请[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">添加/编辑咨询人员变更申请</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> CustomerConsultanAdd([FromBody]CustomerConsultanAdd dto)
        {
            return _ownerShipOrderService.CustomerConsultanAdd(dto);
        }
        #endregion



        #region 添加/ 编辑开发人员变更申请
        /// <summary>
        /// 添加/ 编辑开发人员变更申请[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">添加/ 编辑开发人员变更申请</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> CustomerDeveloperAdd([FromBody]CustomerDeveloperAdd dto)
        {
            return _ownerShipOrderService.CustomerDeveloperAdd(dto);
        }
        #endregion


        #region 咨询/开发人员变更申请 加载
        /// <summary>
        /// 咨询/开发人员变更申请 加载[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">咨询/开发人员变更申请 加载</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, CustomerUserInfo> GetCustomerUserInfo([FromBody]CustomerUserSelect dto)
        {
            return _ownerShipOrderService.GetCustomerUserInfo(dto);
        }
        #endregion

    }
}
