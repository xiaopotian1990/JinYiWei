using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 未完成项目
    /// </summary>
    public class NoDoneOrders
    {

        /// <summary>
        /// 项目ID
        /// </summary>
        public string ChargeID { get; set; }
        /// <summary>
        /// 项目ID
        /// </summary>
        public string ChargeName { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal FinalPrice { get; set; }
        /// <summary>
        /// 退款详细ID
        /// </summary>
        public string DetailID { get; set; }
        /// <summary>
        /// 剩余项目
        /// </summary>
        public int RestNum { get; set; }
    }
}
