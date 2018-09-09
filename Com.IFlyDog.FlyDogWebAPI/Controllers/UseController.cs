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
    /// 科室领用api控制器
    /// </summary>
    public class UseController : ApiController
    {
        /// <summary>
        /// 依赖注入
        /// </summary>
        private IUseService _useService;

        /// <summary>
        /// 依赖注入方法重构
        /// </summary>
        /// <param name="useService"></param>
        public UseController(IUseService useService)
        {
            _useService = useService;
        }

        /// <summary>
        /// 查询所有退货信息[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<UseInfo>>> Get([FromBody]UseSelect dto)
        {
            return _useService.Get(dto);
        }

        /// <summary>
        /// 根据ID查询一条数据
        /// </summary>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, UseInfo> GetByID(long id)
        {
            return _useService.GetByID(id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add(UseAdd dto)
        {
            return _useService.Add(dto);
        }

        /// <summary>
        /// 根据科室领用id查询科室领用详情拼接成字符串打印出来
        /// </summary>
        /// <param name="UseID"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, string> SmartUsePrint(string UseID, long hospitalID)
        {
            return _useService.SmartUsePrint(UseID, hospitalID);
        }
    }
}
