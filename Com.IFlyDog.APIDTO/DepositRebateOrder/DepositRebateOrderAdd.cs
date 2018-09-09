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
    public class DepositRebateOrderAdd
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DepositRebateOrderAdd()
        {
            Details = new List<DepositRebateOrderDetailAdd>();
            CouponDetails = new List<DepositRebateOrderCouponDetailAdd>();
        }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public long CustomerID { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 操作人所在医院
        /// </summary>
        public long HospitalID { get; set; }
        /// <summary>
        /// 退款总金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 扣减积分
        /// </summary>
        public decimal Point { get; set; }
        /// <summary>
        /// 详细
        /// </summary>
        public virtual IEnumerable<DepositRebateOrderDetailAdd> Details { get; set; }
        /// <summary>
        /// 退券详细
        /// </summary>
        public virtual IEnumerable<DepositRebateOrderCouponDetailAdd> CouponDetails { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    /// <summary>
    /// 退款详细
    /// </summary>
    public class DepositRebateOrderDetailAdd
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        public long DepositID { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
    }

    /// <summary>
    /// 退款详细
    /// </summary>
    public class DepositRebateOrderCouponDetailAdd
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        public long CouponID { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
    }

    /// <summary>
    /// 退预收款详细
    /// </summary>
    public class DepositRebateOrderDetailTemp
    {
        /// <summary>
        /// ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 预收款订单
        /// </summary>
        public long OrderID { get; set; }
        /// <summary>
        /// 预收款类型ID
        /// </summary>
        public long DepositID { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
    }


    /// <summary>
    /// 退预收款详细
    /// </summary>
    public class DepositRebateOrderCouponDetailTemp
    {
        /// <summary>
        /// ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 预收款订单
        /// </summary>
        public long OrderID { get; set; }
        /// <summary>
        /// 预收款类型ID
        /// </summary>
        public long CouponID { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
    }
}
