using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 采购发票添加dto
    /// </summary>
   public class InvoiceAdd
    {
        /// <summary>
        /// 采购记录id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 供应商id
        /// </summary>
        public string SupplierID { get; set; }
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
        /// 操作时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 操作人id
        /// </summary>
        public string CreateUserID { get; set; }
        /// <summary>
        /// 医院id
        /// </summary>
        public string HospitalID { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 发票日期
        /// </summary>
        public string BillDate { get; set; }

        /// <summary>
        /// 采购发票信息详细DTO
        /// </summary>
        public virtual List<InvoiceDetailAdd> InvoiceDetailAdd { get; set; }
    }
}
