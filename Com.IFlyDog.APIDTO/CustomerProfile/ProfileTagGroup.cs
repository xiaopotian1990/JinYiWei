using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 查询标签组
    /// </summary>
    public class ProfileTagGroup
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ProfileTagGroup()
        {
            Tags = new List<ProfileTag>();
        }
        /// <summary>
        /// ID
        /// </summary>
        public string GroupID { get; set; }
        /// <summary>
        /// 标签组名称
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 标签列表
        /// </summary>
        public virtual List<ProfileTag> Tags { get; set; }
    }
    /// <summary>
    /// 标签
    /// </summary>
    public class ProfileTag
    {
        /// <summary>
        /// ID
        /// </summary>
        public string TagID { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public string TagName { get; set; }
    }
}
