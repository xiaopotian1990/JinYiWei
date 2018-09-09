using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 删除项目分类
    /// </summary>
   public class ChargeCategoryDelete
    {
        /// <summary>
        /// 操作人id
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 项目分类id
        /// </summary>
        public string ID { get; set; }
    }
}
