using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 可用于收银的余额
    /// </summary>
    public class CanCashier
    {
        /// <summary>
        /// 顾客ID
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 剩余佣金
        /// </summary>
        public decimal Commission { get; set; }
        /// <summary>
        /// 可用代金券
        /// </summary>
        public IEnumerable<CanCashierCoupons> Coupons { get; set; }
        /// <summary>
        /// 可用预收款
        /// </summary>
        public IEnumerable<CanCashierDeposits> Deposits { get; set; }
    }

    /// <summary>
    /// 可用的预收款
    /// </summary>
    public class CanCashierDeposits
    {
        /// <summary>
        /// 预收款ID
        /// </summary>
        public string DepositID { get; set; }
        /// <summary>
        /// 医院ID
        /// </summary>
        public string HospitalID { get; set; }
        /// <summary>
        /// 医院
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 预收款类型ID
        /// </summary>
        public string DepositChargeID { get; set; }
        /// <summary>
        /// 预收款类型名称
        /// </summary>
        public string DepositChargeName { get; set; }
        /// <summary>
        /// 预售款剩余金额
        /// </summary>
        public decimal Rest { get; set; }
        /// <summary>
        /// 范围限制
        /// </summary>
        public int ScopeLimit { get; set; }
    }

    /// <summary>
    /// 可用代金券
    /// </summary>
    public class CanCashierCoupons
    {
        /// <summary>
        /// 预收款ID
        /// </summary>
        public string CouponID { get; set; }
        /// <summary>
        /// 代金券ID
        /// </summary>
        public string HospitalID { get; set; }
        /// <summary>
        /// 医院
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 代金券类型ID
        /// </summary>
        public string CouponChargeID { get; set; }
        /// <summary>
        /// 代金券类型名称
        /// </summary>
        public string CouponChargeName { get; set; }
        /// <summary>
        /// 代金券剩余金额
        /// </summary>
        public decimal Rest { get; set; }
        /// <summary>
        /// 范围限制
        /// </summary>
        public int ScopeLimit { get; set; }
    }
}
