
namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 回访类型编辑
    /// </summary>
    public class SmartCallbackCategoryUpdate
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
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}
