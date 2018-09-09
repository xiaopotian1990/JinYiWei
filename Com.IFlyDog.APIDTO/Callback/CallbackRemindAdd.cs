using System;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 回访提醒
    /// </summary>
    public class CallbackRemindAdd
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
    }

    /// <summary>
    /// 修改回放之前获取回访提醒详细
    /// </summary>
    public class CallbackRemindDetail
    {
        /// <summary>
        /// 回访提醒ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 回访人员ID
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 回访人员姓名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 回访类型ID
        /// </summary>
        public string CategoryID { get; set; }
        /// <summary>
        /// 回访类型
        /// </summary>
        public string CategoryName { get; set; }
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
        public string CustomerID { get; set; }
    }
}
