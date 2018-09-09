using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 支付方式
    /// </summary>
    public enum PayType
    {
        /// <summary>
        /// 券支付
        /// </summary>
        Coupon = 1,
        /// <summary>
        /// 余额支付
        /// </summary>
        Balance = 2,
        /// <summary>
        /// 现金支付
        /// </summary>
        Cash = 3,
        /// <summary>
        /// 银行卡支付
        /// </summary>
        Card = 4
    }
}
