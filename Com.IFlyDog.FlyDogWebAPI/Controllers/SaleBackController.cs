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
    /// 回款记录
    /// </summary>
    public class SaleBackController : ApiController
    {
        private ISaleBackService _saleBackService;

        #region 依赖注入类
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="saleBackService"></param>
        public SaleBackController(ISaleBackService saleBackService)
        {
            _saleBackService = saleBackService;
        }
        #endregion

        /// <summary>
        /// 新增回款记录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add(SaleBackAdd dto)
        {
            return _saleBackService.Add(dto);
        }

        /// <summary>
        /// 删除回款记录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Delete(SaleBackDelete dto)
        {
            return _saleBackService.Delete(dto);
        }

        /// <summary>
        /// 查询所有回款记录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<SaleBackInfo>>> Get(SaleBackSelect dto)
        {
            return _saleBackService.Get(dto);
        }

        /// <summary>
        /// 根据id查询回款记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, SaleBackInfo> GetByID(string id)
        {
            return _saleBackService.GetByID(Convert.ToInt64(id));
        }

        /// <summary>
        /// 更新回款记录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update(SaleBackUpdate dto)
        {
            return _saleBackService.Update(dto);
        }
    }
}
