using Com.IFlyDog.CommonDTO;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 症状
    /// </summary>
    public class Symptom
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 症状名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 状态 0：停用；1：使用
        /// </summary>
        public CommonStatus Status { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int SortNo { get; set; }
    }
}
