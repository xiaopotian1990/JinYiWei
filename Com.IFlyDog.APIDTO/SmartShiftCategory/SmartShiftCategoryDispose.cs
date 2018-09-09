using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 启用停用班次管理
    /// </summary>
   public class SmartShiftCategoryDispose
    {
        /// <summary>
        /// 班级信息id
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 启用/停用状态
        /// </summary>
        public string Status { get; set; }
    }
}
