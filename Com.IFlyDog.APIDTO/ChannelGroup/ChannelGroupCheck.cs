using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 渠道组检测
    /// </summary>
   public class ChannelGroupCheck
    {
        /// <summary>
        /// 渠道id
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 渠道名称
        /// </summary>
        public string ChannelName { get; set; }

        /// <summary>
        /// 状态0：停用1：使用
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 渠道所属渠道组名称
        /// </summary>
        public string ChannelGroupName { get; set; }
    }
}
