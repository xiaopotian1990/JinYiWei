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
    /// 药物品类型管理
    /// </summary>
    public class SmartProductCategoryController : ApiController
    {
        private ISmartProductCategoryService _smartProductCategoryService;

        #region 依赖注入类
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="smartProductCategoryService"></param>
        public SmartProductCategoryController(ISmartProductCategoryService smartProductCategoryService)
        {
            _smartProductCategoryService = smartProductCategoryService;
        }
        #endregion

        #region 添加药物品类型
        /// <summary>
        /// 添加药物品类型[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">药物品信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add([FromBody]SmartProductCategoryAdd dto)
        {
            return _smartProductCategoryService.Add(dto);
        }
        #endregion

        #region 药物品类型信息修改
        /// <summary>
        /// 药物品类型信息修改[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">药物品信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update([FromBody]SmartProductCategoryUpdate dto)
        {
            return _smartProductCategoryService.Update(dto);
        }
        #endregion

        #region 查询所有单位信息
        /// <summary>
        /// 查询所有药物品信息[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<SmartProductCategoryInfo>> Get()
        {
            return _smartProductCategoryService.Get();
        }
        #endregion

        #region 根据ID获取药物品信息
        /// <summary>
        /// 根据ID获取药物品信息
        /// </summary>
        /// <param name="id">药物品ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, SmartProductCategoryInfo> GetByID(long id)
        {
            return _smartProductCategoryService.GetByID(id);
        }
        #endregion

        #region 删除药物品信息
        /// <summary>
        /// 删除药物品信息[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">删除药物品信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Delete([FromBody]SmartProductCategoryDelete dto)
        {
            return _smartProductCategoryService.Delete(dto);
        }
        #endregion

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect()
        {
            return _smartProductCategoryService.GetSelect();
        }
    }
}
