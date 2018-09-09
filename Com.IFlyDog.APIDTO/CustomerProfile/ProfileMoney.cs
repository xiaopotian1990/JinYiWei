using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 客户档案账户情况
    /// </summary>
    public class ProfileMoney
    {
        /// <summary>
        /// 总余额
        /// </summary>
        public decimal Deposit { get; set; }
        /// <summary>
        /// 总全额
        /// </summary>
        public decimal Coupon { get; set; }
        /// <summary>
        /// 总积分
        /// </summary>
        public decimal Point { get; set; }
        /// <summary>
        /// 佣金
        /// </summary>
        public decimal Commission { get; set; }
        /// <summary>
        /// 预收款
        /// </summary>
        public IEnumerable<ProfileDeposit> Deposits { get; set; }
        /// <summary>
        /// 有效代金券
        /// </summary>
        public IEnumerable<ProfileCoupon> Coupons { get; set; }
        /// <summary>
        /// 失效代金券
        /// </summary>
        public IEnumerable<ProfileCoupon> OverDueCoupons { get; set; }
        /// <summary>
        /// 积分变动记录
        /// </summary>
        public IEnumerable<ProfilePointChange> PointChanges { get; set; }
        /// <summary>
        /// 预收款变动
        /// </summary>
        public IEnumerable<ProfileDepositChange> DepositChanges { get; set; }
        /// <summary>
        /// 券变动
        /// </summary>
        public IEnumerable<ProfileCouponChange> CouponChanges { get; set; }
        /// <summary>
        /// 佣金使用
        /// </summary>
        public IEnumerable<ProfileCommissionChange> CommissionChanges { get; set; }
    }

    /// <summary>
    /// 客户档案账户情况预收款
    /// </summary>
    public class ProfileDeposit
    {
        /// <summary>
        /// 预收款ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 购买日期
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 预收款类型
        /// </summary>
        public string DepositChargeName { get; set; }
        /// <summary>
        ///购买医院
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 获得方式
        /// </summary>
        public DepositType Access { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 剩余金额
        /// </summary>
        public decimal Rest { get; set; }
        /// <summary>
        /// 提交人
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
    /// <summary>
    /// 客户档案账户情况券
    /// </summary>
    public class ProfileCoupon
    {
        /// <summary>
        /// 券ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 购买日期
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 券类型ID
        /// </summary>
        public string CouponCategoryName { get; set; }
        /// <summary>
        /// 购买医院
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 券获得方式
        /// </summary>
        public CouponType Access { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 剩余金额
        /// </summary>
        public decimal Rest { get; set; }
        /// <summary>
        /// 提交人
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 券状态
        /// </summary>
        public CouponStatus Status { get; set; }
        /// <summary>
        /// 失效日期
        /// </summary>
        public DateTime OverDueTime { get; set; }
    }
    /// <summary>
    /// 积分变动记录
    /// </summary>
    public class ProfilePointChange
    {
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public PointType Type { get; set; }
        /// <summary>
        /// 操作医院
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string CreateUserName { get; set; }
    }

    /// <summary>
    /// 预收款变动
    /// </summary>
    public class ProfileDepositChange
    {
        /// <summary>
        /// 购买日期
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 预收款类型
        /// </summary>
        public string DepositChargeName { get; set; }
        /// <summary>
        /// 获得方式
        /// </summary>
        public DepositType Access { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 提交人
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    /// <summary>
    /// 券变动
    /// </summary>
    public class ProfileCouponChange
    {
        /// <summary>
        /// 购买日期
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 券类型ID
        /// </summary>
        public string CouponCategoryName { get; set; }
        /// <summary>
        /// 券获得方式
        /// </summary>
        public CouponType Access { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 提交人
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    /// <summary>
    /// 佣金使用
    /// </summary>
    public class ProfileCommissionChange
    {
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 券获得方式
        /// </summary>
        public CommissionType Type { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 提交人
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
