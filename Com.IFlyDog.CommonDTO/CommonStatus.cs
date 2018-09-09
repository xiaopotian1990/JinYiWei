using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 通用状态类 0：停用；1：使用
    /// </summary>
    public enum CommonStatus
    {
        /// <summary>
        /// 停用
        /// </summary>
        [Description("停用")]
        Stop =0,
        /// <summary>
        /// 使用
        /// </summary>
        [Description("使用")]
        Use =1,
        /// <summary>
        /// 全部，查询时使用
        /// </summary>
        [Description("全部")]
        All = 999
    }
}
