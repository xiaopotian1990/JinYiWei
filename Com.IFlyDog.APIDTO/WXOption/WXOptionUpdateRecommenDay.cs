using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 微信系统设置修改推荐时限
    /// </summary>
   public class WXOptionUpdateRecommenDay
    {
        /// <summary>
        /// 推荐时限，天数 Code
        /// </summary>
        public string RecommendNumberDayCode { get; set; }

        /// <summary>
        /// 推荐时限，天数 Value
        /// </summary>
        public string RecommendNumberDayValue { get; set; }

        public long CreateUserID { get; set; }
    }
}
