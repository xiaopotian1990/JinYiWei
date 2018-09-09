using Com.IFlyDog.CommonDTO;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 权益增加
    /// </summary>
    public class EquityAdd
    {
        /// <summary>
        /// 权益名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 类型0：折扣类权益1：自定义权益 2 打车权益
        /// </summary>
        public EquityType Type { get; set; }
        /// <summary>
        /// 折扣
        /// </summary>
        public decimal? Discount { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
    }

    /// <summary>
    /// 权益修改
    /// </summary>
    public class EquityUpdate
    {
        /// <summary>
        /// ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 权益名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 类型0：折扣类权益1：自定义权益 2 打车权益
        /// </summary>
        public EquityType Type { get; set; }
        /// <summary>
        /// 折扣
        /// </summary>
        public decimal? Discount { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
    }

    /// <summary>
    /// 权益使用或者停用
    /// </summary>
    public class EquityStopOrUse
    {
        /// <summary>
        /// ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 状态0：停用1：使用
        /// </summary>
        public CommonStatus Status { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
    }

    /// <summary>
    /// 权益
    /// </summary>
    public class Equity
    {
        /// <summary>
		/// ID
		/// </summary>
		public string ID { get; set; }
        /// <summary>
        /// 权益名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 类型0：折扣类权益1：自定义权益 2打车权益
        /// </summary>
        public EquityType Type { get; set; }
        /// <summary>
        /// 折扣
        /// </summary>
        public decimal? Discount { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 状态0：停用1：使用
        /// </summary>
        public CommonStatus Status { get; set; }
    }
}
