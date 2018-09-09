using System;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 治疗预约更新
    /// </summary>
    public class TreatUpdate
    {
        /// <summary>
        /// 预约记录ID
        /// </summary>
        public long ID { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public long CustomerID { get; set; }
        /// <summary>
        /// 操作人ID
        /// </summary>
        public long CreateUserID { get; set; }
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
        /// 预约医生
        /// </summary>
        public long UserID { get; set; }
        /// <summary>
        /// 预约项目
        /// </summary>
        public long ChargeID { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
