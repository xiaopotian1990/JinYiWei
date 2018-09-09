using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 佣金获得类别
    /// </summary>
    public enum CommissionRecordType
    {
        /// <summary>
        /// 消费提成
        /// </summary>
        [Description("消费提成")]
        Consume=1,
        /// <summary>
        /// 佣金提成
        /// </summary>
        [Description("佣金提成")]
        Commission =2
    }
}
