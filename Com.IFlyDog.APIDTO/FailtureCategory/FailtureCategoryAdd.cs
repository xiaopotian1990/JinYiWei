namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 添加未成交类型
    /// </summary>
    public class FailtureCategoryAdd
    {
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
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
