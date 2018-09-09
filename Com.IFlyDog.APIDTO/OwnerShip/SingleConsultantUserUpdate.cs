using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 单个咨询人员所有权设置
    /// </summary>
   public class SingleConsultantUserUpdate
    {
        /// <summary>
        /// 原咨询人员
        /// </summary>
        public long OldUserID { get; set; }
        /// <summary>
        /// 新咨询人员
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
