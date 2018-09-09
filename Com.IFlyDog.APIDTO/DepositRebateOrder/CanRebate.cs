using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 可退余额
    /// </summary>
    public class CanRebate
    {
        /// <summary>
        /// 可退代金券
        /// </summary>
        public IEnumerable<NoDoneCoupons> Coupons { get; set; }
        /// <summary>
        /// 可退预收款
        /// </summary>
        public IEnumerable<NoDoneDeposits> Deposits { get; set; }
    }

    /// <summary>
    /// 未使用的预收款
    /// </summary>
    public class NoDoneDeposits
    {
        /// <summary>
        /// 预收款ID
        /// </summary>
        public string DepositID { get; set; }
        /// <summary>
        /// 预收款类型
        /// </summary>
        public string DepositChargeName { get; set; }
        /// <summary>
        /// 预收款总金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 预售款剩余金额
        /// </summary>
        public decimal Rest { get; set; }
    }

    /// <summary>
    /// 未使用代金券
    /// </summary>
    public class NoDoneCoupons
    {
        /// <summary>
        /// 代金券ID
        /// </summary>
        public string CouponID { get; set; }
        /// <summary>
        /// 代金券类型
        /// </summary>
        public string CouponCategoryName { get; set; }
        /// <summary>
        /// 代金券总金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 代金券剩余金额
        /// </summary>
        public decimal Rest { get; set; }
    }
}
