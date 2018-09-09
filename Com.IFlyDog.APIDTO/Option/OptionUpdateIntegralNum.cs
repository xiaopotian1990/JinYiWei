using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 修改积分比例dto
    /// </summary>
   public class OptionUpdateIntegralNum
    {
        /// <summary>
        /// 积分数量code
        /// </summary>
        public string Option16 { get; set; }

        /// <summary>
        /// 积分数量值
        /// </summary>
        public string IntegralNumCodeValue { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
    }
}
