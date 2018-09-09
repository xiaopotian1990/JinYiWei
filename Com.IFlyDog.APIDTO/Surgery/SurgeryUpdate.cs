using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 手术更新
    /// </summary>
    public class SurgeryUpdate
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SurgeryUpdate()
        {
            ChargeIDS = new List<long>();
        }
        /// <summary>
        /// 记录ID
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
        /// 麻醉类型
        /// </summary>
        public AnesthesiaType AnesthesiaType { get; set; }
        /// <summary>
        /// 预约医生
        /// </summary>
        public long UserID { get; set; }
        /// <summary>
        /// 预约项目
        /// </summary>
        public IEnumerable<long> ChargeIDS { get; set; }
        /// <summary>
        /// 医院ID，下拉菜单传过来的
        /// </summary>
        public long HospitalID { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
