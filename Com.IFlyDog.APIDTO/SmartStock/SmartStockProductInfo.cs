using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    ///查询库存药品详情dto
    /// </summary>
    public class SmartStockProductInfo
    {
        /// <summary>
        /// 库存id
        /// </summary>
        public string StockId { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 药物品id
        /// </summary>
        public string ProductID { get; set; }
        /// <summary>
        /// 药物品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 拼音码
        /// </summary>
        public string PinYin { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// 分类id
        /// </summary>
        public string CategoryID { get; set; }
        /// <summary>
        /// 分类名称
        /// </summary>
        public string ProductCategoryName { get; set; }
        /// <summary>
        /// 库存单位名称
        /// </summary>
        public string UnitName { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public string Num { get; set; }
        /// <summary>
        /// 进价
        /// </summary>
        public string Price { get; set; }
        /// <summary>
        /// 总价
        /// </summary>
        public string Amount { get; set; }
        /// <summary>
        /// 进货单号
        /// </summary>
        public string No { get; set; }
        /// <summary>
        /// 进货批号
        /// </summary>
        public string Batch { get; set; }
        /// <summary>
        /// 有效期
        /// </summary>
        public string Expiration { get; set; }
    }
}
