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
    /// 报表相关接口
    /// </summary>
    public class ReportController : ApiController
    {
        #region 构造函数
        private IReportService _reportService;
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="reportService"></param>
        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }
        #endregion 

        #region 未成交
        /// <summary>
        /// 未成交报表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<ReportFailture>>>> FailturePages(ReportFailtureSelect dto)
        {
            return await _reportService.FailturePages(dto);
        }
        /// <summary>
        /// 未成交报表，导出Excel
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<ReportFailture>>> Failture(ReportFailtureSelect dto)
        {
            return await _reportService.Failture(dto);
        }

        /// <summary>
        /// 未成交类型统计
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<ReportFailtureCount>>> FailtureCount(ReportFailtureSelect dto)
        {
            return await _reportService.FailtureCount(dto);
        }
        #endregion

        #region 还款相关
        /// <summary>
        /// 集团还款明细
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<ReportDebtCashier>>>> DebtCashierPages(ReportDebtCashierSelect dto)
        {
            return await _reportService.DebtCashierPages(dto);
        }

        /// <summary>
        /// 集团还款明细，导出Excel
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<ReportDebtCashier>>> DebtCashier(ReportDebtCashierSelect dto)
        {
            return await _reportService.DebtCashier(dto);
        }

        /// <summary>
        /// 还款日合计
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<ReportDebtCashierDay>>> DebtCashierDay(ReportDebtCashierSelect dto)
        {
            return await _reportService.DebtCashierDay(dto);
        }

        /// <summary>
        /// 集团还款各医院统计
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<ReportDebtCashierDay>>> DebtCashierDayByHospital(ReportDebtCashierSelect dto)
        {
            return await _reportService.DebtCashierDayByHospital(dto);
        }
        #endregion

        #region 工作量明细
        /// <summary>
        /// 工作量明细
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<ReportOperation>>>> OperationPages(ReportOperationSelect dto)
        {
            return await _reportService.OperationPages(dto);
        }

        /// <summary>
        /// 工作量明细，导出Excel
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<ReportOperation>>> Operation(ReportOperationSelect dto)
        {
            return await _reportService.Operation(dto);
        }
        #endregion

        #region 年龄
        /// <summary>
        /// 年龄明细
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<ReportAge>>>> AgePages(ReportAgeSelect dto)
        {
            return await _reportService.AgePages(dto);
        }

        /// <summary>
        /// 年龄明细，导出Excel
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<ReportAge>>> Age(ReportAgeSelect dto)
        {
            return await _reportService.Age(dto);
        }
        #endregion
    }
}
