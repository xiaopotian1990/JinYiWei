using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 预收款添加
    /// </summary>
    public class DepositOrderAdd
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public DepositOrderAdd()
        {
            Details = new List<DepositOrderDetailAdd>();
        }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public long CustomerID { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 操作人所在医院
        /// </summary>
        public long HospitalID { get; set; }
        /// <summary>
        /// 详细
        /// </summary>
        public virtual IEnumerable<DepositOrderDetailAdd> Details { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    /// <summary>
    /// 预收款详细
    /// </summary>
    public class DepositOrderDetailAdd
    {
        /// <summary>
        /// 预收款项目ID
        /// </summary>
        public long ChargeID { get; set; }
        /// <summary>
        /// 预收款数量
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 价格，前台不用传过来的
        /// </summary>
        public decimal Price { get; set; }
    }


    /// <summary>
    /// 预收款详细
    /// </summary>
    public class DepositOrderDetailTemp
    {
        /// <summary>
        /// 预收款项目ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 预收款订单
        /// </summary>
        public long OrderID { get; set; }
        /// <summary>
        /// 预收款类型ID
        /// </summary>
        public long ChargeID { get; set; }
        /// <summary>
        /// 预收款数量
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 最终价格
        /// </summary>
        public decimal Total { get; set; }
    }
}
