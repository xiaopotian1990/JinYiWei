using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 店家佣金记录表
    /// </summary>
   public class StoreCommissionInfo
    {
        /// <summary>
        /// 操作医院
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 操作用户名称
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 顾客名称
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public string Amount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 累计佣金总数
        /// </summary>
        public string CommissionSum { get; set; }
    }
}
