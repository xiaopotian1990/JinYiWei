using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 券获得途径
    /// </summary>
    public enum CouponType
    {
        /// <summary>
        /// 预收款赠送
        /// </summary>
        [Description("预收款赠送")]
        DepositSend = 1,
        /// <summary>
        /// 手工赠送
        /// </summary>
        [Description("手工赠送")]
        ManualSend = 2,
        /// <summary>
        /// 积分兑换
        /// </summary>
        [Description("积分兑换")]
        PointChange = 3,
        /// <summary>
        /// 退款退得
        /// </summary>
        [Description("退款退得")]
        BackSed = 4,
        /// <summary>
        /// 激活码兑换
        /// </summary>
        [Description("激活码兑换")]
        CodeActive =5,
        /// <summary>
        /// 消费扣减
        /// </summary>
        [Description("消费扣减")]
        OrderUse = 6,
        /// <summary>
        /// 手工扣减
        /// </summary>
        [Description("手工扣减")]
        ManualDelete = 7,
        /// <summary>
        /// 退款扣减
        /// </summary>
        [Description("退款扣减")]
        DepositRebateDelete = 8,
        /// <summary>
        /// 推荐赠送
        /// </summary>
        [Description("推荐赠送")]
        PromoteSend = 9,
    }

    /// <summary>
    /// 券使用类型
    /// </summary>
    public enum CouponUsageType
    {
        /// <summary>
        /// 消费扣减
        /// </summary>
        [Description("消费扣减")]
        OrderUse = 1,
        /// <summary>
        /// 手工扣减
        /// </summary>
        [Description("手工扣减")]
        ManualDelete = 2,
        /// <summary>
        /// 退款扣减
        /// </summary>
        [Description("退款扣减")]
        DepositRebateDelete = 3,
    }
}
