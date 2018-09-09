using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 上门状态
    /// </summary>
    public enum ComeType
    {
        /// <summary>
        /// 已上门
        /// </summary>
        [Description("已上门")]
        Yes = 1,
        /// <summary>
        /// 未上门
        /// </summary>
        [Description("未上门")]
        No = 0,
        /// <summary>
        /// 全部
        /// </summary>
        [Description("全部")]
        All = 999
    }
}
