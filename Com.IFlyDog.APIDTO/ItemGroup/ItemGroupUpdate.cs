using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 修改报表项目组
    /// </summary>
   public class ItemGroupUpdate
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
        /// 排序号
        /// </summary>
        public string SortNo { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
    }
}
