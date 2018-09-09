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
    /// 发票相关接口
    /// </summary>
    public class BillController : ApiController
    {
        private IBillService _billService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="billService"></param>
        public BillController(IBillService billService)
        {
            _billService = billService;
        }

        /// <summary>
        /// 查询可开发票项目
        /// </summary>
        /// <param name="customerID">顾客ID</param>
        /// <param name="hospitalID">医院ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<CanBillCharges>>> GetCanBillCharges(long customerID, long hospitalID)
        {
            return await _billService.GetCanBillCharges(customerID, hospitalID);
        }

        /// <summary>
        /// 添加发票
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Add(BillAdd dto)
        {
            return await _billService.Add(dto);
        }

        /// <summary>
        /// 删除发票
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, int>> Delete(BillDelete dto)
        {
            return await _billService.Delete(dto);
        }

        /// <summary>
        /// 获取今日发票记录
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<Bill>>> GetBillToday(long hospitalID)
        {
            return await _billService.GetBillToday(hospitalID);
        }
    }
}
