using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 新增排班管理dto
    /// </summary>
   public class ShiftAdd
    {
        /// <summary>
        /// 排班id
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }

        /// <summary>
        /// 排班日期dto
        /// </summary>
        public virtual List<ShiftDate> ShiftDateList { get; set; }
        

        /// <summary>
        /// 排班日期用户dto
        /// </summary>
        public virtual List<UserInfo> UserInfoList { get; set; }
    }
}
