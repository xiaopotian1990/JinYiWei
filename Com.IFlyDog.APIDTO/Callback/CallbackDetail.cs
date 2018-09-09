namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 详细信息
    /// </summary>
    public class CallbackDetail
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 顾客编号
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 顾客姓名
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 备用联系方式
        /// </summary>
        public string MobileBackup { get; set; }
        /// <summary>
        /// 渠道名称
        /// </summary>
        public string ChannelName { get; set; }
        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 回访类型
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 回访工具
        /// </summary>
        public string Tool { get; set; }
    }
}
