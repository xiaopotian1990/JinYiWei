using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 删除渠道组
    /// </summary>
   public class ChannelGroupDelete
    {
        /// <summary>
        /// 渠道组id
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 创建用户id
        /// </summary>
        public long CreateUserID { get; set; }
    }
}
