using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 退货信息查询dto
    /// </summary>
   public class SmartReturnInfo
    {
        /// <summary>
    /// 退货信息id
    /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 退货信息状态
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 退货信息单号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 操作用户id，只查询当前用户操作的
        /// </summary>
        public string CrUserId { get; set; }

        /// <summary>
        /// 退货时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 退货仓库id
        /// </summary>
        public string WarehouseID { get; set; }
        /// <summary>
        /// 退货仓库名称
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
        /// 操作用户id
        /// </summary>
        public string CreateUserID { get; set; }
        /// <summary>
        /// 操作用户名称
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 退货详情dto
        /// </summary>
        public virtual List<SmartReturnDetailAdd> SmartReturnDetail { get; set; }
    }
}
