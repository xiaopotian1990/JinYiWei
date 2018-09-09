using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 批量设置咨询人员
    /// </summary>
  public  class BatchConsultantUser
    {
        /// <summary>
        /// 新的咨询人员id
        /// </summary>
        public long UserID { get; set; }

        /// <summary>
        /// 当前医院
        /// </summary>
        public long HospitalID { get; set; }

        public long CreateUserID { get; set; }

        /// <summary>
        /// 要设置的客户数据列表
        /// </summary>
        public virtual List<BatchCustorm> BatchCustormAdd { get; set; }
    }

    public class BatchConsultantUserTemp {
        public long ID { get; set; }

        public long CustomerID { get; set; }

        public long UserID { get; set; }

        public DateTime StartTime { get; set; }

        public string EndTime1 { get; set; }

        public int Type { get; set; }

        public long HospitalID { get; set; }

        public string Remark { get; set; }
    }

    public class BatchConsultantUserUpdateTemp
    {

        public long CustomerID { get; set; }

        public long UserID { get; set; }

            /// <summary>
            /// 要更新的时间
            /// </summary>
        public DateTime EndTime { get; set; }

        public int Type { get; set; }

        public long HospitalID { get; set; }

        //public DateTime StartTime { get; set; }


        //public DateTime EndDateTime { get; set; }
    }
}
