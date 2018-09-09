using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 退预收款
    /// </summary>
    public class DepositRebateOrder
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DepositRebateOrder()
        {
            Details = new List<DepositRebateOrderDetial>();
            CouponDetails = new List<DepositRebateCouponOrderDetial>();
        }
        /// <summary>
        /// 订单ID
        /// </summary>
        public string OrderID { get; set; }
        /// <summary>
        /// 下单医院
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 下单医院ID
        /// </summary>
        public string HospitalID { get; set; }
        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 下单人
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 退预收款总额
        /// </summary>
        public decimal Deposit { get; set; }
        /// <summary>
        /// 退积分总额
        /// </summary>
        public decimal Point { get; set; }
        /// <summary>
        /// 退券总额
        /// </summary>
        public decimal Coupon { get; set; }
        /// <summary>
        /// 支付状态
        /// </summary>
        public PaidStatus PaidStatus { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime? PaidTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public AuditType AuditStatus { get; set; }
        /// <summary>
        /// 代金券详细
        /// </summary>
        public virtual IList<DepositRebateOrderDetial> Details { get; set; }
        /// <summary>
        /// 券详细
        /// </summary>
        public virtual IList<DepositRebateCouponOrderDetial> CouponDetails { get; set; }
    }

    /// <summary>
    /// 详细
    /// </summary>
    public class DepositRebateOrderDetial
    {
        /// <summary>
        /// 预收款ID
        /// </summary>
        public string DepositID { get; set; }
        /// <summary>
        /// 预收款
        /// </summary>
        public string DepositName { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
    }

    /// <summary>
    /// 详细
    /// </summary>
    public class DepositRebateCouponOrderDetial
    {
        /// <summary>
        /// 券ID
        /// </summary>
        public string CouponID { get; set; }
        /// <summary>
        /// 券
        /// </summary>
        public string CouponName { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
    }
}
