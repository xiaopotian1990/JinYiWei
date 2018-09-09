using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 进货详情信息添加
    /// </summary>
   public class SmartPurchaseDetailAdd
    {
        /// <summary>
        /// 进货详情id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 进货信息id
        /// </summary>
        public string PurchaseID { get; set; }
        /// <summary>
        /// 药物品id
        /// </summary>
        public string ProductID { get; set; }
        /// <summary>
        /// 药物品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 药物品数量
        /// </summary>
        public string Num { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// 库存单位（因为是进货入库所以先查出库存单位）
        /// </summary>
        public string UnitName { get; set; }
        /// <summary>
        /// 进价
        /// </summary>
        public string Price { get; set; }
        /// <summary>
        /// 售价
        /// </summary>
        public string Amount { get; set; }
        /// <summary>
        /// 批号
        /// </summary>
        public string Batch { get; set; }
        /// <summary>
        /// 有效期
        /// </summary>
        public string Expiration { get; set; }
    }
}
