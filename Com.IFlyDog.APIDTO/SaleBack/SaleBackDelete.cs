using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 删除回款记录
    /// </summary>
   public class SaleBackDelete
    {
        /// <summary>
        /// 回款记录id
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 操作用户id
        /// </summary>
        public string CreateUserID { get; set; }
        /// <summary>
        /// 店铺id
        /// </summary>
        public string StoreID { get; set; }
    }
}
