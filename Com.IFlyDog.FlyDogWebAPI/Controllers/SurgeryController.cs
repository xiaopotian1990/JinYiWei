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
    /// 手术预约相关接口
    /// </summary>
    public class SurgeryController : ApiController
    {
        private ISurgeryService _surgeryService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="surgeryService">服务</param>
        public SurgeryController(ISurgeryService surgeryService)
        {
            _surgeryService = surgeryService;
        }

        /// <summary>
        /// 添加预约
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Add(SurgeryAdd dto)
        {
            return await _surgeryService.Add(dto);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dto">删除信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Delete(SurgeryDelete dto)
        {
            return await _surgeryService.Delete(dto);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="dto">修改信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Update(SurgeryUpdate dto)
        {
            return await _surgeryService.Update(dto);
        }

        /// <summary>
        /// 获取预约详细信息
        /// </summary>
        /// <param name="ID">预约记录ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, SurgeryDetail>> GetDetail(long ID)
        {
            return await _surgeryService.GetDetail(ID);
        }

        /// <summary>
        /// 手术排台
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<Surgery>>> Get(long hospitalID, DateTime date)
        {
            return await _surgeryService.Get(hospitalID, date);
        }

        /// <summary>
        /// 开始结束手术
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Done(SugeryDone dto)
        {
            return await _surgeryService.Done(dto);
        }
    }
}
