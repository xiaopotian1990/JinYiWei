using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 查询收费项目
    /// </summary>
   public class ChargeInfo
    {
        /// <summary>
        /// 收费项目id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 拼音码
        /// </summary>
        public string PinYin { get; set; }
        /// <summary>
        /// 单位名称
        /// </summary>
        public string UnitID { get; set; }
        /// <summary>
        /// 单位名称
        /// </summary>
        public string UnitName { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public string Price { get; set; }
        /// <summary>
        /// 项目分类id
        /// </summary>
        public string CategoryID { get; set; }
        /// <summary>
        /// 项目分类名称
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// 是否允许增加耗材0 允许， 1不允许
        /// </summary>
        public string ProductAdd { get; set; }
        /// <summary>
        /// 使用状态 0：停用1：使用
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 药物品设置详细DTO
        /// </summary>
        public virtual List<SmartChargeProductDetailAdd> SmartChargeProductDetailAdd { get; set; }
    }
}
