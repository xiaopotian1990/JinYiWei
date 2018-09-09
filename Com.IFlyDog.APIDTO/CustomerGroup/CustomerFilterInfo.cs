using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 结果集列表展示dto
    /// </summary>
   public class CustomerFilterInfo
    {
        /// <summary>
        /// 结果集id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 结果集名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>
        public string CreateUserID { get; set; }
        /// <summary>
        /// 医院id
        /// </summary>
        public string HospitalID { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
