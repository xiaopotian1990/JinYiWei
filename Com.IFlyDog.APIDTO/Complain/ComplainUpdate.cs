using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 更新处理
    /// </summary>
   public class ComplainUpdate
    {
        /// <summary>
        /// 投诉id
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime FinishTime { get; set; }
        /// <summary>
        /// 处理用户id
        /// </summary>
        public long FinishUserID { get; set; }
        /// <summary>
        /// 处理内容
        /// </summary>
        public string Solution { get; set; }
    }
}
