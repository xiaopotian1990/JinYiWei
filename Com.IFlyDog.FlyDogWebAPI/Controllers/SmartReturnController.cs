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
    /// 退货信息api控制器
    /// </summary>
    public class SmartReturnController : ApiController
    {
        /// <summary>
        /// 依赖注入
        /// </summary>
        private ISmartReturnService _smartReturnService;

        /// <summary>
        /// 依赖注入方法重构
        /// </summary>
        /// <param name="smartReturnService"></param>
        public SmartReturnController(ISmartReturnService smartReturnService)
        {
            _smartReturnService = smartReturnService;
        }

        /// <summary>
        /// 查询所有退货信息[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<SmartReturnInfo>>> Get([FromBody]SmartReturnSelect dto1)
        {
            return _smartReturnService.Get(dto1);
        }

        /// <summary>
        /// 根据ID查询一条数据
        /// </summary>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, SmartReturnInfo> GetByID(long id)
        {
            return _smartReturnService.GetByID(id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add(SmartReturnAdd dto)
        {
            return _smartReturnService.Add(dto);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update(SmartReturnUpdate dto)
        {
            return _smartReturnService.Update(dto);
        }

        /// <summary>
        /// 删除退货管理[所属角色("CRM")]
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Delete([FromBody]SmartReturnDelete dto)
        {
            return _smartReturnService.SmartReturnDelete(dto);
        }

        /// <summary>
        /// 根据仓库退货id查询仓库退货详情拼接成字符串打印出来
        /// </summary>
        /// <param name="returnID"></param>
        /// <returns></returns>
        [HttpGet]
        public IFlyDogResult<IFlyDogResultType, string> SmartReturnPrint(string returnID, long hospitalID)
        {
            return _smartReturnService.SmartReturnPrint(returnID, hospitalID);
        }
    }
}
