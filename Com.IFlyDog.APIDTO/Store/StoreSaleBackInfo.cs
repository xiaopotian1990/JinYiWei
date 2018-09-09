using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 销售回款记录表
    /// </summary>
   public class StoreSaleBackInfo
    {
        /// <summary>
        /// 提交医院名称
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public string CreateTime { get; set; }

        /// <summary>
        /// 提交用户
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 回款日期
        /// </summary>
        public string CreateDate { get; set; }
        /// <summary>
        /// 回款金额
        /// </summary>
        public string Amount { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
