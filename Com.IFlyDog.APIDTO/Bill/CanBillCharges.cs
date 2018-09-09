using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 可开发票项目
    /// </summary>
    public class CanBillCharges
    {
        /// <summary>
        /// Detail ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 操作日期
        /// </summary>
        public DateTime PaidTime { get; set; }
        /// <summary>
        /// 项目
        /// </summary>
        public string ChargeName { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 成交价
        /// </summary>
        public decimal FinalPrice { get; set; }
        /// <summary>
        /// 券额
        /// </summary>
        public decimal Coupon { get; set; }
        /// <summary>
        /// 佣金额
        /// </summary>
        public decimal Commission { get; set; }
        /// <summary>
        ///退款金额
        /// </summary>
        public decimal RebateAmount { get; set; }
        /// <summary>
        /// 现金额
        /// </summary>
        public decimal RealAmount { get; set; }
    }
}
