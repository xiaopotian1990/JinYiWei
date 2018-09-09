namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 渠道修改信息
    /// </summary>
    public class ChannelUpdate
    {
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 渠道ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 渠道名称，1-20个字之间
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public string SortNo { get; set; }
        /// <summary>
        /// 备注，0-50个字之间
        /// </summary>
        public string Remark { get; set; }
    }
}
