using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 咨询记录
    /// </summary>
    public class Consult
    {
        /// <summary>
        /// 咨询记录ID
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
        /// 提交用户
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 沟通方式
        /// </summary>
        public string Tool { get; set; }
        /// <summary>
        /// 咨询项目
        /// </summary>
        public string Symptoms { get; set; }
        /// <summary>
        /// 咨询内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }

    /// <summary>
    /// 咨询选择条件
    /// </summary>
    public class ConsultSelect
    {
        /// <summary>
        /// 顾客ID，可空
        /// </summary>
        public long? CustomerID { get; set; }
        /// <summary>
        /// 开始时间，可空
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间，可空
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 咨询项目ID，可空
        /// </summary>
        public long? SymptomID { get; set; }
        /// <summary>
        /// 沟通方式ID，，可空
        /// </summary>
        public long? ToolID { get; set; }
        /// <summary>
        /// 提交用户ID，可空
        /// </summary>
        public long? CreateUserID { get; set; }
        /// <summary>
        /// 医院ID，可空
        /// </summary>
        public long? HospitalID { get; set; }
    }
}
