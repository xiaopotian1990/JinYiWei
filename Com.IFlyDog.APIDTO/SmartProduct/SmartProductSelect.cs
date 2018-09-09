using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 药物品查询dto
    /// </summary>
   public class SmartProductSelect
    {
        /// <summary>
        /// 拼音吗
        /// </summary>
        public string PinYin { get; set; }
        /// <summary>
        /// 药物品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 药物品分类
        /// </summary>
        public string CategoryId { get; set; }
        /// <summary>
        /// 使用状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 仓库id
        /// </summary>
        public string WarehouseID { get; set; }
        /// <summary>
        /// 类型1：入库2：调拨3：盘点4：科室领用
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 入库单号
        /// </summary>
        public string No { get; set; }
        /// <summary>
        /// 当前分页
        /// </summary>
        public int PageNum { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }
    }
}
