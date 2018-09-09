using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 发票添加
    /// </summary>
    public class BillAdd
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public BillAdd()
        {
            OrderDetailID = new List<long>();
        }
        /// <summary>
        /// 医院ID
        /// </summary>
        public long HospitalID { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public long CustomerID { get; set; }
        /// <summary>
        /// 发票号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 发票抬头
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 发票日期
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 订单详细ID
        /// </summary>
        public IEnumerable<long> OrderDetailID { get; set; }
    }
}
