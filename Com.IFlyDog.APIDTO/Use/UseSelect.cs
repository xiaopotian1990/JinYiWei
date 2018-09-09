using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 科室领用查询dto
    /// </summary>
  public  class UseSelect
    {

        /// <summary>
        /// 创建用户id，查询当前用户操作记录时使用
        /// </summary>
        public string CreateUserId { get; set; }
        /// <summary>
        /// 科室领用id
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 仓库id
        /// </summary>
        public string WarehouseID { get; set; }
        /// <summary>
        /// 部门id
        /// </summary>
        public string DeptID { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string BeginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }
        /// <summary>
        /// 领用单号
        /// </summary>
        public string No { get; set; }

        /// <summary>
        /// 当前分页
        /// </summary>
        public int PageNum { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }
    }
}
