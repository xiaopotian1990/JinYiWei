using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 删除进货信息，进货信息详情
    /// </summary>
   public class SmartPurchaseDelete
    {
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 进货信息id
        /// </summary>
        public string PurchaseID { get; set; }
    }
}
