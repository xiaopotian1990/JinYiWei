using Com.IFlyDog.CommonDTO;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 未成交类型使用或者停用
    /// </summary>
    public class FailtureCategoryStopOrUse
    {
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        ///  未成交类型ID
        /// </summary>
        public long FailtureCategoryID { get; set; }
        /// <summary>
        /// 状态 0：停用；1：使用
        /// </summary>
        public CommonStatus Status { get; set; }
    }
}
