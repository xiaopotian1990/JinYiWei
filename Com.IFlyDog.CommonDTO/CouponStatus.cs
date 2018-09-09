using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 代金券状态
    /// </summary>
    public enum CouponStatus
    {
        /// <summary>
        /// 有效
        /// </summary>
        Effective=1,
        /// <summary>
        /// 失效
        /// </summary>
        OverDue=2,
        /// <summary>
        /// 已经使用
        /// </summary>
        //Use=3
    }
}
