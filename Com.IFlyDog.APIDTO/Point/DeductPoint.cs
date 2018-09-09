using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 扣减增加积分
    /// </summary>
    public class DeductPoint
    {
        /// <summary>
        /// 操作人所在医院ID
        /// </summary>
        public long HospitalID { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public long CustomerID { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public decimal Point { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 积分消费方式
        /// </summary>
        public PointType Type { get; set; }
    }

    /// <summary>
    /// 积分兑换券
    /// </summary>
    public class PointToCoupon
    {
        /// <summary>
        /// 操作人所在医院ID
        /// </summary>
        public long HospitalID { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public long CustomerID { get; set; }
        /// <summary>
        /// 积分
        /// </summary>
        public decimal PointAmount { get; set; }
        /// <summary>
        /// 兑换的券额
        /// </summary>
        public decimal CouponAmount { get; set; }
        /// <summary>
        /// 券类型
        /// </summary>
        public long CouponCategory { get; set; }
    }
}
