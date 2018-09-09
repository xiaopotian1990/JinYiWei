using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 订单类型
    /// </summary>
    public enum OrderType
    {
        /// <summary>
        /// 项目订单
        /// </summary>
        [Description("项目订单")]
        Order = 1,
        /// <summary>
        /// 住院单
        /// </summary>
        [Description("住院单")]
        InPatient = 2,
        /// <summary>
        /// 预收款订单
        /// </summary>
        [Description("预收款订单")]
        Deposit = 3,
        /// <summary>
        /// 退单
        /// </summary>
        [Description("退单")]
        Back = 4,
        /// <summary>
        /// 退款
        /// </summary>
        [Description("退款")]
        Refund = 5,
        /// <summary>
        /// 欠款
        /// </summary>
        [Description("欠款")]
        Debt=6
    }
}
