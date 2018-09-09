using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 预收款类型
    /// </summary>
    public enum DepositType
    {
        /// <summary>
        /// 订单购买
        /// </summary>
        [Description("订单购买")]
        Deposit = 1,
        /// <summary>
        /// 退项目获得
        /// </summary>
        [Description("退项目获得")]
        Back = 2,
        /// <summary>
        /// 退款获得
        /// </summary>
        [Description("退款获得")]
        Refund = 3,
        /// <summary>
        /// 转让
        /// </summary>
        [Description("转让")]
        Transfer = 4,
        /// <summary>
        /// 消费使用
        /// </summary>
        [Description("消费使用")]
        Consume = 5
    }
}
