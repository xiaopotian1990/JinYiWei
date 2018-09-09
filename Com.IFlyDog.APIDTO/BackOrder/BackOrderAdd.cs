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
    public class BackOrderAdd
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BackOrderAdd()
        {
            Details = new List<BackOrderDetailAdd>();
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
        public virtual IEnumerable<BackOrderDetailAdd> Details { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    /// <summary>
    /// 退款详细
    /// </summary>
    public class BackOrderDetailAdd
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        public long ChargeID { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 退款详细ID
        /// </summary>
        public long DetailID { get; set; }
    }

    /// <summary>
    /// 退项目单详细
    /// </summary>
    public class BackOrderDetailTemp
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
        public long ChargeID { get; set; }
        /// <summary>
        /// 预收款数量
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 项目详细ID
        /// </summary>
        public long DetailID { get; set; }
    }
}
