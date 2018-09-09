using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 激活券信息
    /// </summary>
    public class ProfileActiveCoupon
    {
        /// <summary>
        /// 激活码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 券活动
        /// </summary>
        public string ActiveName { get; set; }
        /// <summary>
        /// 券类型
        /// </summary>
        public string CouponCategoryName { get; set; }
        /// <summary>
        /// 券金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 发放日期
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// 有效日期
        /// </summary>
        public DateTime Expiration { get; set; }
        /// <summary>
        /// 是否允许重复使用
        /// </summary>
        public CouponActiveStatus IsRepetition { get; set; }
        /// <summary>
        /// 是否已经失效0：没有生效1：已经失效
        /// </summary>
        public CouponActiveStatus IsEfficacy { get; set; }
        /// <summary>
        /// 激活后的券ID
        /// </summary>
        public long? CouponID { get; set; }
        /// <summary>
        /// 券活动ID
        /// </summary>
        public long ActiveID { get; set; }
        /// <summary>
        /// 券ID
        /// </summary>
        public long CategoryID { get; set; }
    }

    /// <summary>
    /// 券激活
    /// </summary>
    public class ProfileActiveCouponAdd
    {
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public long CustomerID { get; set; }
        /// <summary>
        /// 激活码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 医院ID
        /// </summary>
        public long HospitalID { get; set; }
    }
}
