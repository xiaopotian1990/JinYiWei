using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    ///添加预收款项目
    /// </summary>
   public class DepositChargeAdd
    {
        /// <summary>
        /// 预收款id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 预收款类型名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 预收款状态 状态0：停用1：使用
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public string Price { get; set; }
        /// <summary>
        /// 使用范围限制1：无限制2：按照项目分类进行限制3：按照指定项目进行限制
        /// </summary>
        public string ScopeLimit { get; set; }
        /// <summary>
        /// 使用范围限制值
        /// </summary>
        public string ScopeLimitValue { get; set; }
        /// <summary>
        /// 是否赠送代金券0：否1：是
        /// </summary>
        public string HasCoupon { get; set; }
        /// <summary>
        /// 卷类型id
        /// </summary>
        public string CouponCategoryID { get; set; }

        /// <summary>
        /// 卷金额
        /// </summary>
        public string CouponAmount { get; set; }

        /// <summary>
        /// 创建用户
        /// </summary>
        public long CreateUserID { get; set; }

        /// <summary>
        /// 项目分类详细dto
        /// </summary>
        public virtual List<SmartDepositChargeChargeCategoryAdd> SmartDepositChargeChargeCategoryAdd { get; set; }

        /// <summary>
        /// 预收款收费项目dto
        /// </summary>
        public virtual List<SmartDepositChargeChargeAdd> SmartDepositChargeChargeAdd { get; set; }

        /// <summary>
        /// 收费项目映射医院表
        /// </summary>
        public virtual List<SmartDepositChargeHospitalAdd> SmartDepositChargeHospitalAdd { get; set; }
    }

    /// <summary>
    /// 项目分类详细dto
    /// </summary>
    public class SmartDepositChargeChargeCategoryAdd
    {/// <summary>
    /// sid
    /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 预收款id
        /// </summary>
        public string DepositChargeID { get; set; }
        /// <summary>
        /// 项目分类id
        /// </summary>
        public string ChargeCategoryID { get; set; }
        /// <summary>
        /// 项目分类名称
        /// </summary>
        public string ChargeCategoryName { get; set; }

    }
    /// <summary>
    /// 预收款收费项目dto
    /// </summary>
    public class SmartDepositChargeChargeAdd {
        /// <summary>
        /// 映射收费项目id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 预收款id
        /// </summary>
        public string DepositChargeID { get; set; }
        /// <summary>
        /// 收费项目id
        /// </summary>
        public string ChargeID { get; set; }
        /// <summary>
        /// 收费项目名称
        /// </summary>
        public string ChargeName { get; set; }
    }

    /// <summary>
    /// 收费项目映射医院表
    /// </summary>
    public class SmartDepositChargeHospitalAdd {
        /// <summary>
        /// 映射医院项目id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 预收款类型id
        /// </summary>
        public string DepositChargeID { get; set; }
        /// <summary>
        /// 可使用的医院id
        /// </summary>
        public string HospitalID { get; set; }
        /// <summary>
        /// 医院名称
        /// </summary>
        public string HospitalName { get; set; }
    }
}
