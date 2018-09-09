using System;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 回访计划添加
    /// </summary>
    public class CallbackPlanAdd
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
        /// 回访计划ID
        /// </summary>
        public long SetID { get; set; }
        /// <summary>
        /// 回放日期，具体到天
        /// </summary>
        public DateTime TaskTime { get; set; }
    }
}
