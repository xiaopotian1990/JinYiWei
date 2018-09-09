using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 项目分类添加
    /// </summary>
   public class ChargeCategoryAdd
    {
        /// <summary>
        /// 操作人id
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 项目分类id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 项目分类名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 父分类id
        /// </summary>
        public string ParentID { get; set; }
        /// <summary>
        /// 序号
        /// </summary>
        public string SortNo { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

    }
}
