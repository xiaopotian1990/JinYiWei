namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 今日分诊列表
    /// </summary>
    public class TriageToday
    {
        /// <summary>
        /// 分诊记录ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 顾客姓名
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 分诊人员
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 分诊时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 接诊人员
        /// </summary>
        public string AssignUserName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
