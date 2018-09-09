using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 预收款详细
    /// </summary>
    public class DepositOrder
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DepositOrder()
        {
            Details = new List<DepositOrderDetial>();
        }
        /// <summary>
        /// 订单ID
        /// </summary>
        public string OrderID { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 下单医院
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 下单医院ID
        /// </summary>
        public string HospitalID { get; set; }
        /// <summary>
        /// 下单时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 下单人
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 支付状态
        /// </summary>
        public PaidStatus PaidStatus { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime? PaidTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 详细
        /// </summary>
        public virtual IList<DepositOrderDetial> Details { get; set; }
    }

    /// <summary>
    /// 预收款详细
    /// </summary>
    public class DepositOrderDetial
    {
        /// <summary>
        /// 预收款项目ID
        /// </summary>
        public string ChargeID { get; set; }
        /// <summary>
        /// 预收款项目
        /// </summary>
        public string ChargeName { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Total { get; set; }
    }
}
