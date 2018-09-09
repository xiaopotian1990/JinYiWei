namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 分诊
    /// </summary>
    public class TriageAdd
    {
        /// <summary>
        /// 顾客ID
        /// </summary>
        public long CustomerID { get; set; }
        /// <summary>
        /// 顾客姓名
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 医院ID
        /// </summary>
        public long HospitalID { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 分配的咨询师ID
        /// </summary>
        public long SelectID { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 分诊类型
        /// </summary>
        public TriageType Type { get; set; }
    }
    /// <summary>
    /// 分诊类型
    /// </summary>
    public enum TriageType
    {
        /// <summary>
        /// 咨询
        /// </summary>
        Consult = 0,
        /// <summary>
        /// 治疗
        /// </summary>
        Treat = 1
    }
}
