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
    /// 客户所属权api
    /// </summary>
    public class OwnerShipController : ApiController
    {
        private IOwnerShipService _ownerShipService;

        #region 依赖注入类
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="ownerShipService"></param>
        public OwnerShipController(IOwnerShipService ownerShipService)
        {
            _ownerShipService = ownerShipService;
        }
        #endregion

        /// <summary>
        /// 根据条件查询客户信息
        /// </summary>
        /// <param name="type">1 查询开发类型 2 查询咨询类型</param>
        /// <param name="userID">查询开发或者咨询人员客户</param>
        /// <param name="hospitalID">当前医院</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<SingleCustormInfo>> GetByFiltrate(string type, long userID, long hospitalID)
        {
            return _ownerShipService.GetByFiltrate(type, userID, hospitalID);
        }

        #region 批量设置咨询人员归属权
        /// <summary>
        /// 批量设置咨询人员归属权[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">批量设置咨询人员归属权</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> BatchConsultantUserAdd([FromBody]BatchConsultantUser dto)
        {
            return _ownerShipService.BatchConsultantUserAdd(dto);
        }
        #endregion

        #region 批量设置开发人员归属权
        /// <summary>
        /// 批量设置开发人员归属权[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">批量设置开发人员归属权</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> BatchDeveloperUserAdd([FromBody]BatchDeveloperUser dto)
        {
            return _ownerShipService.BatchDeveloperUserAdd(dto);
        }
        #endregion



        #region 查询当前医院客户归属权管理
        /// <summary>
        /// 查询当前医院客户归属权管理[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<OwnerShipInfo>> Get(long hospitalID)
        {
            return _ownerShipService.Get(hospitalID);
        }
        #endregion

        #region 单个添加咨询人员客户归属权
        /// <summary>
        /// 单个添加咨询人员客户归属权[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">单个添加咨询人员客户归属权</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> SingleConsultantUserUpdateAdd([FromBody]SingleConsultantUserUpdate dto)
        {
            return _ownerShipService.SingleConsultantUserUpdateAdd(dto);
        }
        #endregion

        #region 单个添加开发人员客户归属权
        /// <summary>
        /// 单个添加开发人员客户归属权[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">单个添加开发人员客户归属权</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> SingleDeveLoperUserUpdateAdd([FromBody]SingleDeveLoperUserUpdate dto)
        {
            return _ownerShipService.SingleDeveLoperUserUpdateAdd(dto);
        }
        #endregion
    }
}
