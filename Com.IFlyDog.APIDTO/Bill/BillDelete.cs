using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 发票删除
    /// </summary>
    public class BillDelete
    {
        /// <summary>
        /// 操作人
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 医院ID
        /// </summary>
        public long HospitalID { get; set; }
        /// <summary>
        /// 发票记录ID
        /// </summary>
        public long ID { get; set; }
    }
}
