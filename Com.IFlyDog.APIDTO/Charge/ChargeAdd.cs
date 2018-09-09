using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 添加收费项目
    /// </summary>
   public class ChargeAdd
    {

        /// <summary>
        /// 创建人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 主键
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 项目分类id
        /// </summary>
        public string CategoryID { get; set; }
        /// <summary>
        /// 拼音吗
        /// </summary>
        public string PinYin { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public string Price { get; set; }
        /// <summary>
        ///  状态 0：停用1：使用
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string UnitID { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string Size { get; set; }

        /// <summary>
        /// 是否允许耗材  0 允许， 1不允许
        /// </summary>
        public string ProductAdd { get; set; }

        /// <summary>
        /// 药物品设置详细DTO
        /// </summary>
        public virtual List<SmartChargeProductDetailAdd> SmartChargeProductDetailAdd { get; set; }
    }

    /// <summary>
    /// 药物品设置详细DTO
    /// </summary>
    public class SmartChargeProductDetailAdd
    {
        /// <summary>
        /// ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 收费项目ID
        /// </summary>
        public string ChargeID { get; set; }
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
        /// 药物品单位
        /// </summary>
        public string UnitName { get; set; }

        /// <summary>
        /// 最小数量
        /// </summary>
        public string  MinNum { get; set; }
        /// <summary>
        /// 最大数量
        /// </summary>
        public string MaxNum { get; set; }

    }
}
