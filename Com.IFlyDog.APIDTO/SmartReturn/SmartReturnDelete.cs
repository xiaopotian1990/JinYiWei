using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{

    /// <summary>
    /// 删除退货出库信息
    /// </summary>
   public class SmartReturnDelete
    {

        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 退货出库id
        /// </summary>
        public string ReturnID { get; set; }
    }
}
