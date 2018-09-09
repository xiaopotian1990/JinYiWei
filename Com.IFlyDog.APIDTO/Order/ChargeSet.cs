using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 套餐
    /// </summary>
    public class ChargeSet
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ChargeSet()
        {
            Details = new List<ChargeSetDetail>();
        }
        /// <summary>
        /// 套餐ID
        /// </summary>
        public string SetID { get; set; }
        /// <summary>
        /// 套餐名称
        /// </summary>
        public string SetName { get; set; }
        /// <summary>
        /// 价格
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// 详细项目
        /// </summary>
        public virtual IList<ChargeSetDetail> Details{get;set;}
    }
    /// <summary>
    /// 详细项目
    /// </summary>
    public class ChargeSetDetail
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        public string ChargeID { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        public string ChargeName { get; set; }
        /// <summary>
        /// 数量
        /// </summary>
        public int Num { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public decimal Amount { get; set; }
    }
}
