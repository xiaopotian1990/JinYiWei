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
    /// 报表项目api
    /// </summary>
    public class SmartItemController : ApiController
    {
        /// <summary>
        /// 依赖注入
        /// </summary>
        private ISmartItemService _smartItemService;

        /// <summary>
        /// 依赖注入方法重构
        /// </summary>
        /// <param name="smartItemService"></param>
        public SmartItemController(ISmartItemService smartItemService)
        {
            _smartItemService = smartItemService;
        }

        #region 查询所有报表项目组
        /// <summary>
        /// 查询所有报表项目组[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<SmartItemInfo>> Get()
        {
            return _smartItemService.Get();
        }
        #endregion

        #region 
        /// <summary>
        /// 根据ID获取报表项目组
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, SmartItemInfo> GetByID(long id)
        {
            return _smartItemService.GetByID(id);
        }
        #endregion

        #region 删除报表项目组
        /// <summary>
        /// 删除报表项目组[所属角色("CRM")]
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Delete([FromBody]SmartItemDelete dto)
        {
            return _smartItemService.Delete(dto);
        }
        #endregion

        #region 添加报表项目组
        /// <summary>
        /// 添加报表项目组[所属角色("CRM")]
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add([FromBody]SmartItemAdd dto)
        {
            return _smartItemService.Add(dto);
        }
        #endregion

        #region 修改报表项目组
        /// <summary>
        /// 修改报表项目组[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update([FromBody]SmartItemUpdate dto)
        {
            return _smartItemService.Update(dto);
        }
        #endregion

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect()
        {
            return _smartItemService.GetSelect();
        }

    }
}
