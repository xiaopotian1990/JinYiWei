using System;
using System.Collections.Generic;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 咨询记录
    /// </summary>
    public class ConsultDetail
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ConsultDetail()
        {
            Symptoms = new List<SymptomDetail>();
        }
        /// <summary>
        /// 咨询记录ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 提交用户
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 沟通方式ID
        /// </summary>
        public string ToolID { get; set; }
        /// <summary>
        /// 沟通方式
        /// </summary>
        public string Tool { get; set; }

        /// <summary>
        /// 咨询内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 咨询项目列表
        /// </summary>
        public virtual List<SymptomDetail> Symptoms { get; set; }
    }
    /// <summary>
    /// 咨询项目
    /// </summary>
    public class SymptomDetail
    {
        /// <summary>
        /// 咨询项目ID
        /// </summary>
        public string SymptomID { get; set; }
        /// <summary>
        /// 咨询项目
        /// </summary>
        public string SymptomName { get; set; }
    }
}
