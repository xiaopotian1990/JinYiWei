namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 修改投诉
    /// </summary>
    public class SmartComplainCategoryUpdate
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
        /// 投诉名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
  
    }
}
