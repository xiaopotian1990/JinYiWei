using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 正在住院中的顾客
    /// </summary>

    public class InpatientIn
    {
        /// <summary>
        /// 住院记录ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 顾客姓名
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 顾客头像
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// 床位ID
        /// </summary>
        public string BedID { get; set; }
        /// <summary>
        /// 床位
        /// </summary>
        public string BedName { get; set; }
        /// <summary>
        /// 住院时间
        /// </summary>
        public DateTime InTime { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 住院单ID，没有话为0
        /// </summary>
        public string OrderID { get; set; }
    }
}
