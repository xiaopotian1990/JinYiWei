using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 查询药物品设置
    /// </summary>
   public class SmartProductInfo
    {
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 药物品id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 药物品id，主要给盘点使用
        /// </summary>
        public string ProductID { get; set; }

        /// <summary>
        /// 药物品名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 药物品规格
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// 拼音码
        /// </summary>
        public string PinYin { get; set; }
        /// <summary>
        /// 类型idsa
        /// </summary>
        public string CategoryID { get; set; }

        /// <summary>
        /// 药物品类型
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public string Price { get; set; }
        /// <summary>
        /// 库存单位id
        /// </summary>
        public string UnitID { get; set; }
        /// <summary>
        /// 库存单位
        /// </summary>
        public string KcName { get; set; }
        /// <summary>
        /// 使用单位id
        /// </summary>
        public string MiniUnitID { get; set; }
        /// <summary>
        /// 使用单位
        /// </summary>
        public string SYName { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public string Num { get; set; }
        /// <summary>
        /// 总价
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// 进制
        /// </summary>
        public string Scale { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }
    }
}
