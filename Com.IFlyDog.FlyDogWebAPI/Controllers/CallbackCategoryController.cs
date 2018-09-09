using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.WebAPI.Filters;
using System.Collections.Generic;
using System.Web.Http;

namespace Com.IFlyDog.FlyDogWebAPI.Controllers
{
    /// <summary>
    /// 回访类型设置
    /// </summary>
    public class CallbackCategoryController : ApiController
    {
        /// <summary>
        /// 依赖注入
        /// </summary>
        private ICallbackCategoryService _callbackCategoryService;

        /// <summary>
        /// 方法重构
        /// </summary>
        /// <param name="callbackCategoryService"></param>
        public CallbackCategoryController(ICallbackCategoryService callbackCategoryService)
        {
            _callbackCategoryService = callbackCategoryService;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<SmartCallbackCategory>> Get()
        {
           return  _callbackCategoryService.Get();
        }


        /// <summary>
        /// 添加投诉类型[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add([FromBody]SmartCallbackCategoryAdd dto)
        {
            return _callbackCategoryService.Add(dto);
        }

        /// <summary>
        /// 通过ID查询一条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, SmartCallbackCategory> GetByID(long id)
        {
            return _callbackCategoryService.GetByID(id);
        }

        /// <summary>
        /// 添加[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> StopOrUse([FromBody]SmartCallbackCategoryStopOrUse dto)
        {
            return _callbackCategoryService.StopOrUse(dto);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update([FromBody]SmartCallbackCategoryUpdate dto)
        {
            return _callbackCategoryService.Update(dto);
        }

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect()
        {
            return _callbackCategoryService.GetSelect();
        }
    }
}
