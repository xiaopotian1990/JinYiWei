using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 订单支付
    /// </summary>
    public class OrderCashierAdd
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public OrderCashierAdd()
        {
            CardList = new List<CardCashierAdd>();
            DepositUseList = new List<CardCashierAdd>();
            CouponUseList = new List<CardCashierAdd>();
        }
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
        /// 佣金
        /// </summary>
        public decimal Commission { get; set; }
        /// <summary>
        /// 刷卡列表
        /// </summary>
        public IEnumerable<CardCashierAdd> CardList { get; set; }
        /// <summary>
        /// 预收款使用列表
        /// </summary>
        public IEnumerable<CardCashierAdd> DepositUseList { get; set; }
        /// <summary>
        /// 券使用列表
        /// </summary>
        public IEnumerable<CardCashierAdd> CouponUseList { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 是否是欠款收银0：否1：是
        /// </summary>
        public int IsDebt { get; set; }
    }

    
    public class OrderTemp
    {
        public long OrderID { get; set; }
        public long CustomerID { get; set; }
        public long HospitalID { get; set; }
        public decimal FinalPrice { get; set; }
        public PaidStatus PaidStatus { get; set; }
        public AuditType AuditStatus { get; set; }
        public long DetailID { get; set; }
        public long ChargeID { get; set; }
        public decimal DetailFinalPrice { get; set; }
        public int Num { get; set; }
        public decimal DebtAmount { get; set; }
    }
}
