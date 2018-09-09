using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 收银可用预收款项目
    /// </summary>
    public class CashierDeposits
    {
        /// <summary>
        /// 预收款ID
        /// </summary>
        public long DepositID { get; set; }
        /// <summary>
        /// 预收款类型
        /// </summary>
        public string DepositChargeName { get; set; }
        /// <summary>
        /// 预收款总金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 预售款剩余金额
        /// </summary>
        public decimal Rest { get; set; }
    } 
}
