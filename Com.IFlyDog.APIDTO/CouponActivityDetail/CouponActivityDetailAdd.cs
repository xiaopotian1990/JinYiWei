using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 卷活动详细
    /// </summary>
   public class CouponActivityDetailAdd
    {
        /// <summary>
        /// 卷详细id
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 卷活动id
        /// </summary>
        public string ActivityID { get; set; }
        /// <summary>
        /// 编码范围开始
        /// </summary>
        public int CodeBegin { get; set; }
        /// <summary>
        /// 编码范围结束
        /// </summary>
        public int CodeEnd { get; set; }
        /// <summary>
        /// 起始头
        /// </summary>
        public string Prefix { get; set; }

        /// <summary>
        /// 激活码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 卷类型id
        /// </summary>
        public string CouponID { get; set; }
        /// <summary>
        /// 客户id
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 医院id
        /// </summary>
        public long HospitalID { get; set; }

        /// <summary>
        /// 操作人id
        /// </summary>
        public long CreateUserID { get; set; }
    }

    public class CouponActivityDetailTemp {
        /// <summary>
        /// 卷详细id
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 卷活动id
        /// </summary>
        public string ActivityID { get; set; }

        /// <summary>
        /// 激活码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 卷类型id
        /// </summary>
        public string CouponID { get; set; }
    }

}
