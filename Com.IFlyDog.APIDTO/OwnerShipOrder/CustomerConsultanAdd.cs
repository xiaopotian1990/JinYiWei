using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 添加咨询人员变更申请
    /// </summary>
    public class CustomerConsultanAdd
    {
        public long ID { get; set; }

        /// <summary>
        /// 原咨询人员
        /// </summary>
        public long OldUserID { get; set; }
        /// <summary>
        /// 新咨询人员
        /// </summary>
        public long NewUserID { get; set; }

        /// <summary>
        /// 医院id
        /// </summary>
        public long HospitalID { get; set; }

        /// <summary>
        /// 创建用户
        /// </summary>
        public long CreateUserID { get; set; }

        /// <summary>
        /// 客户id
        /// </summary>
        public long CustomerID { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 变更原因
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 审核状态 待审核 1
        /// </summary>
        public int AuditStatus { get; set; }
    }
}
