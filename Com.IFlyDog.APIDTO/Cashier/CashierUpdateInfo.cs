using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 收银更新
    /// </summary>
    public class CashierUpdateInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CashierUpdateInfo()
        {
            CardList = new List<CardCashier>();
        }
        /// <summary>
        /// 收银记录ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 医院ID
        /// </summary>
        public string HospitalID { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 现金
        /// </summary>
        public decimal Cash { get; set; }
        /// <summary>
        /// 刷卡
        /// </summary>
        public decimal Card { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>

        public OrderType OrderType { get; set; }
        /// <summary>
        /// 刷卡列表
        /// </summary>
        public IList<CardCashier> CardList { get; set; }
        /// <summary>
        /// 订单结算状态
        /// </summary>
        public CashierStatus Status { get; set; }
    }

    /// <summary>
    /// 卡收费信息
    /// </summary>
    public class CardCashier
    {
        /// <summary>
        /// 卡类型
        /// </summary>
        public string CardCategoryID { get; set; }
        /// <summary>
        /// 卡类型
        /// </summary>
        public string CardCategoryName { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
    }

    /// <summary>
    /// 收银更新
    /// </summary>
    public class CashierUpdate
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CashierUpdate()
        {
            CardList = new List<CardCashierAdd>();
        }
        /// <summary>
        /// 收银记录ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 医院ID
        /// </summary>
        public long HospitalID { get; set; }
        /// <summary>
        /// 现金
        /// </summary>
        public decimal Cash { get; set; }
        /// <summary>
        /// 订单类型
        /// </summary>

        public OrderType OrderType { get; set; }
        /// <summary>
        /// 刷卡列表
        /// </summary>
        public IList<CardCashierAdd> CardList { get; set; }
    }
}
