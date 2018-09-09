using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 会员条件查询
    /// </summary>
   public class MemberConditionSelect
    {
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 医院ID，下拉菜单
        /// </summary>
        public long HospitalID { get; set; }
        /// <summary>
        /// 会员类型id
        /// </summary>
        public long MemberCategoryID { get; set; }

        /// <summary>
        /// 加入开始时间
        /// </summary>
        public DateTime? JoinBeginTime { get; set; }

        /// <summary>
        /// 加入结束时间
        /// </summary>
        public DateTime? JoinEndTime { get; set; }
    }
}
