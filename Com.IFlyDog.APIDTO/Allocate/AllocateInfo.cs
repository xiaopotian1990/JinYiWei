using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 仓库调拨信息dto
    /// </summary>
   public class AllocateInfo
    {
        /// <summary>
        /// 调拨ids
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 状态0：（暂存）1：已调拨
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 调拨单号
        /// </summary>
        public string No { get; set; }
        /// <summary>
        /// 调拨日期
        /// </summary>
        public string CreateDate { get; set; }
        /// <summary>
        /// 调出仓库id
        /// </summary>
        public string FromWarehouseID { get; set; }
        /// <summary>
        /// 调出仓库名称
        /// </summary>
        public string FromWarehouseName { get; set; }
        /// <summary>
        /// 调入仓库id
        /// </summary>
        public string ToWarehouseID { get; set; }
        /// <summary>
        /// 调入仓库名称
        /// </summary>
        public string ToWarehouseName { get; set; }
        /// <summary>
        /// 申请人（领用人id）
        /// </summary>
        public string DoUserID { get; set; }
        /// <summary>
        /// 申请人（领用人名称）
        /// </summary>
        public string DoUserName { get; set; }
        /// <summary>
        /// 操作人id
        /// </summary>
        public string CreateUserID { get; set; }
        /// <summary>
        /// 操作人名称
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 调拨详情
        /// </summary>
        public virtual List<AllocateDetailAdd> AllocateDetail { get; set; }
    }
}
