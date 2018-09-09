using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 还款明细
    /// </summary>
    public class ReportDebtCashier
    {
        /// <summary>
        /// 还款时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 客户编号	
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 客户姓名
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 还款医院
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 收银员
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 收银单号	
        /// </summary>
        public string No { get; set; }
        /// <summary>
        /// 欠款订单号	
        /// </summary>
        public string OrderID { get; set; }
        /// <summary>
        /// 应还金额	
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 现金	
        /// </summary>
        public decimal Cash { get; set; }
        /// <summary>
        /// 刷卡
        /// </summary>
        public decimal Card { get; set; }
        /// <summary>
        /// 合计还款	
        /// </summary>
        public decimal RealAmount { get; set; }
        /// <summary>
        /// 剩余欠款	
        /// </summary>
        public decimal Debt { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    /// <summary>
    /// 还款日合计
    /// </summary>
    public class ReportDebtCashierDay
    {
        /// <summary>
        /// 医院
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 还款日期	
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 还款数量	
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 还款客户数量	
        /// </summary>
        public int CustomerNum { get; set; }
        /// <summary>
        /// 现金合计	
        /// </summary>
        public decimal Cash { get; set; }
        /// <summary>
        /// 刷卡合计	
        /// </summary>
        public decimal Card { get; set; }
        /// <summary>
        /// 还款总额合计
        /// </summary>
        public decimal DealAmount { get; set; }
    }

    /// <summary>
    /// 集团还款明细Select
    /// </summary>
    public class ReportDebtCashierSelect
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 医院ID
        /// </summary>
        public long? HospitalID { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public long? CustomerID { get; set; }
        /// <summary>
        /// 订单状态
        /// </summary>
        public OrderType OrderType { get; set; }
        /// <summary>
        /// 当前分页
        /// </summary>
        public int PageNum { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }
    }
}
