using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.WebAPI.Filters;
using System.Threading.Tasks;
using System.Web.Http;

namespace Com.IFlyDog.FlyDogWebAPI.Controllers
{
    /// <summary>
    /// 积分相关接口
    /// </summary>
    public class PointController : ApiController
    {
        private IPointService _pointService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pointService"></param>
        public PointController(IPointService pointService)
        {
            _pointService = pointService;
        }
        /// <summary>
        /// 获取顾客积分信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, CustomerPointInfo>> GetPointInfo(long customerID)
        {
            return await _pointService.GetPointInfo(customerID);
        }

        /// <summary>
        /// 手动增加扣减积分
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> DeductPoint(DeductPoint dto)
        {
            return await _pointService.DeductPoint(dto);
        }

        /// <summary>
        /// 积分兑换券
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> PointToCoupon(PointToCoupon dto)
        {
            return await _pointService.PointToCoupon(dto);
        }
    }
}
