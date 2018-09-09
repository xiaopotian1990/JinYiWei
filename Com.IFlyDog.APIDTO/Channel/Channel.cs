using Com.IFlyDog.CommonDTO;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 渠道信息
    /// </summary>
    public class Channel
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 渠道名称
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
