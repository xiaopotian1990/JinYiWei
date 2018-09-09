namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 回访删除（包括提醒）
    /// </summary>
    public class CallbackDelete
    {
        /// <summary>
        /// 顾客ID
        /// </summary>
        public long CustomerID { get; set; }
        /// <summary>
        /// 用户ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 回访ID
        /// </summary>
        public long CallbackID { get; set; }
    }
}
