using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 订单付款类型
    /// </summary>
    public enum PaidStatus
    {
        /// <summary>
        /// 未付款
        /// </summary>
        [Description("未付款")]
        NotPaid = 1,
        /// <summary>
        /// 已付款
        /// </summary>
        [Description("已付款")]
        Paid = 2,
        /// <summary>
        /// 欠款
        /// </summary>
        [Description("欠款")]
        Debt = 3,
        /// <summary>
        /// 取消
        /// </summary>
        [Description("取消")]
        Cancel = 4
    }
}
