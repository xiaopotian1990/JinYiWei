using System.ComponentModel;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 券活动状态
    /// </summary>
    public enum CouponActiveStatus
    {
        /// <summary>
        /// 不可重复使用
        /// </summary>
        [Description("不可重复使用")]
        NoReception = 0,
        /// <summary>
        /// 可重复使用
        /// </summary>
        [Description("可重复使用")]
        CanReception = 1,
        /// <summary>
        /// 有效
        /// </summary>
        [Description("有效")]
        Effective = 2,
        /// <summary>
        /// 失效
        /// </summary>
        [Description("失效")]
        NoEffective = 3
    }
}
