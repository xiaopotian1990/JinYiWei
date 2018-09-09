using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 展示用户店家数量dto
    /// </summary>
   public class StoreUserManager
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        ///负责人名称
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 负责人拥有的店铺数量
        /// </summary>
        public string SumCount { get; set; }
    }
}
