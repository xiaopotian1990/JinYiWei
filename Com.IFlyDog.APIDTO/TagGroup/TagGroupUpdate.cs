using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 修改标签组dto
    /// </summary>
   public class TagGroupUpdate
    {
        /// <summary>
        /// 标签id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }

        /// <summary>
        /// 标签组映射详细dto
        /// </summary>
        public virtual List<TagGroupDetailAdd> TagGroupDetailAdd { get; set; }
    }
}
