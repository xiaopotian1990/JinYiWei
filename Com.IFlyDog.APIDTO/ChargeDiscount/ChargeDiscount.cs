using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 项目折扣
    /// </summary>
    public class ChargeDiscount
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        public long ChargeID { get; set; }
        /// <summary>
        /// 折扣
        /// </summary>
        public int Discount { get; set; }
    }
}
