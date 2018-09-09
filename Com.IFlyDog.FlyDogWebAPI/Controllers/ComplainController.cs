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
    /// 投诉处理api
    /// </summary>
    public class ComplainController : ApiController
    {
        /// <summary>
        /// 依赖注入
        /// </summary>
        private IComplainService _complainService;

        /// <summary>
        /// 依赖注入方法重构
        /// </summary>
        /// <param name="complainService"></param>
        public ComplainController(IComplainService complainService)
        {
            _complainService = complainService;
        }

        /// <summary>
        /// 查询所有投诉信息[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<ComplainInfo>>> Get([FromBody]ComplainSelect dto)
        {
            return _complainService.Get(dto);
        }

        /// <summary>
        /// 根据ID查询一条数据
        /// </summary>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, ComplainInfo> GetByID(long id)
        {
            return _complainService.GetByID(id);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update(ComplainUpdate dto)
        {
            return _complainService.Update(dto);
        }
    }
}
