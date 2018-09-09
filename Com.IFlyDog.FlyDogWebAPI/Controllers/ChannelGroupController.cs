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
    /// 渠道组api
    /// </summary>
    public class ChannelGroupController : ApiController
    {
        /// <summary>
        /// 依赖注入
        /// </summary>
        private IChannelGroupService _channelGroupService;

        /// <summary>
        /// 依赖注入方法重构
        /// </summary>
        /// <param name="channelGroupService"></param>
        public ChannelGroupController(IChannelGroupService channelGroupService)
        {
            _channelGroupService = channelGroupService;
        }

        #region 查询所有渠道组信息
        /// <summary>
        /// 查询所有渠道组信息[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<ChannelGroupInfo>> Get()
        {
            return _channelGroupService.Get();
        }
        #endregion

        #region 
        /// <summary>
        /// 根据ID获取渠道组信息
        /// </summary>
        /// <param name="id">单位ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, ChannelGroupInfo> GetByID(long id)
        {
            return _channelGroupService.GetByID(id);
        }
        #endregion

        #region 删除渠道组信息s
        /// <summary>
        /// 删除渠道组信息s[所属角色("CRM")]
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Delete([FromBody]ChannelGroupDelete dto)
        {
            return _channelGroupService.Delete(dto);
        }
        #endregion

        #region 添加渠道组
        /// <summary>
        /// 添加渠道组[所属角色("CRM")]
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add([FromBody]ChannelGroupAdd dto)
        {
            return _channelGroupService.Add(dto);
        }
        #endregion

        #region 修改渠道组
        /// <summary>
        /// 修改渠道组[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update([FromBody]ChannelGroupUpdate dto)
        {
            return _channelGroupService.Update(dto);
        }
        #endregion

        /// <summary>
        /// 检测所有渠道信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<ChannelGroupCheck>> GetChannelGroupCheck()
        {
            return _channelGroupService.GetChannelGroupCheck();
        }

    }
}
