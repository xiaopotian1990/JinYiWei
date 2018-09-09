using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 删除卷活动详情
    /// </summary>
    public class CouponActivityDetailDelete
    {
        /// <summary>
        ///卷活动详情id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 卷活动id
        /// </summary>
        public string ActivityID { get; set; }

        /// <summary>
        /// 操作人id
        /// </summary>
        public long CreateUserID { get; set; }
    }
}
