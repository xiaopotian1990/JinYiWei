using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 更新药物品设置dto
    /// </summary>
   public class SmartProductUpdate
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
        /// 药物品名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 拼音码
        /// </summary>
        public string PinYin { get; set; }
        /// <summary>
        /// 分类id
        /// </summary>
        public string CategoryID { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public string Price { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 库存单位s
        /// </summary>
        public string UnitID { get; set; }
        /// <summary>
        /// 使用单位
        /// </summary>
        public string MiniUnitID { get; set; }

        /// <summary>
        /// 进制
        /// </summary>
        public string Scale { get; set; }
    }
}
