using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.WebAPI.Filters;
using System.Collections.Generic;
using System.Web.Http;

namespace Com.IFlyDog.FlyDogWebAPI.Controllers
{
    /// <summary>
    /// 知识分类相关API
    /// </summary>
    public class KnowledgeCategoryController : ApiController
    {
        private IKnowledgeCategoryService _knowledgeCategoryService;
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="knowledgeCategoryService"></param>
        public KnowledgeCategoryController(IKnowledgeCategoryService knowledgeCategoryService)
        {
            _knowledgeCategoryService = knowledgeCategoryService;
        }

        /// <summary>
        /// 添加知识分类[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">知识分类</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add([FromBody]KnowledgeCategoryAdd dto)
        {
            return _knowledgeCategoryService.Add(dto);
        }

        /// <summary>
        /// 知识分类修改[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">知识分类</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update([FromBody]KnowledgeCategoryUpdate dto)
        {
            return _knowledgeCategoryService.Update(dto);
        }


        /// <summary>
        /// 查询所有知识分类[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<KnowledgeCategory>> Get()
        {
            return _knowledgeCategoryService.Get();
        }

        /// <summary>
        /// 根据ID获取知识分类
        /// </summary>
        /// <param name="id">知识分类ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, KnowledgeCategory> GetByID(long id)
        {
            return _knowledgeCategoryService.GetByID(id);
        }

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect()
        {
            return _knowledgeCategoryService.GetSelect();
        }
    }
}