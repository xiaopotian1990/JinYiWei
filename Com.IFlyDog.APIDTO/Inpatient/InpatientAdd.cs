using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 住院
    /// </summary>
    public class InpatientAdd
    {
        /// <summary>
        /// 顾客ID
        /// </summary>
        public long CustomerID { get; set; }
        /// <summary>
        /// 操作人所在医院
        /// </summary>
        public long HospitalID { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 床位ID
        /// </summary>
        public long BedID { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    /// <summary>
    /// 出院
    /// </summary>
    public class Inpatientout
    {
        /// <summary>
        /// 住院ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 操作人所在医院
        /// </summary>
        public long HospitalID { get; set; }
        /// <summary>
        /// 床位ID
        /// </summary>
        public long BedID { get; set; }
    }
}
