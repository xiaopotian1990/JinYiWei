using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO.Knowledge;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.WebAPI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Com.IFlyDog.FlyDogWebAPI.Controllers
{
    /// <summary>
    /// 知识管理相关API
    /// </summary>
    public class KnowledgeController : ApiController
    {
        private IKnowledgeService _knowledgeService;
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="knowledgeService"></param>
        public KnowledgeController(IKnowledgeService knowledgeService)
        {
            _knowledgeService = knowledgeService;
        }

        /// <summary>
        /// 添加知识管理[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">知识管理</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add([FromBody]KnowledgeAdd dto)
        {
            return _knowledgeService.Add(dto);
        }

        /// <summary>
        /// 知识管理修改[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">知识管理</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update([FromBody]KnowledgeUpdate dto)
        {
            return _knowledgeService.Update(dto);
        }


        /// <summary>
        /// 查询所有知识管理[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<KnowledgeInfo>>> Get([FromBody]KnowledgeSelect dto)
        {
            return _knowledgeService.Get(dto);
        }

        /// <summary>
        /// 根据ID获取知识管理
        /// </summary>
        /// <param name="id">知识管理ID</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, KnowledgeInfo> GetByID(long id)
        {
            return _knowledgeService.GetByID(id);
        }
    }
}