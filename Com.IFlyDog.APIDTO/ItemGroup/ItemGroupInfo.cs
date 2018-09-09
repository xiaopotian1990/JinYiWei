using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 报表项目组列表展示dto
    /// </summary>
   public class ItemGroupInfo
    {
        /// <summary>
        /// 报表项目组id
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 报表项目名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 属于此报表项目组的报表项目名称（用逗号隔开的）
        /// </summary>
        public string ItemChargeName { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>
        public string SortNo { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
