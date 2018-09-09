using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 新增顾客病例模板
    /// </summary>
   public class CustomerMedicalRecordAdd
    {
        /// <summary>
        /// 顾客病例模板id
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 顾客id
        /// </summary>
        public long CustomerID { get; set; }
        /// <summary>
        /// 病例id
        /// </summary>
        public long MedicalRecordID { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 创建用户
        /// </summary>
        public long CreateUserID { get; set; }
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
    }
}
