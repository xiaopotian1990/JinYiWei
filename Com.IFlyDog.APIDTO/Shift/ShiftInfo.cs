using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 
    /// </summary>
    public class UserShiftTemp
    {
        public UserShiftTemp()
        {
            Shifts = new List<ShiftTemp>();
        }
        public string ID { get; set; }
        public string UserName { get; set; }
        public string DeptName { get; set; }

       public virtual List<ShiftTemp> Shifts { get; set; }
    }

    public class ShiftTemp
    {
        public string ShiftID { get; set; }
        public string CategoryID { get; set; }
        /// <summary>
        /// 班次类型id
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// 排班日期
        /// </summary>
        public DateTime ShiftDate { get; set; }
    }

    public class UserShiftInfo
    {
        public string UserID { get; set; }
        public string UserName { get; set; }
        public string DeptName { get; set; }
        public ShiftInfo Monday { get; set; }
        public ShiftInfo Tuesday { get; set; }
        public ShiftInfo Wednesday { get; set; }
        public ShiftInfo Thursday { get; set; }
        public ShiftInfo Friday { get; set; }
        public ShiftInfo Saturday { get; set; }
        public ShiftInfo Sunday { get; set; }
    }
    public class ShiftInfo
    {

        /// <summary>
        /// 排班id
        /// </summary>
        public string ShiftID { get; set; }
        public string UserID { get; set; }

        public string CategoryID { get; set; }
        public DateTime ShiftDate { get; set; }
        /// <summary>
        /// 班次类型id
        /// </summary>
        public string CategoryName { get; set; }
    }

    public class Shift
    {
        /// <summary>
        /// 标头集合
        /// </summary>
        public List<string> HeaderList { get; set; }

        /// <summary>
        /// 排班用户dto
        /// </summary>
        public virtual IEnumerable<UserShiftInfo> UserInfoList { get; set; }
    }

    /// <summary>
    /// 排班用户信息
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 账号
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DeptName { get; set; }
    }

    /// <summary>
    /// 排班日期
    /// </summary>
    public class ShiftDate
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// 排班日期
        /// </summary>
        public string ShiftDataTime { get; set; }
        /// <summary>
        /// 班次类型id
        /// </summary>
        public string ShiftCategoryID { get; set; }
        /// <summary>
        /// 班次类型名称
        /// </summary>
        public string ShiftCategoryName { get; set; }
    }
}
