using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.WebAPI.Filters;
using System.Collections.Generic;
using System.Web.Http;

namespace Com.IFlyDog.FlyDogWebAPI.Controllers
{
    /// <summary>
    /// 单位管理API相关接口
    /// </summary>
    public class SmartUnitController : ApiController
    {
        private ISmartUnitService _smartUnitService;

        #region 依赖注入类
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="smartUnitService"></param>
        public SmartUnitController(ISmartUnitService smartUnitService)
        {
            _smartUnitService = smartUnitService;
        }
        #endregion

        #region 添加单位
        /// <summary>
        /// 添加单位[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">单位信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add([FromBody]SmartUnitAdd dto)
        {
            return _smartUnitService.Add(dto);
        }
        #endregion

        #region 单位信息修改
        /// <summary>
        /// 单位信息修改[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">单位信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update([FromBody]SmartUnitUpdate dto)
        {
            return _smartUnitService.Update(dto);
        }
        #endregion

        #region 查询所有单位信息
        /// <summary>
        /// 查询所有单位信息[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<SmartUnitInfo>> Get()
        {
            return _smartUnitService.Get();
        }
        #endregion

        #region 根据ID获取单位信息
        /// <summary>
        /// 根据ID获取单位信息
        /// </summary>
        /// <param name="id">单位ID</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, SmartUnitInfo> GetByID(long id)
        {
            return _smartUnitService.GetByID(id);
        }
        #endregion

        #region 删除单位信息
        /// <summary>
        /// 删除单位信息[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">删除单位信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Delete([FromBody]SmartUnitDelete dto)
        {
            return _smartUnitService.Delete(dto);
        }
        #endregion

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect()
        {
            return _smartUnitService.GetSelect();
        }
    }
}
