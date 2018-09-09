namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 未成交添加
    /// </summary>
    public class FailtureAddUpdate
    {
        /// <summary>
        /// 未成交记录ID，更新传过来
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
        /// <summary>
        /// 未成交原因，500字最多
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 未成交类型
        /// </summary>
        public long CategoryID { get; set; }
    }
}
