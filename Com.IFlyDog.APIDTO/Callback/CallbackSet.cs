using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 回访计划
    /// </summary>
    public class CallbackSet
    {
        /// <summary>
        /// 回访计划ID
        /// </summary>
        public string SetID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
    /// <summary>
    /// 回访计划详细
    /// </summary>
    public class CallbackSetDetail
    {
        /// <summary>
        /// 回访类型
        /// </summary>
        public string CallbackCategoryName { get; set; }
        /// <summary>
        /// 说明
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 天数
        /// </summary>
        public int Days { get; set; }
    }
}
