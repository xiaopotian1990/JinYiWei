using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 积分消费方式
    /// </summary>
    public enum PointType
    {
        /// <summary>
        /// 手工赠送
        /// </summary>
        [Description("手工赠送")]
        ManualGive = 11,
        /// <summary>
        /// 消费赠送
        /// </summary>
        [Description("消费赠送")]
        ConsumeGive = 12,
        /// <summary>
        /// 手工扣减
        /// </summary>
        [Description("手工扣减")]
        ManualRebate = 21,
        /// <summary>
        /// 兑换券
        /// </summary>
        [Description("兑换券")]
        CouponExchange = 22,
        /// <summary>
        /// 兑换产品
        /// </summary>
        [Description("兑换产品")]
        ChargeExchange = 23,
        /// <summary>
        /// 退项目扣减
        /// </summary>
        [Description("退项目扣减")]
        BackRebate = 24,
        /// <summary>
        /// 退款扣减
        /// </summary>
        [Description("退款扣减")]
        RebateRebate = 25
    }
}
