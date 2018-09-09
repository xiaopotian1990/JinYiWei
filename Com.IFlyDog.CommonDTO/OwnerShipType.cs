using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 顾客归属关系类型
    /// </summary>
    public enum OwnerShipType
    {
        /// <summary>
        /// 网电咨询师
        /// </summary>
        [Description("网电咨询师")]
        Exploit=1,
        /// <summary>
        /// 现场咨询师
        /// </summary>
        [Description("现场咨询师")]
        Manager =2
    }
}
