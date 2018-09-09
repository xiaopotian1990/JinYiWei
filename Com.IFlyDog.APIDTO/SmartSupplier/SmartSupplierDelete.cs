using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 供应商管理删除DTO类
    /// </summary>
   public class SmartSupplierDelete
    {
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }

        /// <summary>
        /// 供应商管理Id
        /// </summary>
        public string ID { get; set; }
    }
}
