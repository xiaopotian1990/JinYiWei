using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 报表项目组所属项目
    /// </summary>
  public  class ItemInfo
    {
        /// <summary>
        /// 项目组名称
        /// </summary>
        public string GroupID { get; set; }
        /// <summary>
        /// 报表项目名称
        /// </summary>
        public string Name { get; set; }
    }
}
