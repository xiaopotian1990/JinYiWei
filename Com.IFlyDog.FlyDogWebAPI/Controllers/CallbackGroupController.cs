using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO.CallbackGroup;
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
    /// 回访组设置
    /// </summary>
    public class CallbackGroupController : ApiController
    {
        /// <summary>
        /// 依赖注入
        /// </summary>
        private ICallbackGroupService _callbackGroupService;

        /// <summary>
        /// 依赖注入方法重构
        /// </summary>
        /// <param name="callbackGroupService"></param>
        public CallbackGroupController(ICallbackGroupService callbackGroupService)
        {
            _callbackGroupService = callbackGroupService;
        }


        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<SmartCallbackGroup>> Get()
        {
            return _callbackGroupService.Get();
        }

        /// <summary>
        /// 根据ID查询一条数据
        /// </summary>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, SmartCallbackGroup> GetByID(long id)
        {
            return _callbackGroupService.GetByID(id);
        }

        /// <summary>
        /// 使用停用
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> StopOrUse(SmartCallbackGroupStopOrUse dto)
        {
            return _callbackGroupService.StopOrUse(dto);
        }
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add(SmartCallbackGroupAdd dto)
        {
            return _callbackGroupService.Add(dto);
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update(SmartCallbackGroupUpdate dto)
        {
            return _callbackGroupService.Update(dto);
        }

    }
}
