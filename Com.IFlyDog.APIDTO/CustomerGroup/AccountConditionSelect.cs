using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 账户条件查询
    /// </summary>
   public class AccountConditionSelect
    {
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 医院ID，下拉菜单
        /// </summary>
        public long HospitalID { get; set; }
        /// <summary>
        /// 余额条件类型 >=、>、=、<=、<
        /// </summary>
        public string BalanceConditionType { get; set; }
        /// <summary>
        /// 余额金额
        /// </summary>
        public decimal? BalanceMoney { get; set; }

        /// <summary>
        /// 卷额条件类型 >=、>、=、<=、<
        /// </summary>
        public string CouponConditionType { get; set; }

        /// <summary>
        /// 卷金额
        /// </summary>
        public decimal? CouponMoney { get; set; }

        /// <summary>
        /// 积分条件类型 >=、>、=、<=、<
        /// </summary>
        public string PointConditionType { get; set; }

        /// <summary>
        /// 积分
        /// </summary>
        public decimal? PointCount { get; set; }

        /// <summary>
        /// 净手总额条件类型 >=、>、=、<=、<
        /// </summary>
        public string CleanConditionType { get; set; }

        /// <summary>
        /// 净手总额
        /// </summary>
        public decimal? CleanMoney { get; set; }
        /// <summary>
        /// 佣金总额条件类型 >=、>、=、<=、<
        /// </summary>
        public string CommissionType { get; set; }

        /// <summary>
        /// 佣金总额
        /// </summary>
        public decimal? Commission { get; set; }
    }
}
