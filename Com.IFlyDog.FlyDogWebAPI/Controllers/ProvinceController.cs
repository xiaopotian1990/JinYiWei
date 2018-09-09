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
    /// 省相关API
    /// </summary>
    public class ProvinceController : ApiController
    {
        private IProvinceService _provinceService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="provinceService"></param>
        public ProvinceController(IProvinceService provinceService)
        {
            _provinceService = provinceService;
        }


        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect()
        {
            return _provinceService.GetSelect();
        }

        /// <summary>
        /// 根据省查询市
        /// </summary>
        /// <param name="provinceID">省ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetCity(int provinceID)
        {
            return _provinceService.GetCity(provinceID);
        }

        /// <summary>
        /// 根据手机号自动识别省市
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, ProvinceCity> GetProvinceCity(string phone)
        {
            return _provinceService.GetProvinceCity(phone);
        }
    }
}
