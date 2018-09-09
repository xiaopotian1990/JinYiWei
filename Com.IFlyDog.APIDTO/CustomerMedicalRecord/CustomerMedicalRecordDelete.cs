using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 删除病例
    /// </summary>
   public class CustomerMedicalRecordDelete
    {
        public long ID { get; set; }

        public long CreateUserID { get; set; }
    }
}
