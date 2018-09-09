using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 接诊工作台
    /// </summary>
    public class ReceptionTodayInfo
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public ReceptionTodayInfo()
        {
            Receptions = new List<ReceptionToday>();
        }
        /// <summary>
        /// 到诊
        /// </summary>
        public int AllPeople { get; set; }
        /// <summary>
        /// 新客
        /// </summary>
        public int New { get; set; }
        /// <summary>
        /// 初诊
        /// </summary>
        public int First { get; set; }
        /// <summary>
        /// 复诊
        /// </summary>
        public int Twice { get; set; }
        /// <summary>
        /// 老客
        /// </summary>
        public int Old { get; set; }
        /// <summary>
        /// 复查
        /// </summary>
        public int Check { get; set; }
        /// <summary>
        /// 再消费
        /// </summary>
        public int Again { get; set; }
        /// <summary>
        /// 未成交
        /// </summary>
        public int NotDeal { get; set; }
        /// <summary>
        /// 成交
        /// </summary>
        public int Deal { get; set; }
        /// <summary>
        /// 成交总金额
        /// </summary>
        public decimal Amount { get; set; }
        /// <summary>
        /// 上门人员
        /// </summary>
        public IEnumerable<ReceptionToday> Receptions { get; set; }
    }
    /// <summary>
    /// 接诊工作台
    /// </summary>
    public class ReceptionToday
    {
        /// <summary>
        /// 
        /// </summary>
        public ReceptionToday()
        {
            ClubIconList = new List<string>();
        }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 顾客名称
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 咨询人员
        /// </summary>
        public string ManagerName { get; set; }
        /// <summary>
        /// 接诊人员
        /// </summary>
        public string AssignUserName { get; set; }
        /// <summary>
        /// 会员类别
        /// </summary>
        public VisitType VisitType { get; set; }
        /// <summary>
        /// 新老客
        /// </summary>
        public CustomerType CustomerType { get; set; }
        /// <summary>
        /// 上门时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 主咨询项目
        /// </summary>
        public string SymptomName { get; set; }
        /// <summary>
        /// 成交状态
        /// </summary>
        public DealType DealType { get; set; }
        /// <summary>
        /// 会员图标
        /// </summary>
        public string MemberIcon { get; set; }
        /// <summary>
        /// 分享家图标
        /// </summary>
        public string ShareIcon { get; set; }
        /// <summary>
        /// 成交项目
        /// </summary>
        public string ChargeName { get; set; }
        /// <summary>
        /// 成交金额
        /// </summary>
        public decimal FinalPrice { get; set; }
        /// <summary>
        /// 单项目图标
        /// </summary>
        public IList<string> ClubIconList { get; set; }
    }

    /// <summary>
    /// 单项目
    /// </summary>
    public class ClubTemp
    {
        /// <summary>
        /// 项目ID
        /// </summary>
        public string ChargeID { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Icon { get; set; }
    }

    public class ReceptionChargeTemp
    {
        public string CustomerID { get; set; }
        public string ChargeID { get; set; }
        public string ChargeName { get; set; }
        public decimal FinalPrice { get; set; }
        public DateTime PaidTime { get; set; }
    }

    public class ReceptionBack
    {
        public string CustomerID { get; set; }
        public decimal Amount { get; set; }
        public int Type { get; set; }
    }
}
