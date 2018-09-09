using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 编辑回款
    /// </summary>
   public class SaleBackUpdate
    {
        /// <summary>
        /// 回款id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 回款日期
        /// </summary>
        public string CreateDate { get; set; }

        /// <summary>
        /// 回款金额
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 操作用户
        /// </summary>
        public string CreateUserID { get; set; }
        /// <summary>
        /// 店铺id
        /// </summary>
        public string StoreID { get; set; }
    }
}
