using System.Collections.Generic;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 咨询添加,修改
    /// </summary>
    public class ConsultAddUpdate
    {
        /// <summary>
        /// 咨询记录ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public long CustomerID { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 沟通方式ID
        /// </summary>
        public long ToolID { get; set; }
        /// <summary>
        /// 咨询方式
        /// </summary>
        public IEnumerable<long> SymptomIDS { get; set; }
        /// <summary>
        /// 咨询内容
        /// </summary>
        public string Content { get; set; }
    }
}
