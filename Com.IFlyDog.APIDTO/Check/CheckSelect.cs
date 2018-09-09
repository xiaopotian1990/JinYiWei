using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 库存盘点查询dto
    /// </summary>
   public class CheckSelect
    {

        /// <summary>
        /// 登陆用户id，查询当前用户数据时使用
        /// </summary>
        public string CreateUserId { get; set; }
        /// <summary>
        /// 库存盘点id
        /// </summary>
        public string ID { get; set; }

        /// <summary>
        /// 仓库id
        /// </summary>
        public string WarehouseID { get; set; }

        /// <summary>
        /// 盘点开始日期
        /// </summary>
        public string BeginDate { get; set; }
        /// <summary>
        /// 盘点结束日期
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
        /// 当前分页
        /// </summary>
        public int PageNum { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }
    }
}
