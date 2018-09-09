using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 添加店家负责人
    /// </summary>
   public class StoreManagerAdd
    {
        /// <summary>
        /// 负责人id
        /// </summary>
        public string UserID { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }

        /// <summary>
        /// 店家详细dto
        /// </summary>
        public virtual List<StoreManagerInfoData> StoreManagerInfoData { get; set; }
    }

    /// <summary>
    /// 添加添加负责人中店家dto
    /// </summary>
    public class StoreManagerInfoData {
        /// <summary>
        /// 选择的店铺id
        /// </summary>
        public string StoreID { get; set; }
    }
}
