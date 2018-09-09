using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 客户组查询
    /// </summary>
   public class CustomerGroupSelect
    {
        /// <summary>
        /// 创建用户
        /// </summary>
        public long createUserID { get; set; }
        /// <summary>
        /// 客户组名称
        /// </summary>
        public string Name { get; set; }
    }
}
