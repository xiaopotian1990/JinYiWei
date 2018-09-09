using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 退项目
    /// </summary>
    public class BackCashierAdd
    {
        /// <summary>
        /// 顾客ID
        /// </summary>
        public long CustomerID { get; set; }
        /// <summary>
        /// 医院ID
        /// </summary>
        public long HospitalID { get; set; }
        /// <summary>
        /// 收银人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 订单ID
        /// </summary>
        public long OrderID { get; set; }
        /// <summary>
        /// 现金
        /// </summary>
        public decimal Cash { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 刷卡
        /// </summary>
        public long? CardCategoryID { get; set; }
        /// <summary>
        /// 刷卡
        /// </summary>
        public decimal Card { get; set; }
        /// <summary>
        /// 退还到预收款
        /// </summary>
        public long? DepositChargeID { get; set; }
        /// <summary>
        /// 退还到预收款
        /// </summary>
        public decimal Deposit { get; set; }
        /// <summary>
        /// 退还到代金券
        /// </summary>
        public long? CouponCategoryID { get; set; }
        /// <summary>
        /// 退还到代金券
        /// </summary>
        public decimal Coupon { get; set; }
    }


    public class BackOrderTemp
    {
        public long OrderID { get; set; }
        public long CustomerID { get; set; }
        public long HospitalID { get; set; }
        public decimal Amount { get; set; }
        public decimal Point { get; set; }
        public PaidStatus PaidStatus { get; set; }
        public AuditType AuditStatus { get; set; }
        public long DetailID { get; set; }
        public long ChargeID { get; set; }
        public decimal DetailAmount { get; set; }
        public int Num { get; set; }
    }
}
