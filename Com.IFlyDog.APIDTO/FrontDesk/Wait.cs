using System;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 候诊记录
    /// </summary>
    public class Wait
    {
        /// <summary>
        /// 候诊记录ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 顾客头像
        /// </summary>
        public string ImageUrl { get; set; }
        /// <summary>
        /// 顾客姓名
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 归属咨询人员
        /// </summary>
        public string ManagerUserName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 候诊登记时间
        /// </summary>
        public DateTime CreateTime { get; set; }
    }
}
