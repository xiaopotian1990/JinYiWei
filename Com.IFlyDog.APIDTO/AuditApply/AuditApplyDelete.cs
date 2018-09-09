using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 取消我的审核（就是删除）
    /// </summary>
   public class AuditApplyDelete
    {
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }

        /// <summary>
        /// 数据id
        /// </summary>
        public long OrderID { get; set; }

        /// <summary>
        /// 数据类型 4 开发人员 5咨询人员
        /// </summary>
        public string OrderType { get; set; }

    }
}
