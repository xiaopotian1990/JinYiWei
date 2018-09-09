using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 客户访问权限
    /// </summary>
    public class UserCustomerPermission
    {
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 配置的用户ID
        /// </summary>
        public long UserID { get; set; }
        /// <summary>
        /// 操作人医院ID
        /// </summary>
        public long UserHospitalID { get; set; }
        /// <summary>
        /// 可操作医院列表
        /// </summary>
        public IEnumerable<long> Hospitals { get; set; }
        /// <summary>
        /// 可操作部门列表
        /// </summary>
        public IEnumerable<long> Depts { get; set; }
        /// <summary>
        /// 可操作用户列表
        /// </summary>
        public IEnumerable<long> Users { get; set; }
    }

    /// <summary>
    /// 获取权限详细信息
    /// </summary>
    public class UserCustomerPermissionDetail
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 部门
        /// </summary>
        public IEnumerable<DeptSelect> Depts { get; set; }
        /// <summary>
        /// 医院
        /// </summary>

        public IEnumerable<HospitalSelect> Hospitals { get; set; }
        /// <summary>
        /// 用户
        /// </summary>
        public IEnumerable<UserSelect> Users { get; set; }
    }
}
