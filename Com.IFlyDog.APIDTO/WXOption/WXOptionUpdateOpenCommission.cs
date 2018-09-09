using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 微信系统设置佣金提成
    /// </summary>
   public class WXOptionOpenCommission
    {
        /// <summary>
        /// 是否开启佣金提成Code
        /// </summary>
        public string OpenCommissionCode { get; set; }

        /// <summary>
        /// 是否开启佣金提成值Value
        /// </summary>
        public string OpenCommissionValue { get; set; }
        /// <summary>
        /// 佣金提成等级Code
        /// </summary>
        public string CommissionLevelCode { get; set; }

        /// <summary>
        /// 佣金提成等级Value
        /// </summary>
        public string CommissionLevelValue { get; set; }
        /// <summary>
        /// 佣金提成提点Code
        /// </summary>
        public string CommissionRemindedCode { get; set; }
        /// <summary>
        /// 佣金提成提点Value 
        /// 
        /// </summary>
        public string CommissionRemindedValue { get; set; }

        public long CreateUserID { get; set; }
    }
}
