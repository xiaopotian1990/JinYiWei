using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 预约状态0：未上门，1：已完成预约,99：所有
    /// </summary>
    public enum AppointmentStatus
    {
        /// <summary>
        /// 未上门
        /// </summary>
        [Description("未上门")]
        No = 0,
        /// <summary>
        /// 已完成预约
        /// </summary>
        [Description("已完成预约")]
        Yes = 1,
        /// <summary>
        /// 所有
        /// </summary>
        [Description("所有")]
        All =99
    }
}
