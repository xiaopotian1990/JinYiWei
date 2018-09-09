using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 微信设置默认渠道
    /// </summary>
   public class WXOptionDefaultChannel
    {
        /// <summary>
        /// 默认渠道code
        /// </summary>
        public string ChannelCode { get; set; }
        /// <summary>
        /// 默认渠道值
        /// </summary>
        public string ChannelValue { get; set; }

        public long CreateUserID { get; set; }
    }
}
