using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 预收款收银
    /// </summary>
    public class DepositCashierAdd
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DepositCashierAdd()
        {
            CardList = new List<CardCashierAdd>();
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
        /// 预收款订单ID
        /// </summary>
        public long OrderID { get; set; }
        /// <summary>
        /// 现金
        /// </summary>
        public decimal Cash { get; set; }
        /// <summary>
        /// 刷卡列表
        /// </summary>
        public IEnumerable<CardCashierAdd> CardList { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }


    public class DepositOrderTemp
    {
        public long OrderID { get; set; }
        public long CustomerID { get; set; }
        public long HospitalID { get; set; }
        public decimal Amount { get; set; }
        public PaidStatus PaidStatus { get; set; }
        public long ChargeID { get; set; }
        public decimal Total { get; set; }
        public int HasCoupon { get; set; }
        public long? CouponCategoryID { get; set; }
        public decimal? CouponAmount { get; set; }
        public int Num { get; set; }
    }


}
