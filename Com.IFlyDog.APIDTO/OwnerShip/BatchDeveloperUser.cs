using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 批量设置开发人员
    /// </summary>
   public class BatchDeveloperUser
    {
        /// <summary>
        /// 新的开发人员id
        /// </summary>
        public long UserID { get; set; }

        public long HospitalID { get; set; }

        public long CreateUserID { get; set; }

        /// <summary>
        /// 要设置的客户数据列表
        /// </summary>
        public virtual List<BatchCustorm> BatchCustormAdd { get; set; }
    }

    /// <summary>
    /// 客户id
    /// </summary>
    public class BatchCustorm {
        /// <summary>
        /// 要设置的客户id
        /// </summary>
        public long CustormID { get; set; }
    }

}
