using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 手术药物品状态）：未发货1：已发货
    /// </summary>
    public enum OperationProductStatus
    {
        /// <summary>
        /// 未发货
        /// </summary>
        [Description("未发货")]
        No=0,
        /// <summary>
        /// 已发货
        /// </summary>
        [Description("已发货")]
        Yes =1
    }
}
