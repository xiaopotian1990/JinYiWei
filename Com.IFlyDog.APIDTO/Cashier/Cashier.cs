using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 收银记录
    /// </summary>
    public class Cashier
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Cashier()
        {
            CardCategoryNames = new List<string>();
        }
        /// <summary>
        /// 收银记录ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 客户
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>
        public OrderType OrderType { get; set; }
        /// <summary>
        /// 所属医院ID
        /// </summary>
        public string HospitalID { get; set; }
        /// <summary>
        /// 所属医院
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 订单号
        /// </summary>
        public string No { get; set; }
        /// <summary>
        /// 收银时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 收银用户
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 应收金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 实收金额
        /// </summary>
        public decimal RealAmount { get; set; }
        /// <summary>
        /// 现金
        /// </summary>
        public decimal Cash { get; set; }
        /// <summary>
        /// 刷卡
        /// </summary>
        public decimal Card { get; set; }
        /// <summary>
        /// 预收款
        /// </summary>
        public decimal Deposit { get; set; }
        /// <summary>
        /// 代金券
        /// </summary>
        public decimal Coupon { get; set; }
        /// <summary>
        /// 欠款
        /// </summary>
        public decimal Debt { get; set; }
        /// <summary>
        /// 佣金
        /// </summary>
        public decimal Commission { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 结算状态
        /// </summary>
        public CashierStatus Status { get; set; }
        /// <summary>
        /// 可类型
        /// </summary>
        public virtual IList<string> CardCategoryNames { get; set; }
    }

    /// <summary>
    /// 收银记录选择
    /// </summary>
    public class CashierSelect
    {
        /// <summary>
        /// 医院ID
        /// </summary>
        public long HospitalID { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 顾客编号
        /// </summary>
        public long CustomerID { get; set; }
        /// <summary>
        /// 单号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 第几页
        /// </summary>
        public int PageNum { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; }
    }
}
