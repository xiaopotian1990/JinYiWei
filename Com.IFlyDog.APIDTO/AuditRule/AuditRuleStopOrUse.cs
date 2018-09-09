using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 使用停用审核规则
    /// </summary>
   public class AuditRuleStopOrUse
    {
        /// <summary>
        /// 审核规则id
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// 操作用户id
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 医院id
        /// </summary>
        public long HospitalID { get; set; }

        /// <summary>
        /// 状态 0：停用；1：使用
        /// </summary>
        public CommonStatus Status { get; set; }
    }
}
