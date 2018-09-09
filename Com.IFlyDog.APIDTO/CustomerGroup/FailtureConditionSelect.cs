using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 未成交条件查询
    /// </summary>
    public class FailtureConditionSelect
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
        /// 未成交类型id
        /// </summary>
        public long FailtureCategoryID { get; set; }

        /// <summary>
        /// 提交开始时间
        /// </summary>
        public DateTime? SubmitBeginTime { get; set; }

        /// <summary>
        /// 提交结束时间
        /// </summary>
        public DateTime? SubmitEndTime { get; set; }
    }
}
