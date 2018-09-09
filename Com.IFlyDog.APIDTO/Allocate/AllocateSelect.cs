using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 仓库调拨查询dto
    /// </summary>
  public  class AllocateSelect
    {
        /// <summary>
        /// 登陆用户id，查询当前用户数据使用
        /// </summary>
        public string CreateUserId { get; set; }
        /// <summary>
        /// 仓库调拨id
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 调出仓库id
        /// </summary>
        public string FromWarehouseID { get; set; }
        /// <summary>
        /// 调入仓库id
        /// </summary>
        public string ToWarehouseID { get; set; }
        /// <summary>
        /// 调拨开始日期
        /// </summary>
        public string BeginDate { get; set; }
        /// <summary>
        /// 调拨结束日期
        /// </summary>
        public string EndDate { get; set; }
        /// <summary>
        /// 调拨单号
        /// </summary>
        public string No { get; set; }
        /// <summary>
        /// 拼音码
        /// </summary>
        public string PinYin { get; set; }
        /// <summary>
        /// 药品名称
        /// </summary>
        public string ProductName { get; set; }
        /// <summary>
        /// 分类id
        /// </summary>
        public string CategoryID { get; set; }
        /// <summary>
        /// 入库单号
        /// </summary>
        public string RKNo { get; set; }

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
