using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 收银详细
    /// </summary>
    public class CashierDetail
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CashierDetail()
        {
            CardList = new List<CardCashier>();
            ChargeList = new List<CashierChargeTemp>();
        }
        /// <summary>
        /// 医院ID
        /// </summary>
        public long HospitalID { get; set; }
        /// <summary>
        /// 订单ID
        /// </summary>
        public string OrderID { get; set; }
        /// <summary>
        /// 收银记录ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 顾客姓名
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 单号
        /// </summary>
        public string No { get; set; }
        /// <summary>
        /// 收银时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 刷卡
        /// </summary>
        public decimal Card { get; set; }
        /// <summary>
        /// 现金
        /// </summary>
        public decimal Cash { get; set; }
        /// <summary>
        /// 佣金
        /// </summary>
        public decimal Commission { get; set; }
        /// <summary>
        /// 券
        /// </summary>
        public decimal Coupon { get; set; }
        /// <summary>
        /// 欠款
        /// </summary>
        public decimal Debt { get; set; }
        /// <summary>
        /// 预收款
        /// </summary>
        public decimal Deposit { get; set; }
        /// <summary>
        /// 收银员
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>
        public OrderType OrderType { get; set; }

        /// <summary>
        /// 刷卡列表
        /// </summary>
        public IList<CardCashier> CardList { get; set; }
        /// <summary>
        /// 项目列表
        /// </summary>
        public IEnumerable<CashierChargeTemp> ChargeList { get; set; }
    }

    /// <summary>
    /// 临时表
    /// </summary>
    public class CashierChargeTemp
    {
        /// <summary>
        /// 预约项目
        /// </summary>
        public string ChargeID { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public string Num { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 欠款
        /// </summary>
        public decimal DebtAmount { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 项目
        /// </summary>
        public string ChargeName { get; set; }
    }
}
