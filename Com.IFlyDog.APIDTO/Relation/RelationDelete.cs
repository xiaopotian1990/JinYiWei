namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 删除关系管理
    /// </summary>
    public class RelationDelete
    {
        /// <summary>
        /// 主键
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
    }
}
