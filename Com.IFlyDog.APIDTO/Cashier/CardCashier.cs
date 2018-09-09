using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 收银时卡支付
    /// </summary>
    public class CardCashierAdd
    {
        /// <summary>
        /// 卡类型
        /// </summary>
        public long CardCategoryID { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
    }
}
