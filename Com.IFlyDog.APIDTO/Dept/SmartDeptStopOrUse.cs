using Com.IFlyDog.CommonDTO;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 停用或者使用部门
    /// </summary>
    public class SmartDeptStopOrUse
    {
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 部门ID
        /// </summary>
        public long DeptID { get; set; }
        /// <summary>
        /// 部门所属医院ID
        /// </summary>
        public long HospitalID { get; set; }
        /// <summary>
        /// 操作人所在医院ID
        /// </summary>
        public long UserHospitalID { get; set; }
        /// <summary>
        /// 状态 1：使用；0：停用；2：删除
        /// </summary>
        public CommonStatus OpenStatus { get; set; }
    }
}
