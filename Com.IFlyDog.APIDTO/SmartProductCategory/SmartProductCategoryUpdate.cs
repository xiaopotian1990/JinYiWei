using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 药物品类型编辑DTO
    /// </summary>
   public class SmartProductCategoryUpdate
    {
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }

        /// <summary>
        /// 药物品Id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 药物品名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int SortNo { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 父分类id
        /// </summary>
        public string PID { get; set; }
    }
}
