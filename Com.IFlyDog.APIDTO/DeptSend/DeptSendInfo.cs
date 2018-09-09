using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 科室发货请求
    /// </summary>
    public class DeptSendInfo
    {
        /// <summary>
        /// 发货请求ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 顾客姓名
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 顾客
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 划扣项目
        /// </summary>
        public string ChargeName { get; set; }
        /// <summary>
        /// 划扣时间
        /// </summary>
        public DateTime CreateTime { get; set; }
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
}
