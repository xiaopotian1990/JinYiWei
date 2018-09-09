using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 修改挂号是否开启dto
    /// </summary>
   public class OptionUpdateRegistration
    {
        /// <summary>
        /// 挂号是否开启code
        /// </summary>
        public string Option20 { get; set; }

        /// <summary>
        /// 挂号是否开启值
        /// </summary>
        public string RegistrationCodeValue { get; set; }

        /// <summary>
        /// 挂号收费项目code
        /// </summary>
        public string Option21 { get; set; }

        /// <summary>
        /// 挂号收费项目id
        /// </summary>
        public string RegistrationChargeCodeValue { get; set; }

        /// <summary>
        /// 挂号收费项目名称
        /// </summary>
        public string RegistrationChargeName { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
    }
}
