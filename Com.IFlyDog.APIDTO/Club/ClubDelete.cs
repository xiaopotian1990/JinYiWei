using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 单项目管理删除dto
    /// </summary>
   public class ClubDelete
    {
        /// <summary>
        /// ids
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 操作人id
        /// </summary>
        public long CreateUserID { get; set; }
    }
}
