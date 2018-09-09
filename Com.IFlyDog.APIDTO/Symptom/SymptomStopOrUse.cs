using Com.IFlyDog.CommonDTO;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 症状使用或停用
    /// </summary>
    public class SymptomStopOrUse
    {
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 症状ID
        /// </summary>
        public long SymptomID { get; set; }
        /// <summary>
        /// 状态 0：停用；1：使用
        /// </summary>
        public CommonStatus Status { get; set; }
    }
}
