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
    /// 套餐管理api
    /// </summary>
    public class ChargeSetController : ApiController
    {
        /// <summary>
        /// 依赖注入
        /// </summary>
        private IChargeSetService _chargeSetService;

        /// <summary>
        /// 依赖注入方法重构
        /// </summary>
        /// <param name="chargeSetService"></param>
        public ChargeSetController(IChargeSetService chargeSetService)
        {
            _chargeSetService = chargeSetService;
        }

        /// <summary>
        /// 查询所有套餐信息[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<ChargeSetInfo>>> Get([FromBody]ChargeSetSelect dto1)
        {
            return _chargeSetService.Get(dto1);
        }

        /// <summary>
        /// 根据ID查询一条数据
        /// </summary>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, ChargeSetInfo> GetByID(long id)
        {
            return _chargeSetService.GetByID(id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add(ChargeSetAdd dto)
        {
            return _chargeSetService.Add(dto);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update(ChargeSetUpdate dto)
        {
            return _chargeSetService.Update(dto);
        }
    }
}
