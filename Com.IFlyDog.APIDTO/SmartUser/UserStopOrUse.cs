using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 用户使用停用
    /// </summary>
    public class UserStopOrUse
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserID { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 用户所在医院
        /// </summary>
        public long HospitalID { get; set; }
        /// <summary>
        /// 操作人所在ID
        /// </summary>
        public long UserHospitalID { get; set; }
        /// <summary>
        /// 状态0：停用1：使用
        /// </summary>
        public CommonStatus Status { get; set; }
    }

    /// <summary>
    /// 用户密码重置
    /// </summary>
    public class UserPasswordReset
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long UserID { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 用户所在医院
        /// </summary>
        public long HospitalID { get; set; }
        /// <summary>
        /// 操作人所在ID
        /// </summary>
        public long UserHospitalID { get; set; }
    }
}
