namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 未成交详细
    /// </summary>
    public class FailtureDetail
    {
        /// <summary>
        /// 未成交记录ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 未成交原因，500字最多
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 未成交类型
        /// </summary>
        public string CategoryID { get; set; }
    }
}
