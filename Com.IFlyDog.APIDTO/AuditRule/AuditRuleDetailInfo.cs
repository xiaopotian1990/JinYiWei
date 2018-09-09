using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 审核规则详细dto
    /// </summary>
   public class AuditRuleDetailInfo
    {
        /// <summary>
        /// 审核规则详细id
        /// </summary>
        public string   ID { get; set; }
        /// <summary>
        /// 审核规则id
        /// </summary>
        public string RuleID { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 用户所属审核级别
        /// </summary>
        public string Level { get; set; }
    }
}
