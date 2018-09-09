using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 欠款订单
    /// </summary>
    public class DebtOrders
    {
        /// <summary>
        /// 订单ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 订单提交时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 顾客
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 订单提交人
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 订单总额
        /// </summary>
        public decimal FinalPrice { get; set; }
        /// <summary>
        /// 已付金额
        /// </summary>
        public decimal RealAmount { get; set; }
        /// <summary>
        /// 欠款金额
        /// </summary>
        public decimal DebtAmount { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>
        public OrderType OrderType { get; set; }
    }

    /// <summary>
    /// 欠款订单查询选择
    /// </summary>
    public class DebtSelect
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
        /// 顾客ID
        /// </summary>
        public long CustomerID { get; set; }
        /// <summary>
        /// 医院ID
        /// </summary>
        public long HospitalID { get; set; }
    }
}
