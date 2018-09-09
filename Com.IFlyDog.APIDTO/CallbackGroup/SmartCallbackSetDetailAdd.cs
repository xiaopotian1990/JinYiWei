using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO.CallbackGroup
{
    /// <summary>
    /// 回访组设置添加
    /// </summary>
    public class SmartCallbackSetDetailAdd
    {
        /// <summary>
        /// ID
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 回访组ID
        /// </summary>
        public int CallbackGroupID { get; set; }
        ///// <summary>
        ///// 回访组ID
        ///// </summary>
        //public int CallbackGroupCategoryID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string DetailName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string DetailRemark { get; set; }
        /// <summary>
        /// 天数
        /// </summary>
        public int DetailDays { get; set; }

        /// <summary>
        /// 类型名称
        /// </summary>
        public string CategoryName { get; set; }
        /// <summary>
        /// 类型名称
        /// </summary>
        public int CategoryID { get; set; }

    }
}
