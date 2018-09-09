using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 系统设置枚举
    /// </summary>
    public enum WXOption
    {
        /// <summary>
        /// 被推荐用户送券Code
        /// </summary>
        UserSendVolumeCode = 24,
        /// <summary>
        /// 被推荐用户送券，卷类型Code
        /// </summary>
        UserCouponCategoryCode = 25,
        /// <summary>
        /// 被推荐用户送券金额Code
        /// </summary>
        CouponCategoryMoneyCode = 26,
        /// <summary>
        /// 推荐时限 天数Code
        /// </summary>
        RecommendNumberDayCode = 27,
        /// <summary>
        /// 不提点折扣小于Code
        /// </summary>
        NoDiscountCode = 28,

        /// <summary>
        /// 是否开启佣金提成Code
        /// </summary>
        OpenCommissionCode = 29,

        /// <summary>
        /// 佣金提成等级Code
        /// </summary>
        CommissionLevelCode = 30,
        /// <summary>
        /// 佣金提成提点Code
        /// </summary>
        CommissionRemindedCode = 31,

        /// <summary>
        /// 默认渠道Code
        /// </summary>
        ChannelCode = 32
    }

    /// <summary>
    /// 是否送卷
    /// </summary>
    public enum SendVolume
    {
        /// <summary>
        /// 开启
        /// </summary>
        Open = 1,
        /// <summary>
        /// 关闭
        /// </summary>
        Close = 0
    }

    /// <summary>
    /// 是否开启佣金提成
    /// </summary>
    public enum OpenCommission
    {
        /// <summary>
        /// 开启
        /// </summary>
        Open = 1,
        /// <summary>
        /// 关闭
        /// </summary>
        Close = 0
    }
}
