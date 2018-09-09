using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 审核类型
    /// </summary>
    public enum AuditType
    {
        /// <summary>
        /// 待审核
        /// </summary>
        Pending = 1,
        /// <summary>
        /// 无需审核
        /// </summary>
        NoApprove = 2,
        /// <summary>
        /// 审核不通过
        /// </summary>
        UnApprove = 3,
        /// <summary>
        /// 审核通过
        /// </summary>
        Approve = 4,
        /// <summary>
        /// 未提交
        /// </summary>
        Uncommitted = 5
    }
}
