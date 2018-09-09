using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 成交状态
    /// </summary>
    public enum DealType
    {
        /// <summary>
        /// 已成交
        /// </summary>
        [Description("已成交")]
        Yes = 1,
        /// <summary>
        /// 未成交
        /// </summary>
        [Description("未成交")]
        No = 0,
        /// <summary>
        /// 全部
        /// </summary>
        [Description("全部")]
        All = 999
    }
}
