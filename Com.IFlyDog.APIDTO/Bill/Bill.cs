using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 发票
    /// </summary>
    public class Bill
    {
        /// <summary>
        /// 发票ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 开票日期
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 发票日期
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 顾客姓名
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 开票人员
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
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
    }
}
