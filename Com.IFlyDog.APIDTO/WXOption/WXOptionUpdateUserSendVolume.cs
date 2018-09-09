using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 更新被推荐用户送券
    /// </summary>
    public class WXOptionUpdateUserSendVolume
    {
        /// <summary>
        /// 被推荐用户送卷是否开启
        /// </summary>
        public string UserSendVolumeCode { get; set; }
        /// <summary>
        /// 被推荐用户送卷是否开启值
        /// </summary>
        public string UserSendVolumeValue { get; set; }

        /// <summary>
        /// 被推荐用户送卷卷类型code
        /// </summary>
        public string UserCouponCategoryCode { get; set; }

        /// <summary>
        /// 被推荐用户送卷卷类型value
        /// </summary>
        public string UserCouponCategoryValue { get; set; }

        /// <summary>
        /// 被推荐用户送卷金额Code
        /// </summary>
        public string CouponCategoryMoneyCode { get; set; }

        /// <summary>
        /// 被推荐用户送卷金额value
        /// </summary>
        public string CouponCategoryMoneyValue { get; set; }

        public long CreateUserID { get; set; }
    }
}
