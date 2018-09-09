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
    /// 住院相关API接口
    /// </summary>
    public class InpatientController : ApiController
    {
        private IInpatientService _inpatientService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="inpatientService"></param>
        public InpatientController(IInpatientService inpatientService)
        {
            _inpatientService = inpatientService;
        }
        /// <summary>
        /// 住院
        /// </summary>
        /// <param name="dto">住院信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> In(InpatientAdd dto)
        {
            return await _inpatientService.In(dto);
        }

        /// <summary>
        /// 出院
        /// </summary>
        /// <param name="dto">出院信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Out(Inpatientout dto)
        {
            return await _inpatientService.Out(dto);
        }

        /// <summary>
        /// 住院工作台住院列表
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<InpatientIn>>> GetIn(long hospitalID)
        {
            return await _inpatientService.GetIn(hospitalID);
        }
    }
}
