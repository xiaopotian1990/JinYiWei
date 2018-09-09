using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.WebAPI.Filters;
using System.Collections.Generic;
using System.Web.Http;

namespace Com.IFlyDog.FlyDogWebAPI.Controllers
{
    /// <summary>
    /// 岗位分工api
    /// </summary>
    public class PositionController : ApiController
    {
        private IPositionService _positionService;

        #region 依赖注入类
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="positionService"></param>
        public PositionController(IPositionService positionService)
        {
            _positionService = positionService;
        }
        #endregion

        #region 添加岗位分工
        /// <summary>
        /// 添加岗位分工[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">添加岗位分工</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add([FromBody]PositionAdd dto)
        {
            return _positionService.Add(dto);
        }
        #endregion

        #region 修改岗位分工
        /// <summary>
        /// 修改岗位分工[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">修改岗位分工</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update([FromBody]PositionUpdate dto)
        {
            return _positionService.Update(dto);
        }
        #endregion

        #region 查询所有岗位信息
        /// <summary>
        /// 查询所有岗位信息[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<PositionInfo>> Get()
        {
            return _positionService.Get();
        }
        #endregion

        #region 根据ID获取岗位信息
        /// <summary>
        /// 根据ID获取岗位信息
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, PositionInfo> GetByID(long id)
        {
            return _positionService.GetByID(id);
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
            return _positionService.GetSelect();
        }
    }
}
