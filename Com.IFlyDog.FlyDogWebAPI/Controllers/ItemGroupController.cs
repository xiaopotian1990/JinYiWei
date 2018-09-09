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
    /// 报表项目组api
    /// </summary>
    public class ItemGroupController : ApiController
    {

        private IitemGroupService _itemGroupService;

        #region 依赖注入类
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="itemGroupService"></param>
        public ItemGroupController(IitemGroupService itemGroupService)
        {
            _itemGroupService = itemGroupService;
        }
        #endregion

        #region 添加报表项目
        /// <summary>
        /// 添加报表项目[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">添加报表项目</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add([FromBody]ItemGroupAdd dto)
        {
            return _itemGroupService.Add(dto);
        }
        #endregion

        #region 修改报表项目
        /// <summary>
        /// 修改报表项目[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">修改报表项目</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update([FromBody]ItemGroupUpdate dto)
        {
            return _itemGroupService.Update(dto);
        }
        #endregion

        #region 查询所有报表项目
        /// <summary>
        /// 查询所有报表项目[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<ItemGroupInfo>> Get()
        {
            return _itemGroupService.Get();
        }
        #endregion

        #region 根据id获取报表项目详情
        /// <summary>
        /// 根据id获取报表项目详情
        /// </summary>
        /// <param name="id">根据id获取报表项目详情</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, ItemGroupInfo> GetByID(long id)
        {
            return _itemGroupService.GetByID(id);
        }
        #endregion

        #region 删除报表项目组
        /// <summary>
        /// 删除报表项目组[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">删除报表项目组</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Delete([FromBody]ItemGroupDelete dto)
        {
            return _itemGroupService.Delete(dto);
        }
        #endregion

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect()
        {
            return _itemGroupService.GetSelect();
        }
    }
}
