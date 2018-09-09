namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 更新未成交类型
    /// </summary>
    public class FailtureCategoryUpdate
    {
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 未成交类型ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 未成交类型名称，1-20个字之间
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 备注，0-50个字之间
        /// </summary>
        public string Remark { get; set; }
    }
}
