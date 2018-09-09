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
    /// 病例模板api
    /// </summary>
    public class CaseTemplateController : ApiController
    {
        private ICaseTemplateService _caseTemplateService;

        #region 依赖注入类
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="caseTemplateService"></param>
        public CaseTemplateController(ICaseTemplateService caseTemplateService)
        {
            _caseTemplateService = caseTemplateService;
        }
        #endregion

        #region 添加病例模板
        /// <summary>
        /// 添加病例模板[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add([FromBody]CaseTemplateAdd dto)
        {
            return _caseTemplateService.Add(dto);
        }
        #endregion

        #region 修改病例模板
        /// <summary>
        /// 修改病例模板[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update([FromBody]CaseTemplateUpdate dto)
        {
            return _caseTemplateService.Update(dto);
        }
        #endregion

        #region 查询所有病例模板
        /// <summary>
        /// 查询所有病例模板[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
       [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<CaseTemplateInfo>> Get(CaseTemplateSelect dto)
        {
            return _caseTemplateService.Get(dto);
        }
        #endregion

        #region 根据id获取病例模板
        /// <summary>
        /// 根据id获取病例模板
        /// </summary>
        /// <param name="id">单位ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, CaseTemplateInfo> GetByID(long id)
        {
            return _caseTemplateService.GetByID(id);
        }
        #endregion

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect()
        {
            return _caseTemplateService.GetSelect();
        }
    }
}
