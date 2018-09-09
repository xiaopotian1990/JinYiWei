using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 单个开发人员所有权设置
    /// </summary>
   public class SingleDeveLoperUserUpdate
    {
        /// <summary>
        /// 原开发人员
        /// </summary>
        public long OldUserID { get; set; }
        /// <summary>
        /// 新开发人员
        /// </summary>
        public long NewUserID { get; set; }

        /// <summary>
        /// 医院id
        /// </summary>
        public long HospitalID { get; set; }

        /// <summary>
        /// 创建用户
        /// </summary>
        public long CreateUserID { get; set; }
    }
}
