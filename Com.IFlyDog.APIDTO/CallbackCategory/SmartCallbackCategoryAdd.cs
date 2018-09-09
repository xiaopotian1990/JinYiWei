
namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 回访类型添加
    /// </summary>
    public class SmartCallbackCategoryAdd
    {
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
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
