using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 采购发票查询展示数据dto
    /// </summary>
   public class InvoiceInfo
    {
        /// <summary>
        /// 采购发票
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 操作日期
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 供应商id
        /// </summary>
        public string SupplierID { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }
        /// <summary>
        /// 发票号
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 发票抬头
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public string Amount { get; set; }
        /// <summary>
        /// 操作用户
        /// </summary>
        public string CreateUserID { get; set; }
        /// <summary>
        /// 操作用户名称
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 发票日期
        /// </summary>
        public string BillDate { get; set; }

        public string Remark { get; set; }

        /// <summary>
        /// 采购发票详情dto
        /// </summary>
        public virtual List<InvoiceDetailAdd> InvoiceDetailAdd { get; set; }
    }
}
