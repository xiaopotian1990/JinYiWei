using Com.IFlyDog.CommonDTO;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 床位使用或者停用
    /// </summary>
    public class BedStop
    {
        /// <summary>
        /// 床位ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public CommonStatus Status { get; set; }
        
    }
}
