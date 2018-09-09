using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 班次管理更新DTO类
    /// </summary>
   public class SmartShiftCategoryUpdate
    {
        /// <summary>
        /// 班级信息id
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }

        /// <summary>
        /// 班次管理名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 班次类型
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// 班次状态
        /// </summary>
        public int Status { get; set; }
    }
}
