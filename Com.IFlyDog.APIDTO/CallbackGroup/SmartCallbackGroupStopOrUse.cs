using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO.CallbackGroup
{
    /// <summary>
    /// 回访组 使用及停用
    /// </summary>
    public class SmartCallbackGroupStopOrUse
    {
        /// <summary>
        /// ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// ID
        /// </summary>
        public long CallbackGroupID { get; set; }
        /// <summary>
        /// 状态 1：使用；0：停用；2：删除
        /// </summary>
        public CallbackGroupStatusType Status { get; set; }
    }
}
