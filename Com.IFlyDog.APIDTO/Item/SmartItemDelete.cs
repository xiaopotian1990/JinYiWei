using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
 /// <summary>
 /// 删除报表项目dto
 /// </summary>
   public class SmartItemDelete
    {
        /// <summary>
        /// 报表项目dto
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
    }
}
