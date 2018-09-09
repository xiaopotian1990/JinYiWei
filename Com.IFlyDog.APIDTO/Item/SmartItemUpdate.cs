using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 报表项目更新dto
    /// </summary>
   public class SmartItemUpdate
    {
        /// <summary>
        /// 报表项目id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 分组id
        /// </summary>
        public string GroupID { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public string SortNo { get; set; }
        /// <summary>
        /// 排序号
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }

        /// <summary>
        /// 报表项目症状映射详细dto
        /// </summary>
        public virtual List<SymptomDetailInfo> SymptomDetailInfoAdd { get; set; }

        /// <summary>
        /// 报表项目收费项目映射详细dto
        /// </summary>
        public virtual List<ChargeDetailInfo> ChargeDetailInfoAdd { get; set; }
    }
}
