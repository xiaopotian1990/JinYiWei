using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 照片类型
    /// </summary>
    public enum PhotoType
    {
        /// <summary>
        /// 咨询
        /// </summary>
        [Description("咨询")]
        Consult = 1,
        /// <summary>
        /// 治疗前
        /// </summary>
        [Description("治疗前")]
        Before = 2,
        /// <summary>
        /// 治疗中
        /// </summary>
        [Description("治疗中")]
        Under = 3,
        /// <summary>
        /// 治疗后
        /// </summary>
        [Description("治疗后")]
        After = 4,
        /// <summary>
        /// 其他
        /// </summary>
        [Description("其他")]
        Other = 5
    }
}
