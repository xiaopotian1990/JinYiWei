namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 症状添加
    /// </summary>
    public class SymptomAdd
    {
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 症状名称，1-20个字之间
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
