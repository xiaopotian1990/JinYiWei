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
    /// 库存查询api
    /// </summary>
    public class SmartStockController : ApiController
    {
        private ISmartStockService _smartStockService;
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="smartStockService"></param>
        public SmartStockController(ISmartStockService smartStockService)
        {
            _smartStockService = smartStockService;
        }

        /// <summary>
        /// 查询所有供应商管理[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<SmartStockInfo>> Get([FromBody]SmartStockSelect dto)
        {
            return _smartStockService.Get(dto);
        }

        /// <summary>
        /// 根据条件查询库存药物品信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<SmartStockProductInfo>> GetByConditionData(SmartStockSelect dto)
        {
            return _smartStockService.GetByCondition(dto);
        }

        /// <summary>
        /// 根据医院id查询库存商品，按照有效期排序
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<SmartStockInfo>>> GetByHospitalIDData(SmartStockSelect dto)
        {
            return _smartStockService.GetByHospitalIDData(dto);
        }

        /// <summary>
        /// 添加库存[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">添加库存</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add([FromBody]SmartStockAdd dto)
        {
            return _smartStockService.Add(dto);
        }
    }
}
