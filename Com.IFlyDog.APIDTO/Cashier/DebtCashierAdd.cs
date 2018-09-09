using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 欠款收银
    /// </summary>
    public class DebtCashierAdd
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DebtCashierAdd()
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
        /// 订单ID
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
        /// <summary>
        /// 是否是欠款收银0：否1：是
        /// </summary>
        public int IsDebt { get; set; }
    }
}
