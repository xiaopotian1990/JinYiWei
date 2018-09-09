using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 修改是否允许欠款dto
    /// </summary>
   public class OptionUpdateAllowArrears
    {
        /// <summary>
        /// 是否允许欠款code
        /// </summary>
        public string Option18 { get; set; }

        /// <summary>
        /// 是否允许欠款值
        /// </summary>
        public string AllowArrearsCodeValue { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
    }
}
