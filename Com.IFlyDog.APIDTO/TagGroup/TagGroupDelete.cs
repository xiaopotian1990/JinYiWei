using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 删除标签组
    /// </summary>
   public class TagGroupDelete
    {
        /// <summary>
        /// 标签id
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
    }
}
