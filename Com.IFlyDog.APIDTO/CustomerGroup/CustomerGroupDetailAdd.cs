using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 添加客户组详情客户
    /// </summary>
    public class CustomerGroupDetailAdd
    {

        /// <summary>
        /// 客户组id
        /// </summary>
        public long CustomerGroupID { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public long CreateUserID { get; set; }

        /// <summary>
        ///用户输入的客户集合
        /// </summary>
        public virtual List<UserList> UserListAdd { get; set; }
    }

    /// <summary>
    /// 用户输入的客户id集合
    /// </summary>
    public class UserList
    {
        /// <summary>
        /// 客户id
        /// </summary>
        public long UserID { get; set; }
    }
}
