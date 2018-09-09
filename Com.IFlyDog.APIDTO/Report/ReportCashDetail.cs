using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 现金流明细
    /// </summary>
    public class ReportCashDetail
    {
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 客户编号	
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 顾客姓名
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 医院
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 收银员	
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 订单类型	
        /// </summary>
        public OrderType OrderType { get; set; }
        /// <summary>
        /// 收银单号	
        /// </summary>
        public string No { get; set; }
        /// <summary>
        /// 报表项目组	
        /// </summary>
        public string ItemGroup { get; set; }
        /// <summary>
        /// 报表项目	
        /// </summary>
        public string Item { get; set; }
        /// <summary>
        /// 项目
        /// </summary>
        public string ChargeName { get; set; }
        /// <summary>
        /// 收款方式	
        /// </summary>
        public string CashierCategory { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
    }

    /// <summary>
    /// 选择
    /// </summary>
    public class ReportCashDetailSelect
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
        /// 收银员ID
        /// </summary>
        public long? CreateUserID { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public long? CustomerID { get; set; }
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
