using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 今日科室客户
    /// </summary>
    public class DeptVisitToday
    {
        /// <summary>
        /// 顾客iD
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 姓名
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 上门时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
