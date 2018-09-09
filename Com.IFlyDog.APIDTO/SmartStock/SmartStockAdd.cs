using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 新增库存dto
    /// </summary>
   public class SmartStockAdd
    {

        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }

        /// <summary>
        /// 库存id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        ///仓库id
        /// </summary>
        public string WarehouseID { get; set; }
        /// <summary>
        /// 药物品id
        /// </summary>
        public string ProductID { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public string Num { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public string Price { get; set; }
        /// <summary>
        /// 总价
        /// </summary>
        public string Amount { get; set; }
        /// <summary>
        /// 具体的入库id
        /// </summary>
        public string DetailID { get; set; }
        /// <summary>
        /// 类型1：入库2：调拨3：盘点4：科室领用
        /// </summary>
        public int Type { get; set; }

    }
}
