using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 添加结算
    /// </summary>
    public class SettlementAdd
    {
        /// <summary>
        /// 操作人
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 医院ID
        /// </summary>
        public long HospitalID { get; set; }
    }
}
