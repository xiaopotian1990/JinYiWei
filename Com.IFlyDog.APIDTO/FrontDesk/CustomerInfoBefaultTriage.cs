namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 分诊时获取的顾客信息
    /// </summary>
    public class CustomerInfoBefaultTriage
    {
        /// <summary>
        /// 顾客姓名
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 咨询项目
        /// </summary>
        public string Symptom { get; set; }
        /// <summary>
        /// 归属咨询师
        /// </summary>
        public string ManagerUserName { get; set; }
        /// <summary>
        /// 归属咨询师ID
        /// </summary>
        public string ManagerUserID { get; set; }
    }
}
