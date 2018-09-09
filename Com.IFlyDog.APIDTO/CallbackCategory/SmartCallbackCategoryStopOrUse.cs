using Com.IFlyDog.CommonDTO;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 回访类型状态停用
    /// </summary>
    public class SmartCallbackCategoryStopOrUse
    {
        /// <summary>
        /// ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// ID
        /// </summary>
        public long CallbackID { get; set; }
        /// <summary>
        /// 状态 1：使用；0：停用；2：删除
        /// </summary>
        public CommonStatus Status { get; set; }
    }
}
