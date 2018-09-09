using Com.IFlyDog.CommonDTO;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 顾客标签
    /// </summary>
    public class TagAdd
    {
        /// <summary>
        /// 标签内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
    }

    /// <summary>
    /// 顾客标签修改
    /// </summary>
    public class TagUpdate
    {
        /// <summary>
        /// ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 银行卡名称
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
    }

    /// <summary>
    /// 顾客标签使用停用
    /// </summary>
    public class TagStopOrUse
    {
        /// <summary>
        /// 银行卡ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 状态0：停用；1：使用
        /// </summary>
        public CommonStatus Status { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
    }

    /// <summary>
    /// 顾客标签信息
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 顾客标签名称
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public CommonStatus Status { get; set; }
    }
}
