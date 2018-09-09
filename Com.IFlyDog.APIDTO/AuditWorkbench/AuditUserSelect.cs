using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 审核规则点击审核查询dto
    /// </summary>
   public class AuditUserSelect
    {
        /// <summary>
        /// 哪个类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 数据id
        /// </summary>
        public long OrderID { get; set; }

        /// <summary>
        /// 哪个用户
        /// </summary>
        public long UserID { get; set; }
        /// <summary>
        /// 哪个医院
        /// </summary>
        public long HospitalID { get; set; }
    }
}
