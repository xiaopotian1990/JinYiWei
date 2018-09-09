using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 床位添加
    /// </summary>
    public class BedAdd
    {
        /// <summary>
        /// 床位ID，用于更新
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 床位名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 操作人所在医院ID
        /// </summary>
        public long HospitalID { get; set; }
    }
}
