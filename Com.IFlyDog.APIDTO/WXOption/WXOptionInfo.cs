using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 微信系统设置查询
    /// </summary>
   public class WXOptionInfo
    {

        /// <summary>
        /// code值
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 设置值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 被推荐用户送卷Code
        /// </summary>
        public string SendVolumeCode { get; set; }
        /// <summary>
        /// 被推荐用户送卷是否开启Value
        /// </summary>
        public string SendVolumeValue { get; set; }
        /// <summary>
        /// 被推荐用户送券，卷类型Code
        /// </summary>
        public string UserCouponCategoryCode { get; set; }

        /// <summary>
        /// 被推荐用户送券，卷类型value
        /// </summary>
        public string UserCouponCategoryValue { get; set; }

        /// <summary>
        /// 被推荐用户送券，卷类型名称
        /// </summary>
        public string UserCouponCategoryName { get; set; }

        /// <summary>
        /// 被推荐用户送券金额Code　
        /// </summary>
        public string CouponCategoryMoneyCode { get; set; }

        /// <summary>
        /// 被推荐用户送券金额value
        /// </summary>
        public string CouponCategoryMoneyValue { get; set; }

        /// <summary>
        /// 推荐时限 天数Code
        /// </summary>
        public string RecommendNumberDayCode { get; set; }

        /// <summary>
        /// 推荐时限 天数value
        /// </summary>
        public string RecommendNumberDayValue { get; set; }

        /// <summary>
        /// 不提点折扣小于Code
        /// </summary>
        public string NoDiscountCode { get; set; }
        /// <summary>
        /// 不提点折扣小于Value
        /// </summary>
        public string NoDiscountValue { get; set; }
        /// <summary>
        /// 是否开启佣金提成Code
        /// </summary>
        public string OpenCommissionCode { get; set; }
        /// <summary>
        /// 是否开启佣金提成Value
        /// </summary>
        public string OpenCommissionValue { get; set; }
        /// <summary>
        /// 佣金提成等级code
        /// </summary>
        public string CommissionLevelCode { get; set; }

        /// <summary>
        /// 佣金提成等级Value
        /// </summary>
        public string CommissionLevelValue { get; set; }
        /// <summary>
        /// 佣金提成提点code
        /// </summary>
        public string CommissionRemindedCode { get; set; }

        /// <summary>
        /// 佣金提成提点value
        /// </summary>
        public string CommissionRemindedValue { get; set; }

        /// <summary>
        /// 默认渠道code
        /// </summary>
        public string ChannelCode { get; set; }
        /// <summary>
        /// 默认渠道值
        /// </summary>
        public string ChannelValue { get; set; }

        /// <summary>
        /// 级别提点
        /// </summary>
        public List<PromoteLevelInfo> PromoteLevelList { get; set; }

        /// <summary>
        /// 特殊渠道信息
        /// </summary>
        public List<ChannelInfo> ChannelInfoList { get; set; }
    }

    /// <summary>
    /// 级别提点
    /// </summary>
    public class PromoteLevelInfo {
        /// <summary>
        /// 级别提点id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 级别
        /// </summary>
        public string Level { get; set; }
        /// <summary>
        /// 提点
        /// </summary>
        public string Rate { get; set; }
    }

    /// <summary>
    /// 特殊渠道信息
    /// </summary>
    public class ChannelInfo {
        /// <summary>
        /// 渠道id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 渠道名称
        /// </summary>
        public string Name { get; set; }
    }
}
