using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 科室领用更新dto
    /// </summary>
   public class UseUpdate
    {
        /// <summary>
        /// 科室领用记录id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 仓库id
        /// </summary>
        public string WarehouseID { get; set; }
        /// <summary>
        /// 操作人id
        /// </summary>
        public string CreateUserID { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 领用日期
        /// </summary>
        public string CreateDate { get; set; }
        /// <summary>
        /// 领用单号
        /// </summary>
        public string No { get; set; }
        /// <summary>
        /// 部门id
        /// </summary>
        public string DeptID { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 领用人
        /// </summary>
        public string UseUserID { get; set; }
        /// <summary>
        /// 备注s
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 领用信息详细DTO
        /// </summary>
        public virtual List<UseDetailAdd> UseDetailAdd { get; set; }
    }
}
