using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 库存查询dto
    /// </summary>
   public class SmartStockInfo
    {
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }

        /// <summary>
        /// 库存查询id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 库存id，（这里先写了两个，主要是为了库存盘点，）
        /// </summary>
        public string StockId { get; set; }

        /// <summary>
        /// 仓库id
        /// </summary>
        public string WarehouseID { get; set; }
        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName { get; set; }
        /// <summary>
        ///分类id
        /// </summary>

        public string CategoryID { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string ProductCategoryName { get; set; }
        /// <summary>
        /// 药物品名称
        /// </summary>
        public string WPName { get; set; }

        /// <summary>
        /// 药物品id
        /// </summary>
        public string ProductID { get; set; }

        /// <summary>
        /// 药物品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 药物品规格
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// 拼音码
        /// </summary>
        public string PinYin { get; set; }
        /// <summary>
        /// 单位名称
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        public string Price { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Num { get; set; }
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
