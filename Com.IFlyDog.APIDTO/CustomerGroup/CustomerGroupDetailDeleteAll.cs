using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 清空客户组详情关联的客户
    /// </summary>
   public class CustomerGroupDetailDeleteAll
    {
        /// <summary>
        /// 客户组id
        /// </summary>
        public long CustomerGroupID { get; set; }


        /// <summary>
        /// 创建用户
        /// </summary>
        public long CreateUserID { get; set; }
    }
}
