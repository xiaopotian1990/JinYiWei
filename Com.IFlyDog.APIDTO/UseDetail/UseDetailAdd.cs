using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 科室领用详情
    /// </summary>
   public class UseDetailAdd
    {
        /// <summary>
        /// 科室领用详情id
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 库存id
        /// </summary>
        public string StockId { get; set; }

        /// <summary>
        /// 科室领用id
        /// </summary>
        public string UseID { get; set; }
        /// <summary>
        /// 药物品id
        /// </summary>
        public string ProductID { get; set; }
        /// <summary>
        /// 药物品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 规格
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// 库存单位
        /// </summary>
        public string KcName { get; set; }
        /// <summary>
        /// 进货单号
        /// </summary>
        public string JHNo { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 进价
        /// </summary>
        public string Price { get; set; }
        /// <summary>
        /// 总价
        /// </summary>
        public string Amount { get; set; }
    }
}
