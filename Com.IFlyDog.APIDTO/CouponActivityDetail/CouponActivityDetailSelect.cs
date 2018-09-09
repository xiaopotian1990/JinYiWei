using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 卷活动详情查询
    /// </summary>
   public class CouponActivityDetailSelect
    {
        /// <summary>
        /// 卷id
        /// </summary>
        public string CategoryID { get; set; }
        /// <summary>
        /// 当前分页
        /// </summary>
        public int PageNum { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }
    }
}
