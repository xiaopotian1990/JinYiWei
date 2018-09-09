using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 会员权益类型
    /// </summary>
    public enum EquityType
    {
        /// <summary>
        /// 折扣类权益
        /// </summary>
        [Description("折扣类权益")]
        Discount = 0,
        /// <summary>
        /// 自定义权益
        /// </summary>
        [Description("自定义权益")]
        Custom = 1,

        /// <summary>
        /// 打车权益
        /// </summary>
        [Description("打车权益")]
        TakeATaxi = 2
    }
}
