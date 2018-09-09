namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 投诉类型 添加
    /// </summary>
    public class SmartComplainCategoryAdd
    {
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 投诉名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 投诉备注
        /// </summary>
        public string Remark { get; set; }
    }
}
