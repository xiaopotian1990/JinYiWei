using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 佣金使用类型
    /// </summary>
    public enum CommissionType
    {
        /// <summary>
        /// 消费
        /// </summary>
        [Description("消费")]
        Consume=1,
        /// <summary>
        /// 提现
        /// </summary>
        [Description("提现")]
        Out =2,
        /// <summary>
        /// 转让
        /// </summary>
        [Description("转让")]
        Send =3,
    }
}
