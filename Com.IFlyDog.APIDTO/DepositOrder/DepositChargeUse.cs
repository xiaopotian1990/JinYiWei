using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 医院可使用预收款
    /// </summary>
    public class DepositChargeHospitalUseTemp
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int ScopeLimit { get; set; }
        public string ChargeName { get; set; }
        public string ChargeCategoryName { get; set; }
        public int HasCoupon { get; set; }
        public string CouponCategoryName { get; set; }
        public decimal CouponAmount { get; set; }
    }

    /// <summary>
    /// 添加预收款时查询出的可使用预收款
    /// </summary>
    public class DepositChargeHospitalUse
    {
        /// <summary>
        /// 预收款类型ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 使用范围
        /// </summary>
        public string ScopeLimit { get; set; }
        /// <summary>
        /// 赠券
        /// </summary>
        public string HasCoupon { get; set; }
        /// <summary>
        /// 赠券金额
        /// </summary>
        public decimal CouponAmount { get; set; }
    }
}
