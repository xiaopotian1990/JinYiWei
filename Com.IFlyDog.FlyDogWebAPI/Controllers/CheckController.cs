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
    /// 库存盘点api
    /// </summary>
    public class CheckController : ApiController
    {
        /// <summary>
        /// 依赖注入
        /// </summary>
        private ICheckService _checkService;

        /// <summary>
        /// 依赖注入方法重构
        /// </summary>
        /// <param name="checkService"></param>
        public CheckController(ICheckService checkService)
        {
            _checkService = checkService;
        }

        /// <summary>
        /// 查询所有盘点信息[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<CheckInfo>>> Get([FromBody]CheckSelect dto1)
        {
            return _checkService.Get(dto1);
        }

        /// <summary>
        /// 根据ID查询一条数据
        /// </summary>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, CheckInfo> GetByID(long id)
        {
            return _checkService.GetByID(id);
        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add(CheckAdd dto)
        {
            return _checkService.Add(dto);
        }


    }
}
