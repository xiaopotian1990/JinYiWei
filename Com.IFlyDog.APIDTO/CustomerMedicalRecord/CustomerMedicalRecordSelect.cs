using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 查询顾客病例列表
    /// </summary>
   public class CustomerMedicalRecordSelect
    {
        /// <summary>
        /// 操作用户id
        /// </summary>
        public long CreateUserID { get; set; }

        /// <summary>
        /// 客户id
        /// </summary>
        public long CustomerID { get; set; }
    }
}
