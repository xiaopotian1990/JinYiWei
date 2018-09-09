using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
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
    /// 接诊相关接口
    /// </summary>
    public class ReceptionController : ApiController
    {
        private IReceptionService _receptionService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="receptionService"></param>
        public ReceptionController(IReceptionService receptionService)
        {
            _receptionService = receptionService;
        }
        /// <summary>
        /// 获取今日上门记录-接诊工作台
        /// </summary>
        /// <param name="hospitalID">医院ID</param>
        /// <returns></returns>
        public async Task<IFlyDogResult<IFlyDogResultType, ReceptionTodayInfo>> GetReceptionTodayAsync(long hospitalID,long userID)
        {
            return await _receptionService.GetReceptionTodayAsync(hospitalID, userID);
        }
    }
}
