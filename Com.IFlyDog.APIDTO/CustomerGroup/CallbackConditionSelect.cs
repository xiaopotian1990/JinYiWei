using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 回访条件查询
    /// </summary>
   public class CallbackConditionSelect
    {
        /// <summary>
        /// 回访类型id
        /// </summary>
        public long CallbackCategoryID { get; set; }

        /// <summary>
        /// 回访方式
        /// </summary>
        public string CallbackType { get; set; }

        /// <summary>
        /// 提醒开始时间
        /// </summary>
        public DateTime? TaskBeginTime { get; set; }

        /// <summary>
        /// 提醒结束时间
        /// </summary>
        public DateTime? TaskEndTime { get; set; }

        /// <summary>
        /// 回访开始时间
        /// </summary>
        public DateTime? CallbackBeginTime { get; set; }

        /// <summary>
        /// 回访结束时间
        /// </summary>
        public DateTime? CallbackEndTime { get; set; }

        /// <summary>
        /// 最后回访开始时间
        /// </summary>
        public DateTime? LastCallbackBeginTime { get; set; }

        /// <summary>
        /// 最后回访结束时间
        /// </summary>
        public DateTime? LastCallbackEndTime { get; set; }

        /// <summary>
        /// 提醒内容
        /// </summary>
        public string TaskContent { get; set; }

        /// <summary>
        /// 回访内容
        /// </summary>
        public string CallbackContent { get; set; }

        public long CreateUserID { get; set; }
    }
}
