using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 未成交记录
    /// </summary>
    public class Failture
    {
        /// <summary>
        /// 未成交记录ID，更新传过来
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 未成交原因，500字最多
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 登记时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 未成交类型
        /// </summary>
        public string CategoryName { get; set; }
    }
}
