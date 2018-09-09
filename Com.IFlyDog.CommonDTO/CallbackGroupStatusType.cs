using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 状态 1：使用；0：停用；2：删除
    /// </summary>
    public enum CallbackGroupStatusType
    {
        /// <summary>
        /// 停用
        /// </summary>
        [Description("回访组停用")]
        Stop = 0,
        /// <summary>
        /// 正常或者使用
        /// </summary>
        [Description("回访组使用")]
        Normal = 1,
        /// <summary>
        /// 删除
        /// </summary>
        [Description("回访组删除")]
        Delete = 2
    }
}
