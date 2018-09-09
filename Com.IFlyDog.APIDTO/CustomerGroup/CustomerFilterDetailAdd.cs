using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 添加客户结果集
    /// </summary>
   public class CustomerFilterDetailAdd
    {
        /// <summary>
        /// 结果集详情id
        /// </summary>
        public long ID { get; set; }
       
        /// <summary>
        /// 结果集id
        /// </summary>
        public long FilterID { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public long CreateUserID { get; set; }

        /// <summary>
        ///客户集合
        /// </summary>
        public virtual List<CustormInfo> CustormInfoAdd { get; set; }
    }

    /// <summary>
    /// 客户信息
    /// </summary>
    public class CustormInfo {
        /// <summary>
        /// 客户id
        /// </summary>
        public long CustomerID { get; set; }
    }
}
