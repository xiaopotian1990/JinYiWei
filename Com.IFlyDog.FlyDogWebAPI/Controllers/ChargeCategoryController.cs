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
    /// 项目分类api
    /// </summary>
    public class ChargeCategoryController : ApiController
    {
        private IChargeCategoryService _chargeCategoryService;

        #region 依赖注入类
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="chargeCategoryService"></param>
        public ChargeCategoryController(IChargeCategoryService chargeCategoryService)
        {
            _chargeCategoryService = chargeCategoryService;
        }
        #endregion

        #region 添加项目分类类型
        /// <summary>
        /// 添加项目分类类型[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">项目分类信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add([FromBody]ChargeCategoryAdd dto)
        {
            return _chargeCategoryService.Add(dto);
        }
        #endregion

        #region 项目分类信息修改
        /// <summary>
        /// 项目分类修改[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">项目分类信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update([FromBody]ChargeCategoryUpdate dto)
        {
            return _chargeCategoryService.Update(dto);
        }
        #endregion

        #region 查询所有单位信息
        /// <summary>
        /// 查询所有项目分类信息[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<ChargeCategoryInfo>> Get()
        {
            return _chargeCategoryService.Get();
        }
        #endregion

        #region 根据ID获取项目分类信息
        /// <summary>
        /// 根据ID获取项目分类信息
        /// </summary>
        /// <param name="id">项目分类ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, ChargeCategoryInfo> GetByID(long id)
        {
            return _chargeCategoryService.GetByID(id);
        }
        #endregion

        #region 删除药物品信息
        /// <summary>
        /// 删除项目分类信息[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">删除项目分类信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Delete([FromBody]ChargeCategoryDelete dto)
        {
            return _chargeCategoryService.Delete(dto);
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
            return _chargeCategoryService.GetSelect();
        }
    }
}
