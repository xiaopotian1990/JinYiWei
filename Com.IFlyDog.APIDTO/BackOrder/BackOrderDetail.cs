using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 退项目单
    /// </summary>
    public class BackOrder
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BackOrder()
        {
            Details = new List<BackOrderDetial>();
        }
        /// <summary>
        /// 订单ID
        /// </summary>
        public string OrderID { get; set; }
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
        /// 扣减积分
        /// </summary>
        public decimal Point { get; set; }
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
        /// 审核状态
        /// </summary>
        public AuditType AuditStatus { get; set; }
        /// <summary>
        /// 详细
        /// </summary>
        public virtual IList<BackOrderDetial> Details { get; set; }
    }

    /// <summary>
    /// 详细
    /// </summary>
    public class BackOrderDetial
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
        public decimal Amount { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Num { get; set; }
    }
}
