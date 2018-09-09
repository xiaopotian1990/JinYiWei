using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 审核工作台默认页面列表展示数据
    /// </summary>
   public class AuditWorkbenchInfo
    {
        /// <summary>
        /// 列表id
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 客户id
        /// </summary>
        public string CustomerID { get; set; }

        /// <summary>
        /// 审核类型
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public string TypeValue { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 提交医院
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 提交用户
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 提交日期
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
