using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 结算时查询出的收银信息
    /// </summary>
    public class CashierOfUserInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public CashierOfUserInfo()
        {
            CashierOfUserList = new List<CashierOfUser>();
            CardCategoryList = new List<string>();
        }
        /// <summary>
        /// 结算开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结算终止时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 现金
        /// </summary>
        public decimal Cash { get; set; }
        /// <summary>
        /// 卡
        /// </summary>
        public decimal Card { get; set; }
        /// <summary>
        /// 记录数
        /// </summary>
        public int Count { get; set; }
        /// <summary>
        /// 结算时查询出的收银列表
        /// </summary>
        public virtual IEnumerable<CashierOfUser> CashierOfUserList { get; set; }
        /// <summary>
        /// 卡记录
        /// </summary>
        public virtual IEnumerable<string> CardCategoryList { get; set; }
    }

    /// <summary>
    /// 结算时查询出的收银列表
    /// </summary>
    public class CashierOfUser
    {
        /// <summary>
        /// 收银记录ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 客户
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 收银时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 收银用户
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 现金
        /// </summary>
        public decimal Cash { get; set; }
        /// <summary>
        /// 刷卡
        /// </summary>
        public decimal Card { get; set; }
    }
}
