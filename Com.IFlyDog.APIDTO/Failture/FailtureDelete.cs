namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 未成交删除
    /// </summary>
    public class FailtureDelete
    {
        /// <summary>
        /// 记录ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public long CustomerID { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
    }
}
