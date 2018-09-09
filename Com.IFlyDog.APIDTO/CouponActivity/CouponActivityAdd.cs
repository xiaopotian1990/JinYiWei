using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 添加卷活动
    /// </summary>
   public class CouponActivityAdd
    {
        /// <summary>
        /// 卷活动id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 卷活动名称
        /// </summary>
        public string ActivityName { get; set; }
        /// <summary>
        /// 卷类型id
        /// </summary>
        public string CategoryID { get; set; }
        /// <summary>
        /// 卷类型名称
        /// </summary>
        public string CouponCategoryName { get; set; }
        /// <summary>
        /// 卷金额
        /// </summary>
        public string Amount { get; set; }
        /// <summary>
        /// 售价
        /// </summary>
        public string Price { get; set; }
        /// <summary>
        /// 发放渠道
        /// </summary>
        public string Channel { get; set; }
        /// <summary>
        /// 生效日期
        /// </summary>
        public string CreateDate { get; set; }
        /// <summary>
        /// 失效日期
        /// </summary>
        public string Expiration { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 是否允许同一客户重复使用0：否1：是
        /// </summary>
        public string IsRepetition { get; set; }
        /// <summary>
        /// 代码前缀
        /// </summary>
        public string Prefix { get; set; }
        /// <summary>
        /// 医院
        /// </summary>
        public string HospitalID { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public long CreateUserID { get; set; }
    }
}
