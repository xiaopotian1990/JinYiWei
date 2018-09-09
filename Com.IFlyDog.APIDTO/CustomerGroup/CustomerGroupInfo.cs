using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 列表展示客户组
    /// </summary>
   public class CustomerGroupInfo
    {
        /// <summary>
        /// 客户组id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 名称
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
