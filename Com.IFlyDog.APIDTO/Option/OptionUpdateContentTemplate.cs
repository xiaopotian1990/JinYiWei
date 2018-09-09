using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 修改默认咨询模板dto
    /// </summary>
   public class OptionUpdateContentTemplate
    {
        /// <summary>
        /// 用户咨询模板code
        /// </summary>
        public string Option11 { get; set; }
        /// <summary>
        /// 用户咨询模板值
        /// </summary>
        public string ContentTemplateCodeValue { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
    }
}
