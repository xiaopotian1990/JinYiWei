using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 预约
    /// </summary>
    public class ProfileAppoint
    {
        /// <summary>
        /// 咨询预约
        /// </summary>
        public IEnumerable<ProfileAppointment> AppointmentList { get; set; }
        /// <summary>
        /// 治疗预约
        /// </summary>
        public IEnumerable<ProfileTreat> TreatList { get; set; }
        /// <summary>
        /// 手术预约
        /// </summary>
        public IEnumerable<ProfileSurgery> SurgeryList { get; set; }
    }

    /// <summary>
    /// 预约详细
    /// </summary>
    public class ProfileAppointment
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
        /// <summary>
        /// 预约码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 预约状态
        /// </summary>
        public AppointmentStatus Status { get; set; }
    }

    /// <summary>
    /// 治疗预约详细
    /// </summary>
    public class ProfileTreat
    {
        /// <summary>
        /// 记录ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 提交人
        /// </summary>
        public string CreateUserName { get; set; }
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
        public string UserName { get; set; }
        /// <summary>
        /// 预约项目
        /// </summary>
        public string ChargeName { get; set; }
        /// <summary>
        /// 医院ID，下拉菜单传过来的
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 预约状态
        /// </summary>
        public AppointmentStatus Status { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }


    /// <summary>
    /// 手术详细
    /// </summary>
    public class ProfileSurgery
    {
        /// <summary>
        /// 记录ID
        /// </summary>
        public string ID { get; set; }
        /// <summary>
        /// 顾客ID
        /// </summary>
        public string CustomerID { get; set; }
        /// <summary>
        /// 提交人
        /// </summary>
        public string CreateUserName { get; set; }
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
        public string UserName { get; set; }

        /// <summary>
        /// 医院ID，下拉菜单传过来的
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 项目
        /// </summary>
        public string ChargeName { get; set; }
        /// <summary>
        ///  手术预约状态0：未完成；1：正在进行中；2：已完成
        /// </summary>
        public SurgeryStatus Status { get; set; }
        /// <summary>
        /// 手术开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 手术结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
    }
}
