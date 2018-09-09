using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 用户添加
    /// </summary>
    public class UserAdd
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 账户
        /// </summary>
        public string Account { get; set; }
        /// <summary>
        /// 名字
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 男女1：男2：女
        /// </summary>
        public GenderEnum Gender { get; set; }
        /// <summary>
        /// 部门ID
        /// </summary>
        public long DeptID { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        /// 座机
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        /// 操作人所属医院ID
        /// </summary>
        public long UserHospitalID { get; set; }
        /// <summary>
        /// 新建用户所属医院ID
        /// </summary>
        public long HospitalID { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 角色ID
        /// </summary>
        public IEnumerable<long> Roles { get; set; }
    }
}
