using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 订单
    /// </summary>
    public class OrderAdd
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public OrderAdd()
        {
            Details = new List<OrderDetailAdd>();
        }
        /// <summary>
        /// 订单ID，更新传
        /// </summary>
        public long OrderID { get; set; }
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
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 住院ID,普通订单传null，住院单传住院ID
        /// </summary>
        public long? InpatientID { get; set; }
        /// <summary>
        /// 详细
        /// </summary>

        public virtual IEnumerable<OrderDetailAdd> Details { get; set; }
    }
    /// <summary>
    /// 订单详细
    /// </summary>
    public class OrderDetailAdd
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        public long ChargeID { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 总价格
        /// </summary>
        public decimal FinalPrice { get; set; }
        /// <summary>
        /// 套餐ID
        /// </summary>
        public long SetID { get; set; }
    }

    /// <summary>
    /// 预收款详细
    /// </summary>
    public class OrderDetailAddTemp
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
        public long? ChargeID { get; set; }
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
        public decimal FinalPrice { get; set; }
        /// <summary>
        /// 剩余数量
        /// </summary>
        public int RestNum { get; set; }
        /// <summary>
        /// 套餐ID
        /// </summary>
        public long? SetID { get; set; }
        /// <summary>
        /// 套餐数量
        /// </summary>
        public int? SetNum { get; set; }
    }
}
