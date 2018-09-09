using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 
    /// </summary>
   public class CouponActivityDetailInfo
    {
        /// <summary>
        /// 卷活动id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 卷id
        /// </summary>
        public string ActivityID { get; set; }
        /// <summary>
        /// 代码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 状态 是否激活
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 用户卷id(根据他来判断状态)
        /// </summary>
        public string CouponID { get; set; }
        /// <summary>
        /// 激活时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 激活客户
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 激活医院
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 激活操作用户
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 激活编号
        /// </summary>
        public string SerialNumber { get; set; }
    }
}
