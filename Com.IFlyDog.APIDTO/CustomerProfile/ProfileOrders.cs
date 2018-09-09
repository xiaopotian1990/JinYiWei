using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 订单情况
    /// </summary>
    public class ProfileOrders
    {
        /// <summary>
        /// 订单
        /// </summary>
        public IEnumerable<Order> Orders { get; set; }
        /// <summary>
        /// 住院单
        /// </summary>
        public IEnumerable<Order> InpatientOrders { get; set; }
        /// <summary>
        /// 预收款单
        /// </summary>
        public IEnumerable<DepositOrder> DepositOrders { get; set; }
        /// <summary>
        /// 退项目单
        /// </summary>
        public IEnumerable<BackOrder> BackOrders { get; set; }
        /// <summary>
        /// 退预收款单
        /// </summary>
        public IEnumerable<DepositRebateOrder> DepositRebateOrders { get; set; }
    }
}
