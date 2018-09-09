using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 住院状态1：住院中；0：已出院
    /// </summary>
    public enum InpatientStatus
    {
        /// <summary>
        /// 住院
        /// </summary>
        [Description("住院")]
        In = 1,
        /// <summary>
        /// 出院
        /// </summary>
        [Description("出院")]
        Out = 0
    }
}
