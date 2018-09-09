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
   public class UseInfo
    {
        /// <summary>
        /// 科室领用id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 领用单号
        /// </summary>
        public string No { get; set; }
        /// <summary>
        /// 领用日期
        /// </summary>
        public string CreateDate { get; set; }
        /// <summary>
        /// 领用部门id
        /// </summary>
        public string DeptID { get; set; }
        /// <summary>
        /// 领用部门名称
        /// </summary>
        public string DeptName { get; set; }
        /// <summary>
        /// 领用用户id
        /// </summary>
        public string UseUserID { get; set; }
        /// <summary>
        /// 领用用户名称
        /// </summary>
        public string UseName { get; set; }
        /// <summary>
        /// 仓库id
        /// </summary>
        public string WarehouseID { get; set; }
        /// <summary>
        /// 仓库名称
        /// </summary>
        public string WarehouseName { get; set; }
        /// <summary>
        /// 操作用户id
        /// </summary>
        public string CreateUserID { get; set; }
        /// <summary>
        /// 操作用户名称
        /// </summary>
        public string CreateName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 领用详情dto
        /// </summary>
        public virtual List<UseDetailAdd> UseDetail { get; set; }
    }
}
