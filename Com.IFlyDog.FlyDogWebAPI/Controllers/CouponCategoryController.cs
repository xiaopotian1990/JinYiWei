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
    /// 代金券类型api
    /// </summary>
    public class CouponCategoryController : ApiController
    {
        private ICouponCategoryService _couponCategoryService;
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="couponCategoryService"></param>
        public CouponCategoryController(ICouponCategoryService couponCategoryService)
        {
            _couponCategoryService = couponCategoryService;
        }

        /// <summary>
        /// 添加代金券类型管理[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">代金券类型</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add([FromBody]CouponCategoryAdd dto)
        {
            return _couponCategoryService.Add(dto);
        }

        // <summary>
        /// 代金券类型管理修改[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">代金券类型</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update([FromBody]CouponCategoryUpdate dto)
        {
            return _couponCategoryService.Update(dto);
        }

        /// <summary>
        /// 获取全部代金券类型信息，
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<CouponCategoryInfo>> Get()
        {
            return _couponCategoryService.Get();
        }

        /// <summary>
        /// 根据医院id查询当前医院状态为可使用的所有卷类型s
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<CouponCategoryInfo>> GetByHospitalID(long hospitalID)
        {
            return _couponCategoryService.GetByHospitalID(hospitalID);
        }

        /// <summary>
        /// 根据ID获取代金券类型
        /// </summary>
        /// <param name="id">代金券类型</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, CouponCategoryInfo> GetByID(long id)
        {
            return _couponCategoryService.GetByID(id);
        }

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect(long hospitalID)
        {
            return _couponCategoryService.GetSelect(hospitalID);
        }
    }
}
