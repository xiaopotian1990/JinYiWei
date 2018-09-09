using System.ComponentModel;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 上门状态
    /// </summary>
    public enum VisitType
    {
        /// <summary>
        /// 初诊
        /// </summary>
        [Description("初诊")]
        First = 1,
        /// <summary>
        /// 复诊
        /// </summary>
        [Description("复诊")]
        Twice = 2,
        /// <summary>
        /// 成交未分诊 复查
        /// </summary>
        [Description("复查")]
        Check = 3,
        /// <summary>
        /// 成交后再分诊 再消费
        /// </summary>
        [Description("再消费")]
        Again = 4
    }

    /// <summary>
    /// 顾客状态
    /// </summary>
    public enum CustomerType
    {
        /// <summary>
        /// 新客
        /// </summary>
        [Description("新客")]
        New = 1,
        /// <summary>
        /// 老客
        /// </summary>
        [Description("老客")]
        Old = 2,
    }
}
