using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 进货信息管理添加dto
    /// </summary>
   public class SmartPurchaseAdd
    {
    
        /// <summary>
        /// 进货信息id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 仓库id
        /// </summary>
        public string WarehouseID { get; set; }
        /// <summary>
        /// 供应商id
        /// </summary>
        public string SupplierID { get; set; }
        /// <summary>
        /// 操作人id
        /// </summary>
        public string CreateUserID { get; set; }
        /// <summary>
        ///操作时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 状态 0：为进货（暂存）1：已进货
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 进货单号
        /// </summary>
        public string No { get; set; }
        /// <summary>
        /// 操作日期
        /// </summary>
        public string CreateDate { get; set; }

        /// <summary>
        /// 进货信息详情
        /// </summary>
        public virtual List<SmartPurchaseDetailAdd> SmartPurchaseDetail { get; set; }
    }
}
