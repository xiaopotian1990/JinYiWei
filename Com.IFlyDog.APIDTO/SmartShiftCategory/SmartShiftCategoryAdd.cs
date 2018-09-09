using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 添加班次管理用dto
    /// </summary>
  public  class SmartShiftCategoryAdd
    {
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
        public string ShiftCategoryType { get; set; }
        /// <summary>
        /// 班次状态
        /// </summary>
        public string ShiftState { get; set; }
    }
}
