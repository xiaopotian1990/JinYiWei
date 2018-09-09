using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 检测收费项目
    /// </summary>
   public class ChargeCheckItem
    {
        /// <summary>
        /// 收费项目名称
        /// </summary>
        public string ChargeName { get; set; }
        /// <summary>
        /// 状态0：停用1：使用
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 所属报表名称
        /// </summary>
        public string ChargeItemName { get; set; }
    }
}
