using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 删除仓库调拨dto
    /// </summary>
   public class AllocateDelete
    {
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 仓库调拨id
        /// </summary>
        public string AllocateID { get; set; }
    }
}
