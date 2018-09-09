using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 删除店家
    /// </summary>
    public class StoreDelete
    {
        /// <summary>
        /// 店家id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 操作人所在医院ID
        /// </summary>
        public long HospitalID { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string CreateUserID { get; set; }
    }
}
