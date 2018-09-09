using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 手术预约状态0：未完成；1：正在进行中；2：已完成
    /// </summary>
    public enum SurgeryStatus
    {
        /// <summary>
        /// 0：未完成
        /// </summary>
        NoDone=0,
        /// <summary>
        /// 1：正在进行中
        /// </summary>
        Doing=1,
        /// <summary>
        /// 2：已完成
        /// </summary>
        Done=2
    }
}
