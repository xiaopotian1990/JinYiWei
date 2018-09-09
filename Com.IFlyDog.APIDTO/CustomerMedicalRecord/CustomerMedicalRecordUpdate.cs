using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 
    /// </summary>
   public class CustomerMedicalRecordUpdate
    {
        /// <summary>
        /// 顾客病例模板id
        /// </summary>
        public long ID { get; set; }

        /// <summary>
        /// 病例id
        /// </summary>
        public long MedicalRecordID { get; set; }

        /// <summary>
        /// 病历号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 存放位置
        /// </summary>
        public string Location { get; set; }
        /// <summary>
        /// 具体内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        public long CreateUserID { get; set; }
    }
}
