using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 我的审核申请查询参数
    /// </summary>
  public  class AuditApplySelect
    {

        /// <summary>
        /// 1 待审核  3 审核不通过 4 审核通过
        /// </summary>
        public int AuditType { get; set; }

        /// <summary>
        /// 提交开始日期
        /// </summary>
        public DateTime? BeginTime { get; set; }

        /// <summary>
        /// 提交结束日期
        /// </summary>
        public DateTime? EndTime { get; set; }

        /// <summary>
        /// 客户编号
        /// </summary>
        public string CustomerID { get; set; }

        /// <summary>
        /// 创建用户id
        /// </summary>
        public long CreateUserId { get; set; }

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
