using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 进货信息详情
    /// </summary>
   public class SmartPurchaseInfo
    {
        /// <summary>
        /// 进货信息id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 进货状态 0 暂存 1已进货
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 进货单号
        /// </summary>
        public string No { get; set; }
        /// <summary>
        /// 进货日期
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 仓库id
        /// </summary>
        public string WarehouseID { get; set; }
        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName { get; set; }
        /// <summary>
        /// 供应商id
        /// </summary>
        public string SupplierID { get; set; }
        /// <summary>
        /// 供应商名称
        /// </summary>
        public string SupplierName { get; set; }
        /// <summary>
        /// 操作用户
        /// </summary>
        public string CreateName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// /
        /// </summary>
        public string ProductName { get; set; }


        /// <summary>
        /// 批号
        /// </summary>
        public string Batch { get; set; }
        /// <summary>
        /// 有效期
        /// </summary>
        public string Expiration { get; set; }

        /// <summary>
        /// 进货药物品详细DTO
        /// </summary>
        public List<SmartPurchaseDetailAdd> SmartPurchaseDetail { get; set; }
    }
}
