using Com.IFlyDog.CommonDTO;
using System;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 今日上门记录
    /// </summary>
    public class VisitToday
    {
        /// <summary>
        /// 上门ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 顾客姓名
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 现场咨询
        /// </summary>
        public string ManagerUserName { get; set; }
        /// <summary>
        /// 上门时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 消费类型
        /// </summary>
        public VisitType VisitType { get; set; }
        /// <summary>
        /// 新老客
        /// </summary>
        public CustomerType CustomerType { get; set; }
        /// <summary>
        /// 接待用户
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 接诊人员/部门
        /// </summary>
        public string AssignUserName { get; set; }
    }
}
