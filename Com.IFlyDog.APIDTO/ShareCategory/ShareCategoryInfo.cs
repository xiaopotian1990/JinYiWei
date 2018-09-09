using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 分享家展示dto类
    /// </summary>
  public  class ShareCategoryInfo
    {
        /// <summary>
        /// id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 分享家名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 级别
        /// </summary>
        public int Level { get; set; }
        /// <summary>
        /// 分享数量
        /// </summary>
        public int Number { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }


        /// <summary>
        /// 最大级别
        /// </summary>
        public string MaxLevle { get; set; }
        /// <summary>
        /// 最大分享数量
        /// </summary>
        public string MaxNumber { get; set; }
    }
}
