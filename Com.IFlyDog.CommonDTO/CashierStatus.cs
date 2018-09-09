using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 订单结算状态
    /// </summary>
    public enum CashierStatus
    {
        /// <summary>
        /// 已经结算
        /// </summary>
        [Description("已经结算")]
        Yes = 1,
        /// <summary>
        /// 未结算
        /// </summary>
        [Description("未结算")]
        No = 0
    }
}
