using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.WebAPI.Filters;
using System.Collections.Generic;
using System.Web.Http;

namespace Com.IFlyDog.FlyDogWebAPI.Controllers
{
    /// <summary>
    /// 未成交类型相关API
    /// </summary>
    public class FailtureCategoryController : ApiController
    {
        private IFailtureCategoryService _failtureCategoryService;
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="failtureCategoryService"></param>
        public FailtureCategoryController(IFailtureCategoryService failtureCategoryService)
        {
            _failtureCategoryService = failtureCategoryService;
        }

        /// <summary>
        /// 添加未成交类型
        /// </summary>
        /// <param name="dto">症状信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add([FromBody]FailtureCategoryAdd dto)
        {
            return _failtureCategoryService.Add(dto);
        }

        /// <summary>
        /// 未成交类型修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update([FromBody]FailtureCategoryUpdate dto)
        {
            return _failtureCategoryService.Update(dto);
        }

        /// <summary>
        /// 未成交类型使用停用
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> StopOrUse([FromBody]FailtureCategoryStopOrUse dto)
        {
            return _failtureCategoryService.StopOrUse(dto);
        }

        /// <summary>
        /// 查询所有未成交类型
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<FailtureCategory>> Get()
        {
            return _failtureCategoryService.Get();
        }

        /// <summary>
        /// 根据ID查询详细未成交类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, FailtureCategory> GetByID(long id)
        {
            return _failtureCategoryService.GetByID(id);
        }

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect()
        {
            return _failtureCategoryService.GetSelect();
        }
    }
}
