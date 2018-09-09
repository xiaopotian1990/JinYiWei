using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 排班管理查询
    /// </summary>
  public  class ShiftSelect
    {
        /// <summary>
        /// 0 本周  1上一周  2 下一周
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 如果是上周，需要减的日期，如果是下周，需要加的日期
        /// </summary>
        public string Value { get; set; }
    }
}
