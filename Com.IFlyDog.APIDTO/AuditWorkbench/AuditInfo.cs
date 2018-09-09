using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 查询审核信息
    /// </summary>
   public class AuditInfo
    {

        /// <summary>
        /// 审核时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 审核医生
        /// </summary>
        public string AuditUserName { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 审核意见
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 级别
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// 审核用户
        /// </summary>
        public string UserID { get; set; }
    }
}
