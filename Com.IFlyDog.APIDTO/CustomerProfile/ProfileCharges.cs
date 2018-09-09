using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 客户档案消费项目
    /// </summary>
    public class ProfileCustomerCharges
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProfileCustomerCharges()
        {
            Charges = new List<ProfileCharges>();
        }
        /// <summary>
        /// 合计消费金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 现金
        /// </summary>
        public decimal CashCardAmount { get; set; }
        /// <summary>
        /// 预收款
        /// </summary>
        public decimal Deposit { get; set; }
        /// <summary>
        /// 券
        /// </summary>
        public decimal Coupon { get; set; }
        /// <summary>
        /// 欠款
        /// </summary>
        public decimal Debt { get; set; }
        /// <summary>
        /// 退款金额
        /// </summary>
        public decimal RebateAmount { get; set; }
        /// <summary>
        /// 佣金
        /// </summary>
        public decimal Commission { get; set; }
        /// <summary>
        /// 项目列表
        /// </summary>
        public IEnumerable<ProfileCharges> Charges { get; set; }
    }
    /// <summary>
    /// 消费项目
    /// </summary>
    public class ProfileCharges
    {
        /// <summary>
        /// 购买医院
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 购买日期
        /// </summary>
        public DateTime PaidTime { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ChargeName { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// 总数量
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 划扣数量
        /// </summary>
        public int UseNum { get; set; }
        /// <summary>
        /// 剩余数量
        /// </summary>
        public int RestNum { get; set; }
        /// <summary>
        /// 下单用户
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 成交金额
        /// </summary>
        public decimal FinalPrice { get; set; }
        /// <summary>
        /// 现金+刷卡
        /// </summary>
        public decimal CashCardAmount { get; set; }
        /// <summary>
        /// 预收款
        /// </summary>
        public decimal Deposit { get; set; }
        /// <summary>
        /// 券
        /// </summary>
        public decimal Coupon { get; set; }
        /// <summary>
        /// 佣金
        /// </summary>
        public decimal Commission { get; set; }
        /// <summary>
        /// 欠款
        /// </summary>
        public decimal Debt { get; set; }
        /// <summary>
        /// 退数量
        /// </summary>
        public int RebateNum { get; set; }
        /// <summary>
        /// 退金额
        /// </summary>
        public decimal RebateAmount { get; set; }
    }
}
