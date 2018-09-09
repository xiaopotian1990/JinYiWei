using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.WebAPI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Com.IFlyDog.FlyDogWebAPI.Controllers
{
    /// <summary>
    /// 班次管理API相关接口
    /// </summary>
    public class SmartShiftCategoryController : ApiController
    {
        private ISmartShiftCategoryService _smartShiftCategoryService;

        #region 依赖注入类
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="smartShiftCategoryService"></param>
        public SmartShiftCategoryController(ISmartShiftCategoryService smartShiftCategoryService)
        {
            _smartShiftCategoryService = smartShiftCategoryService;
        }
        #endregion

        /// <summary>
        /// 添加班次[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">班次信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add([FromBody]SmartShiftCategoryAdd dto)
        {
            return _smartShiftCategoryService.Add(dto);
        }

        /// <summary>
        /// 班级信息修改[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">班级信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update([FromBody]SmartShiftCategoryUpdate dto)
        {
            return _smartShiftCategoryService.Update(dto);
        }

        /// <summary>
        /// 班级信息使用停用[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">班级停用启用信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> SmartShiftCategoryDispose([FromBody]SmartShiftCategoryDispose dto)
        {
            return _smartShiftCategoryService.SmartShiftCategoryDispose(dto);
        }

        /// <summary>
        /// 查询所有班级信息[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<SmartShiftCategoryInfo>> Get()
        {
            return _smartShiftCategoryService.Get();
        }

        /// <summary>
        /// 根据ID获取部门信息
        /// </summary>
        /// <param name="id">部门ID</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, SmartShiftCategoryInfo> GetByID(long id)
        {
            return _smartShiftCategoryService.GetByID(id);
        }


        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect()
        {
            return _smartShiftCategoryService.GetSelect();
        }
        }
}