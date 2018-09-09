using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 修改退货入库信息dto
    /// </summary>
   public class SmartReturnUpdate
    {
        /// <summary>
        /// 退货信息id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 供应商id
        /// </summary>
        public string SupplierID { get; set; }
        /// <summary>
        /// 仓库id
        /// </summary>
        public string WarehouseID { get; set; }
        /// <summary>
        /// 操作人id
        /// </summary>
        public string CreateUserID { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 退货单号
        /// </summary>
        public string No { get; set; }
        /// <summary>
        /// 退货日期
        /// </summary>
        public string CreateDate { get; set; }

        /// <summary>
        /// 退货信息详细DTO
        /// </summary>
        public virtual List<SmartReturnDetailAdd> SmartReturnDetail { get; set; }
    }
}
