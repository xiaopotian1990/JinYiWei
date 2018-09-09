using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.WebAPI.Filters;
using System.Collections.Generic;
using System.Web.Http;

namespace Com.IFlyDog.FlyDogWebAPI.Controllers
{
    /// <summary>
    /// 投诉类型API
    /// </summary>
    public class ComplainCategoryController : ApiController
    {
        private IComplainCategoryService _complainCategoryService;
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="complainCategoryService"></param>
        public ComplainCategoryController(IComplainCategoryService complainCategoryService)
        {
            _complainCategoryService = complainCategoryService;
        }

        /// <summary>
        /// 查询所有投诉类型[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<SmartComplainCategory>> Get()
        {
            return _complainCategoryService.Get();
        }

        /// <summary>
        /// 添加投诉类型[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add([FromBody]SmartComplainCategoryAdd dto)
        {
            return _complainCategoryService.Add(dto);
        }

        /// <summary>
        /// 通过ID查询一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, SmartComplainCategory> GetByID(long id)
        {
            return _complainCategoryService.GetByID(id);
        }

        /// <summary>
        /// 添加投诉类型[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> StopOrUse([FromBody]SmartComplainCategoryStopOrUse dto)
        {
            return _complainCategoryService.StopOrUse(dto);
        }

        /// <summary>
        /// 修改投诉
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update([FromBody]SmartComplainCategoryUpdate dto)
        {
            return _complainCategoryService.Update(dto);
        }

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect()
        {
            return _complainCategoryService.GetSelect();
        }
    }
}
