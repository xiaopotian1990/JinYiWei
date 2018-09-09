using Com.IFlyDog.CommonDTO;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 部门更新
    /// </summary>
    public class SmartDeptUpdate
    {
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 部门名称
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
        /// 部门所属医院ID
        /// </summary>
        public long HospitalID { get; set; }
        /// <summary>
        /// 操作人所在医院ID
        /// </summary>
        public long UserHospitalID { get; set; }
    }
}
