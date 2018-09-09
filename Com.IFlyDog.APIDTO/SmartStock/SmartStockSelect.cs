using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 库存查询查询dto类
    /// </summary>
  public  class SmartStockSelect
    {
        /// <summary>
        /// 所属医院id，每次查询库存信息都必填的，从后台取数据，目前数据库中还没有这个字段，
        /// 所以目前先不用，等确定下来加上
        /// </summary>
        public long HospitalID { get; set; }
        /// <summary>
        /// /仓库id
        /// </summary>
        public string WarehouseID { get; set; }
        /// <summary>
        /// 药物品名称
        /// </summary>
        public string WPName { get; set; }
        /// <summary>
        /// 拼音码
        /// </summary>
        public string PinYin { get; set; }
        /// <summary>
        /// 分类id
        /// </summary>
        public string CategoryID { get; set; }
        /// <summary>
        /// 登录用户id
        /// </summary>
        public string CreateUserId { get; set; }
        /// <summary>
        /// 类型1：入库2：调拨3：盘点4：科室领用
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 开始时间
        /// </summary>
        public string BeginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public string EndTime { get; set; }

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
