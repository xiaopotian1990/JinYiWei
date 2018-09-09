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
    /// 渠道相关API
    /// </summary>
    public class ChannelController : ApiController
    {
        private IChannelService _channelService;
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="channelService"></param>
        public ChannelController(IChannelService channelService)
        {
            _channelService = channelService;
        }

        /// <summary>
        /// 添加渠道
        /// </summary>
        /// <param name="dto">渠道信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add([FromBody]ChannelAdd dto)
        {
            return _channelService.Add(dto);
        }

        /// <summary>
        /// 渠道修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update([FromBody]ChannelUpdate dto)
        {
            return _channelService.Update(dto);
        }

        /// <summary>
        /// 渠道使用停用
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> StopOrUse([FromBody]ChannelStopOrUse dto)
        {
            return _channelService.StopOrUse(dto);
        }

        /// <summary>
        /// 查询所有渠道
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Channel>> Get()
        {
            return _channelService.Get();
        }

        /// <summary>
        /// 查询所有可使用的渠道
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Channel>> GetByIsOk()
        {
            return _channelService.GetByIsOk();
        }

        /// <summary>
        /// 根据渠道ID获取渠道信息
        /// </summary>
        /// <param name="id">渠道ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, Channel> GetByID(long id)
        {
            return _channelService.GetByID(id);
        }

        /// <summary>
        /// 渠道下拉菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect()
        {
            return _channelService.GetSelect();
        }
    }
}
