using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 微信系统设置级别提点
    /// </summary>
   public class WXOptionUpdatePromoteLevel
    {

        public long CreateUserID { get; set; }

        /// <summary>
        /// 级别信息
        /// </summary>
        public List<PromoteLevelAddInfo> PromoteLevelAddInfo { get; set; }
    }

    public class PromoteLevelAddInfo {
        /// <summary>
        /// 级别
        /// </summary>
        public int Level{ get; set; }
        /// <summary>
        /// 提点
        /// </summary>
        public string Rate { get; set; }
    }
}
