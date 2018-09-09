using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 订单条件查询
    /// </summary>
   public class OrderConditionSelect
    {
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
        /// <summary>
        /// 医院ID，下拉菜单
        /// </summary>
        public long HospitalID { get; set; }
        /// <summary>
        /// 支付开始时间
        /// </summary>
        public DateTime? PaymentBeginTime { get; set; }

        /// <summary>
        /// 支付结束时间
        /// </summary>
        public DateTime? PaymentEndTime { get; set; }

        /// <summary>
        /// 执行项目范围 1指定项目 2指定项目分类 3 指定报表分类
        /// </summary>
        public int? ScopeLimit { get; set; }

        /// <summary>
        /// 指定项目id
        /// </summary>
        public long ChargeID { get; set; }

        /// <summary>
        /// 指定项目分类
        /// </summary>
        public long ChargeCategoryID { get; set; }

        /// <summary>
        /// 指定报表分类
        /// </summary>
        public long ItemCategoryID { get; set; }

        /// <summary>
        /// 消费金额条件类型 >=、>、=、<=、<
        /// </summary>
        public string ConsumptionConditionType { get; set; }

        /// <summary>
        /// 消费金额
        /// </summary>
        public decimal? ConsumptionMoney { get; set; }


    }

    public class OrderSelectTemp
    {
        public long CustomerID { get; set; }
        public long ChargeID { get; set; }
        public decimal FinalPrice { get; set; }
    }
}
