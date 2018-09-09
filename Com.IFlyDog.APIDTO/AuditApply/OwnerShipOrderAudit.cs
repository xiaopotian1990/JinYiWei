using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 审核查看
    /// </summary>
   public class OwnerShipOrderAudit
    {
        /// <summary>
        /// 审核状态 1 待审核  3 审核不通过 4 审核通过
        /// </summary>
        public int AuditStatus { get; set; }

        /// <summary>
        /// 审核详情记录
        /// </summary>
        public List<AuditDetails> AuditDetailsList { get; set; }
    }

    /// <summary>
    /// 审核详情s
    /// </summary>
    public class AuditDetails {

        /// <summary>
        /// 审核规则下可以审核的用户
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 等级
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 审核用户
        /// </summary>
        public string AuditUser { get; set; }
        /// <summary>
        /// 审核状态  1 待审核  3 审核不通过 4 审核通过
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public string  CreateTime{ get; set; }

        /// <summary>
        /// 意见
        /// </summary>
        public string Content { get; set; }
    }
}
