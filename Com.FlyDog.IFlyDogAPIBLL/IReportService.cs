using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    public interface IReportService
    {
        /// <summary>
        /// 未成交报表
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<ReportFailture>>>> FailturePages(ReportFailtureSelect dto);
        /// <summary>
        /// 未成交报表，导出Excel
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<ReportFailture>>> Failture(ReportFailtureSelect dto);

        /// <summary>
        /// 未成交类型统计
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<ReportFailtureCount>>> FailtureCount(ReportFailtureSelect dto);

        /// <summary>
        /// 集团还款明细
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<ReportDebtCashier>>>> DebtCashierPages(ReportDebtCashierSelect dto);

        /// <summary>
        /// 集团还款明细，导出Excel
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<ReportDebtCashier>>> DebtCashier(ReportDebtCashierSelect dto);

        /// <summary>
        /// 还款日合计
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<ReportDebtCashierDay>>> DebtCashierDay(ReportDebtCashierSelect dto);

        /// <summary>
        /// 集团还款各医院统计
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<ReportDebtCashierDay>>> DebtCashierDayByHospital(ReportDebtCashierSelect dto);

        /// <summary>
        /// 工作量明细
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<ReportOperation>>>> OperationPages(ReportOperationSelect dto);

        /// <summary>
        /// 工作量明细，导出Excel
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<ReportOperation>>> Operation(ReportOperationSelect dto);

        /// <summary>
        /// 年龄明细
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<ReportAge>>>> AgePages(ReportAgeSelect dto);

        /// <summary>
        /// 年龄明细，导出Excel
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<ReportAge>>> Age(ReportAgeSelect dto);
    }
}
