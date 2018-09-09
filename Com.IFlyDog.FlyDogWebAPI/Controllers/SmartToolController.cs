using System.Collections.Generic;
using System.Web.Http;
using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.WebAPI.Filters;
using Com.IFlyDog.APIDTO;

namespace Com.IFlyDog.FlyDogWebAPI.Controllers
{
    /// <summary>
    ///     工具API
    /// </summary>
    public class SmartToolController : ApiController
    {
        /// <summary>
        ///     注入
        /// </summary>
        private readonly ISmartToolService _smartToolService;

        /// <summary>
        ///     注入
        /// </summary>
        public SmartToolController(ISmartToolService smartToolService)
        {
            _smartToolService = smartToolService;
        }

        /// <summary>
        ///     添加工具
        /// </summary>
        /// <param name="dto">工具信息</param>
        /// <returns></returns>
        [ModuleAuthorization("CRM")]
        [HttpPost]
        public IFlyDogResult<IFlyDogResultType, int> Add(SmartToolAdd dto)
        {
            return _smartToolService.Add(dto);
        }


        /// <summary>
        ///     更新工具信息
        /// </summary>
        /// <param name="dto">工具信息</param>
        /// <returns></returns>
        [ModuleAuthorization("CRM")]
        [HttpPost]
        public IFlyDogResult<IFlyDogResultType, int> Update(SmartToolUpdate dto)
        {
            return _smartToolService.Update(dto);
        }


        /// <summary>
        ///     工具使用停用
        /// </summary>
        /// <param name="dto">参数集</param>
        /// <returns></returns>
        [ModuleAuthorization("CRM")]
        [HttpPost]
        public IFlyDogResult<IFlyDogResultType, int> StopOrUse(SmartToolStopOrUse dto)
        {
            return _smartToolService.StopOrUse(dto);
        }

        /// <summary>
        ///     查询所有工具
        /// </summary>
        /// <returns></returns>
        [ModuleAuthorization("CRM")]
        [HttpGet]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<SmartToolInfo>> Get()
        {
            return _smartToolService.Get();
        }

        /// <summary>
        ///     查询工具详细
        /// </summary>
        /// <param name="id">工具ID</param>
        /// <returns></returns>
        [ModuleAuthorization("CRM")]
        [HttpGet]
        public IFlyDogResult<IFlyDogResultType, SmartToolInfo> GetByID(long id)
        {
            return _smartToolService.GetByID(id);
        }

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        [ModuleAuthorization("CRM")]
        [HttpGet]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect()
        {
            return _smartToolService.GetSelect();
        }
    }
}