using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 我的审核申请查询
    /// </summary>
    public class AuditApplyInfo
    {
        /// <summary>
        /// 审核申请id
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
        /// 医院id
        /// </summary>
        public string HospitalID { get; set; }


        /// <summary>
        /// 原因
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 提交时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 类型 1开发人员变更申请 2咨询人员变更申请
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 1 待审核 2 无需审核 3 审核不通过 4 审核通过 5 未提交
        /// </summary>
        public int AuditStatus { get; set; }
    }
}
