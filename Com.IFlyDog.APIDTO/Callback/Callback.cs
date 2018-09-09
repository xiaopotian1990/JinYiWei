using Com.IFlyDog.CommonDTO;
using System;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 回访内容
    /// </summary>
    public class Callback
    {
        /// <summary>
        /// 回访记录ID
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
        /// 创建回访提醒的用户
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 回访提醒创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 沟通方式
        /// </summary>
        public string Tool { get; set; }
        /// <summary>
        /// 回访内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 回访类型
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 回访计划
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 回访提醒人
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 回访提醒日期，具体到天
        /// </summary>
        public DateTime? TaskTime { get; set; }
        /// <summary>
        /// 回访提醒完成时间
        /// </summary>
        public DateTime? TaskCreateTime { get; set; }
        /// <summary>
        /// 回访提醒完成人
        /// </summary>
        public string TaskCreateUser { get; set; }
        /// <summary>
        /// 回访状态
        /// </summary>
        public CallbackStatus Status { get; set; }
    }

    /// <summary>
    /// 回放选择条件
    /// </summary>
    public class CallbackSelect
    {
        /// <summary>
        /// 登录用户ID
        /// </summary>
        public long LoginUserID { get; set; }
        /// <summary>
        /// 回访开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 回放结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 回访类型，全部传-1
        /// </summary>
        public long CategoryID { get; set; }
        /// <summary>
        /// 回放状态
        /// </summary>
        public CallbackStatus Status { get; set; }
        /// <summary>
        /// 回访提醒人,不传为0
        /// </summary>
        public long UserID { get; set; }
        /// <summary>
        /// 顾客ID,不传为0
        /// </summary>
        public long CustomerID { get; set; }
        /// <summary>
        /// 回访计划
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 第几页
        /// </summary>
        public int PageNum { get; set; }
        /// <summary>
        /// 每页多少
        /// </summary>
        public int PageSize { get; set; }
    }
}
