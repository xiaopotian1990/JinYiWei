using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 顾客佣金查询
    /// </summary>
    public class ProfileCustomerStoreCommission
    {
        /// <summary>
        /// 总佣金
        /// </summary>
        public decimal TotalAmount { get; set; }
        /// <summary>
        /// 详细记录
        /// </summary>
        public IEnumerable<ProfileCustomerStoreCommissionDetail> List { get; set; }
    }

    /// <summary>
    /// 记录
    /// </summary>
    public class ProfileCustomerStoreCommissionDetail
    {
        /// <summary>
        /// 佣金记录ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 店家ID
        /// </summary>
        public string StoreID { get; set; }
        /// <summary>
        /// 店家
        /// </summary>
        public string StoreName { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 提交人
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 医院
        /// </summary>
        public string HospitalName { get; set; }
    }
}
