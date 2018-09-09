using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 结算记录
    /// </summary>
    public class Settlement
    {
        /// <summary>
        /// 结算记录ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 结算时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public string CreateUserID { get; set; }
        /// <summary>
        /// 操作用户
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 结算时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 现金
        /// </summary>
        public decimal Cash { get; set; }
        /// <summary>
        /// 刷卡
        /// </summary>
        public decimal Card { get; set; }
        /// <summary>
        /// 总金额
        /// </summary>
        public decimal Amount { get; set; }
    }

    /// <summary>
    /// 结算记录选择
    /// </summary>
    public class SettlementSelect
    {
        /// <summary>
        /// 医院ID
        /// </summary>
        public long HospitalID { get; set; }
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结算时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 用户名
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 第几页
        /// </summary>
        public int PageNum { get; set; }
        /// <summary>
        /// 页大小
        /// </summary>
        public int PageSize { get; set; }
    }
}
