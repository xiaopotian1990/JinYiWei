using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 删除仓库dto
    /// </summary>
    public class SmartWarehouseDelete
    {
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }

        /// <summary>
        /// 仓库管理id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 医院id
        /// </summary>
        public string HospitalID { get; set; }
    }
}
