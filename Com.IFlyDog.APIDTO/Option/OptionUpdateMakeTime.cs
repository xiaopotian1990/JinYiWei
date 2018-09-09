using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 修改预约设置
    /// </summary>
   public class OptionUpdateMakeTime
    {
        /// <summary>
        /// 预约开始时间Code
        /// </summary>
        public string Option13 { get; set; }
        /// <summary>
        /// 预约开始时间值
        /// </summary>
        public string MakeBeginTimeCodeValue { get; set; }

        /// <summary>
        /// 预约结束时间Code
        /// </summary>
        public string Option14 { get; set; }

        /// <summary>
        /// 预约结束时间值
        /// </summary>
        public string MakeEndTimeCodeValue { get; set; }

        /// <summary>
        /// 预约时间间隔Code
        /// </summary>
        public string Option15 { get; set; }

        /// <summary>
        /// 预约时间间隔值
        /// </summary>
        public string MakeTimeIntervalCodeValue { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
    }
}
