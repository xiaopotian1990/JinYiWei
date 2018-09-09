using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 预约添加
    /// </summary>
    public class AppointmentAdd
    {
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
        /// 预约内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 医院ID，下拉菜单传过来的
        /// </summary>
        public long HospitalID { get; set; }
    }


    /// <summary>
    /// 今日新增预约
    /// </summary>
    public class AppointmentToday
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 顾客
        /// </summary>
        public string CustomerName { get; set; }
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
        /// 预约内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 登记时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }
    }

    /// <summary>
    /// 今日新增预约
    /// </summary>
    public class AppointmentComeToday
    {
        /// <summary>
        /// ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 顾客
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 时间段，几点几分
        /// </summary>
        public TimeSpan AppointmentStartTime { get; set; }
        /// <summary>
        /// 时间段，几点几分
        /// </summary>
        public TimeSpan AppointmentEndTime { get; set; }
        /// <summary>
        /// 预约内容
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }
        /// <summary>
        /// 联系方式
        /// </summary>
        public string Mobile { get; set; }
    }
}
