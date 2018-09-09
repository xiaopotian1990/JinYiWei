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
    /// 未成交相关API
    /// </summary>
    public class FailtureController : ApiController
    {
        private IFailtureService _failtureService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="failtureService"></param>
        public FailtureController(IFailtureService failtureService)
        {
            _failtureService = failtureService;
        }

        /// <summary>
        /// 获取顾客未成交列表
        /// </summary>
        /// <param name="customerID">顾客ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<Failture>>> GetByCustomerID(long customerID)
        {
            return await _failtureService.GetByCustomerID(customerID);
        }

        /// <summary>
        /// 获取未成交详细信息
        /// </summary>
        /// <param name="ID">咨记录ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, FailtureDetail>> GetDetail(long ID)
        {
            return await _failtureService.GetDetail(ID);
        }

        /// <summary>
        /// 未成交修改
        /// </summary>
        /// <param name="dto">未成交信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Update(FailtureAddUpdate dto)
        {
            return await _failtureService.Update(dto);
        }

        /// <summary>
        /// 添加未成交
        /// </summary>
        /// <param name="dto">未成交信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Add(FailtureAddUpdate dto)
        {
            return await _failtureService.Add(dto);
        }

        /// <summary>
        /// 未成交删除
        /// </summary>
        /// <param name="dto">删除信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Delete(FailtureDelete dto)
        {
            return await _failtureService.Delete(dto);
        }
    }
}
