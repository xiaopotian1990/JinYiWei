using Com.IFlyDog.CommonDTO;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 工具-停用.使用
    /// </summary>
    public class SmartToolStopOrUse
    {
        /// <summary>
        /// 银行卡ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 状态0：停用；1：使用
        /// </summary>
        public CommonStatus Status { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
    }
}