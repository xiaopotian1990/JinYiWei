using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 发货记录
    /// </summary>
    public class DeptSend
    {
        /// <summary>
        /// 发货请求ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 顾客姓名
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 发货时间
        /// </summary>
        public DateTime SendTime { get; set; }
        /// <summary>
        /// 发货仓库
        /// </summary>
        public string Warehouse { get; set; }
        /// <summary>
        /// 耗材名称
        /// </summary>
        public string Product { get; set; }
        /// <summary>
        /// 规格
        /// </summary>
        public string Size { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Num { get; set; }

    }

    /// <summary>
    /// 发货
    /// </summary>
    public class DeptSendAdd
    {
        /// <summary>
        /// 发货请求ID
        /// </summary>
        public long ID{get;set;}
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
    }

    /// <summary>
    /// 临时表
    /// </summary>
    public class WarehouseTemp
    {
        /// <summary>
        /// ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 剩余数量
        /// </summary>
        public int Num { get; set; }
    }
}
