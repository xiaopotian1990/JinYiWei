using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 班次管理查询数据DTO类
    /// </summary>
  public  class SmartShiftCategoryInfo
    {
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 班次管理id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 班次管理名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 班次状态 1启用 0停用
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 班次类型
        /// </summary>
        public int Type { get; set; }
    }
}
