using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 采购详情dto
    /// </summary>
   public class InvoiceDetailAdd
    {
        /// <summary>
        /// 采购发票id
        /// </summary>
        public string ID { get; set; }
/// <summary>
/// 进货入库id
/// </summary>
        public string PurchaseID { get; set; }
        /// <summary>
        /// 进货单号
        /// </summary>
        public string No { get; set; }
        /// <summary>
        /// 进货仓库名称
        /// </summary>
        public string WarehouseName { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }
        /// <summary>
        /// 入库日期
        /// </summary>
        public string CreateTime { get; set; }
    }
}
