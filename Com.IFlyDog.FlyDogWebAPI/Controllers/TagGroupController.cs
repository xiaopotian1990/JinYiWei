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
    /// 标签组api
    /// </summary>
    public class TagGroupController : ApiController
    {
        /// <summary>
        /// 依赖注入
        /// </summary>
        private ITagGroupService _tagGroupService;

        /// <summary>
        /// 依赖注入方法重构
        /// </summary>
        /// <param name="tagGroupService"></param>
        public TagGroupController(ITagGroupService tagGroupService)
        {
            _tagGroupService = tagGroupService;
        }

        #region 查询所有标签组信息
        /// <summary>
        /// 查询所有标签组信息[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<TagGroupInfo>> Get()
        {
            return _tagGroupService.Get();
        }
        #endregion

        #region 
        /// <summary>
        /// 根据ID获取标签组信息
        /// </summary>
        /// <param name="id">单位ID</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, TagGroupInfo> GetByID(long id)
        {
            return _tagGroupService.GetByID(id);
        }
        #endregion

        #region 删除标签组信息s
        /// <summary>
        /// 删除标签组信息s[所属角色("CRM")]
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Delete([FromBody]TagGroupDelete dto)
        {
            return _tagGroupService.Delete(dto);
        }
        #endregion

        #region 添加标签组
        /// <summary>
        /// 添加标签组[所属角色("CRM")]
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add([FromBody]TagGroupAdd dto)
        {
            return _tagGroupService.Add(dto);
        }
        #endregion

        #region 修改标签组
        /// <summary>
        /// 修改标签组[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update([FromBody]TagGroupUpdate dto)
        {
            return _tagGroupService.Update(dto);
        }
        #endregion
    }
}
