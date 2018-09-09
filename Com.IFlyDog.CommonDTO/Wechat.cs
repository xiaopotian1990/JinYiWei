using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 微信状态
    /// </summary>
    public enum WechatStatus
    {
        /// <summary>
        /// 绑定
        /// </summary>
        [Description("绑定")]
        Binding=1,
        /// <summary>
        /// 未绑定
        /// </summary>
        [Description("未绑定")]
        UnBinding =0,
        /// <summary>
        /// 全部
        /// </summary>
        [Description("全部")]
        All = 999
    }
}
