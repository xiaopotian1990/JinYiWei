using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 查询用户折扣dto
    /// </summary>
   public class UserDiscountInfo
    {
        /// <summary>
        /// id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 折扣
        /// </summary>
        public string Discount { get; set; }

        /// <summary>
        /// 状态 1：使用；0：停用
        /// </summary>
        public CommonStatus Status { get; set; }
    }
}
