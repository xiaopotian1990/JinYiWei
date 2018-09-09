using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 修改排班管理
    /// </summary>
   public class ShiftUpdate
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 排班日期
        /// </summary>
        public string DataTimeShif { get; set; }

        /// <summary>
        /// 排班类型id
        /// </summary>
        public string ShiftCategoryID { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
    }
}
