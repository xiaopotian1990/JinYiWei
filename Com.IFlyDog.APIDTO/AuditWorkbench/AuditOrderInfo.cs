using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 打开审核页面右侧订单详情（主要是开发人员和咨询人员变更）
    /// </summary>
   public class AuditOrderInfo
    {
        /// <summary>
        /// 数据id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 客户id
        /// </summary>
        public string CustomerID { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 医院名称
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 提交用户
        /// </summary>
        public string CreateName { get; set; }
        /// <summary>
        /// 原用户id
        /// </summary>
        public string OldID { get; set; }
        /// <summary>
        /// 原用户
        /// </summary>
        public string OldName { get; set; }

        /// <summary>
        /// 新用户id
        /// </summary>
        public string NewID { get; set; }
        /// <summary>
        /// 新用户
        /// </summary>
        public string NewName { get; set; }
        /// <summary>
        /// 原因
        /// </summary>
        public string Content { get; set; }
    }
}
