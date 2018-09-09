using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 等候开启dto
    /// </summary>
    public class OptionUpdateWaitingDiagnosis
    {
        /// <summary>
        /// 等候诊断code
        /// </summary>
        public string Option22 { get; set; }

        /// <summary>
        /// 等候诊断值
        /// </summary>
        public string WaitingDiagnosisCodeValue { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
    }
}
