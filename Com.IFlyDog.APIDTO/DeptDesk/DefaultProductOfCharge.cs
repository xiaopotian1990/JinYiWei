using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 默认项目信息
    /// </summary>
    public class DefaultChargeInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DefaultChargeInfo()
        {
            Products = new List<DefaultProductsOfCharge>();
            DefaultProducts = new List<DefaultProductsOfCharge>();
        }
        /// <summary>
        /// 手术记录ID
        /// </summary>
        public string OperationID { get; set; }
        /// <summary>
        /// 项目ID
        /// </summary>
        public string ChargeID { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ChargeName { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 是否允许添加额外耗材0：否；1：是
        /// </summary>
        public int ProductAdd { get; set; }
        /// <summary>
        /// 耗材
        /// </summary>
        public IList<DefaultProductsOfCharge> Products { get; set; }
        /// <summary>
        /// 默认耗材
        /// </summary>
        public IEnumerable<DefaultProductsOfCharge> DefaultProducts { get; set; }
    }

    /// <summary>
    /// 耗材列表
    /// </summary>
    public class DefaultProductsOfCharge
    {
        /// <summary>
        /// 耗材记录ID
        /// </summary>
        public string OperationProductID { get; set; }
        /// <summary>
        /// 药物品ID
        /// </summary>
        public string ProductID { get; set; }
        /// <summary>
        /// 药物品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// 最小数量
        /// </summary>
        public int MinNum { get; set; }
        /// <summary>
        /// 最大数量
        /// </summary>
        public int MaxNum { get; set; }
        /// <summary>
        /// 仓库ID
        /// </summary>
        public string WarehouseID { get; set; }
        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName { get; set; }
        /// <summary>
        /// 发货状态
        /// </summary>
        public OperationProductStatus Status { get; set; }
    }
}
