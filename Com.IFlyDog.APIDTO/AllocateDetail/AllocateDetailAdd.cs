using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 添加仓库调拨详情dto
    /// </summary>
   public class AllocateDetailAdd
    {
      
        /// <summary>
        /// 详情id
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 库存id
        /// </summary>
        public string SmartStockID { get; set; }

        /// <summary>
        /// 库存id
        /// </summary>
        public string StockId { get; set; }
        /// <summary>
        /// 调拨信息id
        /// </summary>
        public string AllocateID { get; set; }
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
