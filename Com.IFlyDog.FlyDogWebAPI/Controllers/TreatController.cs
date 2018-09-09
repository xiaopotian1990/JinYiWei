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
    /// 治疗预约相关接口
    /// </summary>
    public class TreatController : ApiController
    {
        private ITreatService _treatService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="treatService"></param>
        public TreatController(ITreatService treatService)
        {
            _treatService = treatService;
        }
        /// <summary>
        /// 添加预约
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Add(TreatAdd dto)
        {
            return await _treatService.Add(dto);
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
            return await _treatService.Delete(dto);
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="dto">修改信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Update(TreatUpdate dto)
        {
            return await _treatService.Update(dto);
        }

        /// <summary>
        /// 获取预约详细信息
        /// </summary>
        /// <param name="ID">预约记录ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, TreatDetail>> GetDetail(long ID)
        {
            return await _treatService.GetDetail(ID);
        }

        /// <summary>
        /// 获取预约记录
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<Treat>>> Get(long hospitalID, DateTime startTime, DateTime endTime)
        {
            return await _treatService.Get(hospitalID, startTime, endTime);
        }
    }
}
