using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 仓库管理启用停用dto
    /// </summary>
   public class SmartWarehouseStopOrUse
    {
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }

        /// <summary>
        /// 仓库管理id
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 状态 1：使用；0：停用
        /// </summary>
        public int Status { get; set; }
    }
}
