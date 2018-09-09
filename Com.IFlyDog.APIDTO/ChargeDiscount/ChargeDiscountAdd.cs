using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 添加项目折扣
    /// </summary>
   public class ChargeDiscountAdd
    {
        /// <summary>
        /// id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 有效性限制0：对有所项目有效1：对一类项目有效2：对单个项目有效
        /// </summary>
        public string ScopeLimit { get; set; }
        /// <summary>
        /// 项目id
        /// </summary>
        public string ChargeID { get; set; }
        /// <summary>
        /// 项目分类id
        /// </summary>
        public string ChargeCategoryID { get; set; }
        /// <summary>
        /// 折扣
        /// </summary>
        public string Discount { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public string StartTime { get; set; }
        /// <summary>
        /// 结束时间s
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 状态0：停用1：使用
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 创建人ID
        /// </summary>
        public long CreateUserID { get; set; }

        /// <summary>
        /// 医院id
        /// </summary>
        public long HospitalID { get; set; }
    }
}
