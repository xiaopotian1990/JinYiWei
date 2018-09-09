using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 标签条件查询
    /// </summary>
   public class TagConditionSelect
    {
        /// <summary>
        /// 
        /// </summary>
        public TagConditionSelect()
        {
            TagInfoAdd = new List<TagInfo>();
        }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 医院ID，下拉菜单
        /// </summary>
        public long HospitalID { get; set; }
        /// <summary>
        ///标签信息dto
        /// </summary>
        public virtual List<TagInfo> TagInfoAdd { get; set; }
    }

    /// <summary>
    /// 标签信息
    /// </summary>
    public class TagInfo {
        /// <summary>
        /// 标签id
        /// </summary>
        public long TagID { get; set; }
    }
}
