using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 审核记录查询
    /// </summary>
   public class AuditRecordSelect
    {
        /// <summary>
        /// 审核类型 项目单、退预收款单、退项目单 开发人变更，咨询人变更
        /// </summary>
        public string AuditType { get; set; }

        /// <summary>
        /// 审核开始时间
        /// </summary>
        public DateTime? AuditBeginTime { get; set; }
        /// <summary>
        /// 审核结束时间
        /// </summary>
        public DateTime? AuditEndTime { get; set; }

        /// <summary>
        /// 客户编号
        /// </summary>
        public string CustormNo { get; set; }

        public long HospitalID { get; set; }

        /// <summary>
        /// 当前分页
        /// </summary>
        public int PageNum { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }
    }
}
