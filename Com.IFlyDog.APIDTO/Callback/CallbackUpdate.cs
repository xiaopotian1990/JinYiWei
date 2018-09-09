using System;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 修改回访
    /// </summary>
    public class CallbackUpdate
    {
        /// <summary>
        /// 访问记录ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 操作用户ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 沟通工具
        /// </summary>
        public long Tool { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public long CustomerID { get; set; }
    }

    /// <summary>
    /// 回放详细
    /// </summary>
    public class CallbackUpdateDetail
    {
        /// <summary>
        /// 访问记录ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 回访类型
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 回访类型ID
        /// </summary>
        public string CategoryID { get; set; }
        /// <summary>
        /// 回访完成时间
        /// </summary>
        public DateTime TaskCreateTime { get; set; }
        /// <summary>
        /// 回访人ID
        /// </summary>
        public string TaskCreateUserID { get; set; }
        /// <summary>
        /// 回访人
        /// </summary>
        public string TaskCreateUserName { get; set; }
        /// <summary>
        /// 沟通工具ID
        /// </summary>
        public string Tool { get; set; }
        /// <summary>
        /// 沟通工具
        /// </summary>
        public string ToolName { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public string CustomerID { get; set; }
    }
}
