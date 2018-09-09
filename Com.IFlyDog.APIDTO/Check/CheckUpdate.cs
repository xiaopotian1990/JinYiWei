using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 库存盘点修改dto
    /// </summary>
   public class CheckUpdate
    {
        /// <summary>
        /// 盘点id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 操作人id
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 仓库id
        /// </summary>
        public string WarehouseID { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 状态 0进行中1：已完成
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 盘点单号
        /// </summary>
        public string No { get; set; }
        /// <summary>
        /// 盘点日期
        /// </summary>
        public string CreateDate { get; set; }

        /// <summary>
        /// 盘点详情
        /// </summary>
        public virtual List<CheckDetailAdd> CheckDetailUpdate { get; set; }
    }
}
