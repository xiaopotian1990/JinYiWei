using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 仓库调拨更新dto类
    /// </summary>
   public class AllocateUpdate
    {
        /// <summary>
        /// 新增仓库调拨id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 操作人id
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 操作时间
        /// </summary>
        public string CreateTime { get; set; }
        /// <summary>
        /// 出仓库id
        /// </summary>
        public string FromWarehouseID { get; set; }
        /// <summary>
        /// 入仓库id
        /// </summary>
        public string ToWarehouseID { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 调拨单号
        /// </summary>
        public string No { get; set; }
        /// <summary>
        /// 调拨日期
        /// </summary>
        public string CreateDate { get; set; }
        /// <summary>
        /// 申请人，领用人
        /// </summary>
        public string DoUserID { get; set; }

        /// <summary>
        /// 调拨详情
        /// </summary>
        public virtual List<AllocateDetailAdd> AllocateDetailUpdate { get; set; }
    }
}
