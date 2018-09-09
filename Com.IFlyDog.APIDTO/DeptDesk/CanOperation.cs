using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 可划扣项目
    /// </summary>
    public class CanOperation
    {
        /// <summary>
        /// 详细ID
        /// </summary>
        public string DetailID { get; set; }
        /// <summary>
        /// 项目ID
        /// </summary>
        public string ChargeID { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ChargeName { get; set; }
        /// <summary>
        /// 购买医院
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 支付时间
        /// </summary>
        public DateTime PaidTime { get; set; }
        /// <summary>
        /// 项目分类
        /// </summary>
        public string ChargeCategoryName { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        public string UnitName { get; set; }
        /// <summary>
        /// 购买数量
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 剩余数量
        /// </summary>
        public int RestNum { get; set; }
        /// <summary>
        /// 成交价格
        /// </summary>
        public decimal FinalPrice { get; set; }
    }
}
