using Com.IFlyDog.CommonDTO;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 银行卡类型
    /// </summary>
    public class CardCategoryAdd
    {
        /// <summary>
        /// 银行卡名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
    }

    /// <summary>
    /// 银行卡修改
    /// </summary>
    public class CardCategoryUpdate
    {
        /// <summary>
        /// ID
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// 银行卡名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
    }

    /// <summary>
    /// 银行卡使用停用
    /// </summary>
    public class CardCategoryStopOrUse
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
    /// 银行卡信息
    /// </summary>
    public class CardCategory
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 银行卡名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public CommonStatus Status { get; set; }
    }
}