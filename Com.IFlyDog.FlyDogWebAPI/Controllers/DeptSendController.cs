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
    /// 科室发料相关
    /// </summary>
    public class DeptSendController : ApiController
    {
        private IDeptSendService _deptSendService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="deptSendService"></param>
        public DeptSendController(IDeptSendService deptSendService)
        {
            _deptSendService = deptSendService;
        }
        /// <summary>
        /// 科室发料请求
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<DeptSendInfo>>> GetDeptSendInfo(long hospitalID, long userID)
        {
            return await _deptSendService.GetDeptSendInfo(hospitalID, userID);
        }

        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Send(DeptSendAdd dto)
        {
            return await _deptSendService.Send(dto);
        }

        /// <summary>
        /// 今日发货记录
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<DeptSend>>> GetDeptSendToday(long hospitalID, long userID)
        {
            return await _deptSendService.GetDeptSendToday(hospitalID, userID);
        }
    }
}
