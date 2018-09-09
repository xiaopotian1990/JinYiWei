using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 角色删除
    /// </summary>
    public class RoleDelete
    {
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 操作人所在医院
        /// </summary>
        public long UserHospitalID { get; set; }
        /// <summary>
        /// 角色所属医院
        /// </summary>
        public long HospitalID { get; set; }
        /// <summary>
        /// 角色管理ID
        /// </summary>
        public long RoleID { get; set; }
    }
}
