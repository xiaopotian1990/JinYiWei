using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 手术更新
    /// </summary>
    public class SugeryDone
    {
        /// <summary>
        /// 手术记录ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 状态 1：开始2：结束
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Time { get; set; }
    }
}
