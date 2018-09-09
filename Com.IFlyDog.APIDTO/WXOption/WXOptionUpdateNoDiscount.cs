using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 微信系统设置修改不提点折扣小于
    /// </summary>
  public  class WXOptionUpdateNoDiscount
    {
        /// <summary>
        /// 不提点折扣小于Code
        /// </summary>
        public string NoDiscountCode { get; set; }

        /// <summary>
        /// 不提点折扣小于Value
        /// </summary>
        public string NoDiscountValue { get; set; }

        public long CreateUserID { get; set; }
    }
}
