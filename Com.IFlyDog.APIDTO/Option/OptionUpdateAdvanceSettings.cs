using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 修改预收款成交设置
    /// </summary>
   public class OptionUpdateAdvanceSettings
    {
        /// <summary>
        /// 预收款成交设置code
        /// </summary>
        public string Option17 { get; set; }

        /// <summary>
        /// 预收款成交设置值
        /// </summary>
        public string AdvanceSettingsCodeValue { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
    }
}
