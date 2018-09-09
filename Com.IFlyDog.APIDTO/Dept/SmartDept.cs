using Com.IFlyDog.CommonDTO;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 部门信息
    /// </summary>
    public class SmartDept
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }
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
        /// 所属医院ID
        /// </summary>
        public string HospitalID { get; set; }
        /// <summary>
        /// 所属医院
        /// </summary>
        public string HospitalName { get; set; }
    }
}
