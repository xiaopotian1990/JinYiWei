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
    /// 项目折扣api
    /// </summary>
    public class ChargeDiscountController : ApiController
    {
        /// <summary>
        /// 依赖注入
        /// </summary>
        private IChargeDiscountService _chargeDiscountService;

        /// <summary>
        /// 依赖注入方法重构
        /// </summary>
        /// <param name="chargeDiscountService"></param>
        public ChargeDiscountController(IChargeDiscountService chargeDiscountService)
        {
            _chargeDiscountService = chargeDiscountService;
        }

        /// <summary>
        /// 查询所有项目折扣[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<ChargeDiscountInfo>>> Get([FromBody]ChargeDiscountSelect dto)
        {
            return _chargeDiscountService.Get(dto);
        }

        /// <summary>
        /// 根据ID查询一条数据
        /// </summary>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, ChargeDiscountInfo> GetByID(long id)
        {
            return _chargeDiscountService.GetByID(id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add(ChargeDiscountAdd dto)
        {
            return _chargeDiscountService.Add(dto);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update(ChargeDiscountUpdate dto)
        {
            return _chargeDiscountService.Update(dto);
        }

    }
}
