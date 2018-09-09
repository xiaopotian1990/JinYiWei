using Com.IFlyDog.CommonDTO;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 部门（添加）
    /// </summary>
    public class SmartDeptAdd
    {
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 开放状态 1 是 0 否
        /// </summary>
        public CommonStatus OpenStatus { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int SortNo { get; set; }
        /// <summary>
        /// 所属医院ID
        /// </summary>
        public long HospitalID { get; set; }
        /// <summary>
        /// 操作人所在医院ID
        /// </summary>
        public long UserHospitalID { get; set; }

    }

    /// <summary>
    /// 是否治疗部门
    /// </summary>
    public enum DeptStatus
    {
        /// <summary>
        /// 治疗部门
        /// </summary>
        Treat=1,
        /// <summary>
        /// 普通部门
        /// </summary>
        Normal=0
    }
}
