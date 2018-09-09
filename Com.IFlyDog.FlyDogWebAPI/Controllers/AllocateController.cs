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
    /// 仓库调拨api接口
    /// </summary>
    public class AllocateController : ApiController
    {
        /// <summary>
        /// 依赖注入
        /// </summary>
        private IAllocateService _allocateService;

        /// <summary>
        /// 依赖注入方法重构
        /// </summary>
        /// <param name="allocateService"></param>
        public AllocateController(IAllocateService allocateService)
        {
            _allocateService = allocateService;
        }

        /// <summary>
        /// 查询所有进货信息[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<AllocateInfo>>> Get([FromBody]AllocateSelect dto1)
        {
            return _allocateService.Get(dto1);
        }

        /// 根据ID查询一条数据
        /// </summary>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, AllocateInfo> GetByID(long id)
        {
            return _allocateService.GetByID(id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add(AllocateAdd dto)
        {
            return _allocateService.Add(dto);
        }

        /// <summary>
        /// 根据仓库进货id查询详情打印
        /// </summary>
        /// <param name="allocateID"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, string> SmartAllocatePrint(string allocateID, long hospitalID)
        {
            return _allocateService.SmartAllocatePrint(allocateID, hospitalID);
        }
    }
}
