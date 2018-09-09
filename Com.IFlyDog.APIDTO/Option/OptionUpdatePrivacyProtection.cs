using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 修改隐私保护dto
    /// </summary>
   public class OptionUpdatePrivacyProtection
    {
        /// <summary>
        /// 隐私保护值code
        /// </summary>
        public string Option19 { get; set; }

        /// <summary>
        /// 隐私保护值
        /// </summary>
        public string PrivacyProtectionCodeValue { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
    }
}
