using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 耗材添加
    /// </summary>
    public class OperationProductAdd
    {
        /// <summary>
        /// 划扣ID
        /// </summary>
        public long OperationID { get; set; }
        /// <summary>
        /// 药物品ID
        /// </summary>
        public long ProductID { get; set; }
        /// <summary>
        /// 仓库ID
        /// </summary>
        public long WarehouseID { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public long Num { get; set; }
    }
}
