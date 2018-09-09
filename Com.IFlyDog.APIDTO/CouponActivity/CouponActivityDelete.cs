using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 删除卷活动
    /// </summary>
   public class CouponActivityDelete
    {
        /// <summary>
        /// 卷活动id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        public long CreateUserID { get; set; }
    }
}
