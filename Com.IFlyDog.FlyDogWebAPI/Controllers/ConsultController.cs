using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.WebAPI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Com.IFlyDog.FlyDogWebAPI.Controllers
{
    /// <summary>
    /// 咨询相关API
    /// </summary>
    public class ConsultController : ApiController
    {
        private IConsultService _consultService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="consultService"></param>
        public ConsultController(IConsultService consultService)
        {
            _consultService = consultService;
        }

        /// <summary>
        /// 获取顾客咨列表
        /// </summary>
        /// <param name="customerID">顾客ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<Consult>>> GetConsult(long customerID)
        {
            return await _consultService.GetConsult(customerID);
        }

        /// <summary>
        /// 添加咨询
        /// </summary>
        /// <param name="dto">咨询内容</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> ConsultAdd(ConsultAddUpdate dto)
        {
            return await _consultService.ConsultAdd(dto);
        }

        /// <summary>
        /// 咨询修改
        /// </summary>
        /// <param name="dto">咨询内容</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> ConsultUpdate(ConsultAddUpdate dto)
        {
            return await _consultService.ConsultUpdate(dto);
        }

        /// <summary>
        /// 咨询删除
        /// </summary>
        /// <param name="dto">删除信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> ConsultDelete(ConsultDelete dto)
        {
            return await _consultService.ConsultDelete(dto);
        }

        /// <summary>
        /// 获取咨询详细信息
        /// </summary>
        /// <param name="ID">咨询记录ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, ConsultDetail>> GetConsultDetail(long ID)
        {
            return await _consultService.GetConsultDetail(ID);
        }
    }
}
