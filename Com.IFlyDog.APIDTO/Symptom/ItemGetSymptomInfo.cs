using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 检测症状用dto
    /// </summary>
   public class ItemGetSymptomInfo
    {
        /// <summary>
        /// 症状名称
        /// </summary>
        public string SymptonName { get; set; }

        /// <summary>
        /// 状态0：停用1：使用
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 所属报表项目名称
        /// </summary>
        public string ItemName { get; set; }
    }
}
