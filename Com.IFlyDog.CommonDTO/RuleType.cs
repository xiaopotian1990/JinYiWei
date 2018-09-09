using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 审核类型
    /// </summary>
    public enum RuleType
    {
        /// <summary>
        /// 项目单
        /// </summary>
        [Description("项目单")]
        Order =1,
        /// <summary>
        /// 退项目单
        /// </summary>
        [Description("退项目单")]
        BackOrder=2,
        /// <summary>
        /// 退预收款
        /// </summary>
        [Description("退预收款")]
        DepositRebate =3
    }
}
