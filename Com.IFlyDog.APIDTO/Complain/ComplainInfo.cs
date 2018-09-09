using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 投诉处理
    /// </summary>
   public class ComplainInfo
    {
        /// <summary>
        /// id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 投诉客户id
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 投诉客户名称
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 投诉时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 投诉内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 处理状态 0 未处理 1 已处理
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 处理时间
        /// </summary>
        public string FinishTime { get; set; }
        /// <summary>
        /// 处理结果
        /// </summary>
        public string Solution { get; set; }
        /// <summary>
        /// 处理用户
        /// </summary>
        public string FinishUserID { get; set; }
        /// <summary>
        /// 处理用户名称
        /// </summary>
        public string FinishUserName { get; set; }
    }
}
