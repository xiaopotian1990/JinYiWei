using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 店铺负责人删除
    /// </summary>
   public class StoreManagerDelete
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 店铺id
        /// </summary>
        public string StoreID { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
    }
}
