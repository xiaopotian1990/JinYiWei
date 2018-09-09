using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 删除采购发票dto
    /// </summary>
   public class InvoiceDelete
    {
        /// <summary>
        /// 操作用户id
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 采购发票id
        /// </summary>
        public string InvoiceID { get; set; }
    }
}
