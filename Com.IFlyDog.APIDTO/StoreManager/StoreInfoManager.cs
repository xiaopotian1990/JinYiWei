using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 店家负责人展示店家列表dto
    /// </summary>
    public class StoreInfoManager
    {
        /// <summary>
        /// 店铺id
        /// </summary>
        public string StoreID { get; set; }
        /// <summary>
        /// 店铺名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 店铺联系人
        /// </summary>
        public string Linkman { get; set; }
        /// <summary>
        /// 店铺地址
        /// </summary>
        public string Address { get; set; }
    }
}
