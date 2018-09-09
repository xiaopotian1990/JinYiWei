using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 微信设置特殊渠道
    /// </summary>
   public class WXOptionSpecialChannel
    {

        public long CreateUserID { get; set; }

        /// <summary>
        /// 特殊渠道信息
        /// </summary>
        public List<SpecialChannelAddInfo> SpecialChannelAddInfoList { get; set; }
    }

    /// <summary>
    /// 特殊渠道信息
    /// </summary>
    public class SpecialChannelAddInfo
    {
        /// <summary>
        /// 渠道id'
        /// </summary>
        public long  ID { get; set; }
    }
}
