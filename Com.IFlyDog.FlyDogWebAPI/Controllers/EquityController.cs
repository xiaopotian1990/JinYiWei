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
    /// 会员权益相关接口
    /// </summary>
    public class EquityController : ApiController
    {
        private IEquityService _equityService;
        /// <summary>
        /// 构造函数 
        /// </summary>
        /// <param name="equityService"></param>
        public EquityController(IEquityService equityService)
        {
            _equityService = equityService;
        }

        /// <summary>
        /// 添加会员权益
        /// </summary>
        /// <param name="dto">会员权益信息</param>
        /// <returns></returns>
        [ModuleAuthorization("CRM")]
        [HttpPost]
        public IFlyDogResult<IFlyDogResultType, int> Add(EquityAdd dto)
        {
            return _equityService.Add(dto);
        }

        /// <summary>
        /// 更新会员权益信息
        /// </summary>
        /// <param name="dto">会员权益信息</param>
        /// <returns></returns>
        [ModuleAuthorization("CRM")]
        [HttpPost]
        public IFlyDogResult<IFlyDogResultType, int> Update(EquityUpdate dto)
        {
            return _equityService.Update(dto);
        }

        /// <summary>
        /// 会员权益停用
        /// </summary>
        /// <param name="dto">参数集</param>
        /// <returns></returns>
        [ModuleAuthorization("CRM")]
        [HttpPost]
        public IFlyDogResult<IFlyDogResultType, int> StopOrUse(EquityStopOrUse dto)
        {
            return _equityService.StopOrUse(dto);
        }

        /// <summary>
        /// 查询所有会员权益
        /// </summary>
        /// <returns></returns>
        [ModuleAuthorization("CRM")]
        [HttpGet]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Equity>> Get()
        {
            return _equityService.Get();
        }

        /// <summary>
        /// 只查询可用的会员权益
        /// </summary>
        /// <returns></returns>
        [ModuleAuthorization("CRM")]
        [HttpGet]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Equity>> GetStatusIsTrue()
        {
            return _equityService.GetStatusIsTrue();
        }

        /// <summary>
        /// 查询会员权益详细
        /// </summary>
        /// <param name="id">会员权益ID</param>
        /// <returns></returns>
        [ModuleAuthorization("CRM")]
        [HttpGet]
        public IFlyDogResult<IFlyDogResultType, Equity> GetByID(long id)
        {
            return _equityService.GetByID(id);
        }
    }
}
