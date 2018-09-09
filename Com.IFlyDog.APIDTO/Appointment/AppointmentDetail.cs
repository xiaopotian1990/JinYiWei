using System;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 预约详细
    /// </summary>
    public class AppointmentDetail
    {
        /// <summary>
        /// 预约记录ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 医院ID
        /// </summary>
        public string HospitalID { get; set; }
        /// <summary>
        /// 提交人
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 提交时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 预约日期，具体到天
        /// </summary>
        public DateTime AppointmentDate { get; set; }
        /// <summary>
        /// 时间段，几点几分
        /// </summary>
        public TimeSpan AppointmentStartTime { get; set; }
        /// <summary>
        /// 时间段，几点几分
        /// </summary>
        public TimeSpan AppointmentEndTime { get; set; }
        /// <summary>
        /// 预约医院
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        public string Content { get; set; }
    }
}
