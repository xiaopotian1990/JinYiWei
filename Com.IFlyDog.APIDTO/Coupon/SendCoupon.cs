using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 手工赠券
    /// </summary>
    public class SendCoupon
    {
        /// <summary>
        /// 操作人所在医院ID
        /// </summary>
        public long HospitalID { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public long CustomerID { get; set; }
        /// <summary>
        /// 券额
        /// </summary>
        public decimal CouponAmount { get; set; }
        /// <summary>
        /// 券类型或者券ID
        /// </summary>
        public long CouponID { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
