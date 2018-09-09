using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 回访状态0：回访提醒1：已经完成的回访
    /// </summary>
    public enum CallbackStatus
    {
        /// <summary>
        /// 回访提醒
        /// </summary>
        [Description("回访提醒")]
        Remind = 0,
        /// <summary>
        /// 已完成
        /// </summary>
        [Description("已完成")]
        Done = 1,
        /// <summary>
        /// 全部
        /// </summary>
        [Description("全部")]
        All =99
    }
}
