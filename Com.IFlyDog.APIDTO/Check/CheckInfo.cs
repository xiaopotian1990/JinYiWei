using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 库存盘点详情dto
    /// </summary>
   public class CheckInfo
    {
        /// <summary>
        /// 盘点id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 盘点单号
        /// </summary>
        public string No { get; set; }
        /// <summary>
        /// 盘点日期
        /// </summary>
        public string CreateDate { get; set; }
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
        public string CreateUserName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 盘点详情
        /// </summary>
        public virtual List<CheckDetailAdd> CheckDetailAdd { get; set; }
    }
}
