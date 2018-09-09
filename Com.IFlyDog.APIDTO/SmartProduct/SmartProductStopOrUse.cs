using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 药物品类型使用/停用
    /// </summary>
   public class SmartProductStopOrUse
    {
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 药物品id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 使用状态1使用0：停用
        /// </summary>
        public string Status { get; set; }
    }
}
