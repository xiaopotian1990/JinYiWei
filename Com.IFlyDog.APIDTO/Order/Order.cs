using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 订单
    /// </summary>
    public class Order
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Order()
        {
            ChargeDetials = new List<OrderChargeDetail>();
            SetDetials = new List<OrderSetDetail>();
        }
        /// <summary>
        /// 订单号
        /// </summary>
        public string OrderID { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public string CreateUserID { get; set; }
        /// <summary>
        /// 提交人
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 下单医院ID
        /// </summary>
        public string HospitalID { get; set; }
        /// <summary>
        /// 下单医院
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 总价格
        /// </summary>
        public decimal TotalPrice { get; set; }
        /// <summary>
        /// 折后价格
        /// </summary>
        public decimal FinalPrice { get; set; }
        /// <summary>
        /// 支付状态
        /// </summary>
        public PaidStatus PaidStatus { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public AuditType AuditStatus { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime? PaidTime { get; set; }
        /// <summary>
        /// 欠款金额
        /// </summary>
        public decimal DebtAmount { get; set; }
        /// <summary>
        /// 总体折扣率
        /// </summary>
        public int Discount { get; set; }
        /// <summary>
        /// 项目
        /// </summary>
        public virtual IEnumerable<OrderChargeDetail> ChargeDetials { get; set; }
        /// <summary>
        /// 套餐
        /// </summary>
        public IEnumerable<OrderSetDetail> SetDetials { get; set; }
    }
    /// <summary>
    /// 项目详细
    /// </summary>
    public class OrderChargeDetail
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        public string ChargeID { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ChargeName { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 最终价格
        /// </summary>
        public decimal FinalPrice { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Num { get; set; }
    }

    /// <summary>
    /// 套餐详细
    /// </summary>
    public class OrderSetDetail
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public OrderSetDetail()
        {
            ChargeDetails = new List<OrderChargeDetail>();
        }
        /// <summary>
        /// 套餐ID
        /// </summary>
        public string SetID { get; set; }
        /// <summary>
        /// 套餐名称
        /// </summary>
        public string SetName { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int SetNum { get; set; }
        /// <summary>
        /// 单价
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 折后总价格
        /// </summary>
        public decimal FinalPrice { get; set; }
        /// <summary>
        /// 项目详细
        /// </summary>
        public IList<OrderChargeDetail> ChargeDetails { get; set; }
    }
}
