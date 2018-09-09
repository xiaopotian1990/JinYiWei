using System;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 回访提醒更新
    /// </summary>
    public class CallbackRemindUpdate
    {
        /// <summary>
        /// 回访提醒ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 操作用户ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 回访人员
        /// </summary>
        public long UserID { get; set; }
        /// <summary>
        /// 回访类型
        /// </summary>
        public long CategoryID { get; set; }
        /// <summary>
        /// 回放日期，具体到天
        /// </summary>
        public DateTime TaskTime { get; set; }
        /// <summary>
        /// 回访计划
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public long CustomerID { get; set; }
    }
}
