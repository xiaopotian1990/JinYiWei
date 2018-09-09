using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 店家客户列表
    /// </summary>
   public class StoreManagerInfo
    {
        /// <summary>
        /// 店家客户id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 客户名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        public string Gender { get; set; }
        /// <summary>
        /// 客户登记时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 咨询症状名称
        /// </summary>
        public string SymptomName { get; set; }
        /// <summary>
        /// 是否上门，如果为空则未上门，否则上门
        /// </summary>
        public string FirstConsultTime { get; set; }

        /// <summary>
        /// 成交状态，如果为空则是未成交，否则成交
        /// </summary>
        public string FirstDealTime { get; set; }
        /// <summary>
        /// 累计推荐客户总数
        /// </summary>
        public string CustomerSum { get; set; }
    }
}
