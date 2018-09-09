using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 新增回款记录
    /// </summary>
   public class SaleBackAdd
    {
        /// <summary>
        /// 回款id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 医院id
        /// </summary>
        public string HospitalID { get; set; }
        /// <summary>
        /// 店铺id
        /// </summary>
        public string StoreID { get; set; }
        /// <summary>
        /// 回款日期
        /// </summary>
        public string CreateDate { get; set; }
        /// <summary>
        /// 回款金额
        /// </summary>
        public string Amount { get; set; }
        /// <summary>
        /// 提交用户
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 创建用户id
        /// </summary>
        public string CreateUserID { get; set; }

        /// <summary>
        /// 提交时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
