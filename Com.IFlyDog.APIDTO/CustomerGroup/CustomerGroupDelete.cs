using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 删除客户组
    /// </summary>
   public class CustomerGroupDelete
    {
        /// <summary>
        /// 客户组id
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// 创建用户
        /// </summary>
        public long CreateUserID { get; set; }
    }
}
