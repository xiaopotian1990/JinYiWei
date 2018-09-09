using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 审核规则枚举
    /// </summary>
    public enum AuditRuleTypeStatus
    {
        /// <summary>
        /// 订单折扣
        /// </summary>
        [Description("订单折扣")]
        OrderDiscount = 1,
        /// <summary>
        /// 退项目单
        /// </summary>
        [Description("退项目单")]
        BackOrder = 2,
        /// <summary>
        /// 退预收款
        /// </summary>
        [Description("退预收款")]
        DepositRebate = 3,

        /// <summary>
        /// 咨询人员变更
        /// </summary>
        [Description("咨询人员变更")]
        ConsultantUser = 4,
        /// <summary>
        /// 开发人员变更
        /// </summary>
        [Description("开发人员变更")]
        DeveloperUser = 5
    }
}
