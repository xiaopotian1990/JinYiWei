using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 添加卷类型dto
    /// </summary>
   public class CouponCategoryAdd
    {
        /// <summary>
        /// 卷类型主键
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 卷类型名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 状态 状态0：停用1：使用
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 使用范围限制1：无限制2：按照项目分类进行限制3：按照指定项目进行限制
        /// </summary>
        public string ScopeLimit { get; set; }
  
        /// <summary>
        /// 使用时间限制1：无限制2：指定日期之前3：生效之后N天
        /// </summary>
        public string TimeLimit { get; set; }
        /// <summary>
        /// /当TimeLimit为2时，代表指定日期
        /// </summary>
        public string EndDate { get; set; }
        /// <summary>
        /// 当TimeLimit为3时，代表生效之后多少天
        /// </summary>
        public string Days { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public long CreateUserID { get; set; }

        /// <summary>
        ///卷类型项目映射dto
        /// </summary>
        public virtual List<SmartCouponCategoryChargeAdd> SmartCouponCategoryChargeAdd { get; set; }

        /// <summary>
        /// 卷类型项目分类映射dto
        /// </summary>
        public virtual List<SmartCouponCategoryChargeCategoryAdd> SmartCouponCategoryChargeCategoryAdd { get; set; }

        /// <summary>
        ///卷类型医院映射表dto
        /// </summary>
        public virtual List<SmartCouponCategoryHospitalAdd> SmartCouponCategoryHospitalAdd { get; set; }
    }

    /// <summary>
    /// 卷类型项目映射dto
    /// </summary>
    public class SmartCouponCategoryChargeAdd
    {/// <summary>
     /// sid
     /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 卷类型id
        /// </summary>
        public string CouponCategoryID { get; set; }
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
    /// 卷类型项目分类映射dto
    /// </summary>
    public class SmartCouponCategoryChargeCategoryAdd
    {
        /// <summary>
        /// 映射收费项目id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 卷类型id
        /// </summary>
        public string CouponCategoryID { get; set; }
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
    /// 卷类型医院映射表dto
    /// </summary>
    public class SmartCouponCategoryHospitalAdd
    {
        /// <summary>
        /// 映射医院项目id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 卷类型id
        /// </summary>
        public string CouponCategoryID { get; set; }
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
