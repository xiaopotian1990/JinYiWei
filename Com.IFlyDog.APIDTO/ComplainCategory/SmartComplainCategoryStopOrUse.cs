using Com.IFlyDog.CommonDTO;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 未成交使用停用
    /// </summary>
    public class SmartComplainCategoryStopOrUse
    {
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 部门ID
        /// </summary>
        public long ComplainID { get; set; }
        /// <summary>
        /// 状态 1：使用；0：停用；2：删除
        /// </summary>
        public CommonStatus Status { get; set; }
    }
}
