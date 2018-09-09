using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 审核操作dto类
    /// </summary>
   public class AuditOperationAdd
    {
        /// <summary>
        /// 审核id
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 当前用户所属的审核级别
        /// </summary>
        public int UserLevel { get; set; }

        /// <summary>
        /// 这条数据所属的最大审核级别
        /// </summary>
        public int MaxLevel { get; set; }

        /// <summary>
        /// 数据id
        /// </summary>
        public long AuditDataID { get; set; }
        /// <summary>
        /// 审核用户id
        /// </summary>
        public long AuditUserID { get; set; }
        /// <summary>
        /// 审核时间
        /// </summary>
        public DateTime AuditTime { get; set; }
        /// <summary>
        /// 审核状态 1 待审核 2 无需审核 3 审核不通过 4 审核通过 5 未提交
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 审核意见
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 审核类型 1 项目单  3 退预收款单 2 退项目单 4 开发 5 咨询
        /// </summary>
        public int AutitType { get; set; }

        /// <summary>
        /// 原用户id
        /// </summary>
        public long OldID { get; set; }

        /// <summary>
        /// 新用户id
        /// </summary>
        public long NewID { get; set; }

        /// <summary>
        /// 客户id
        /// </summary>
        public long CustomerID { get; set; }

        /// <summary>
        /// 医院id
        /// </summary>
        public long HospitalID { get; set; }
    }
}
