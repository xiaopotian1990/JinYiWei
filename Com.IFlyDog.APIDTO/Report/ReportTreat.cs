using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO.Report
{
    /// <summary>
    /// 治疗预约明细
    /// </summary>
    public class ReportTreat
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
        /// 医院ID
        /// </summary>
        public string HospitalID { get; set; }
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
        public string UserID { get; set; }
        /// <summary>
        /// 预约医生
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 预约项目
        /// </summary>
        public string ChargeID { get; set; }
        /// <summary>
        /// 预约项目
        /// </summary>
        public string ChargeName { get; set; }
        /// <summary>
        /// 医院ID，下拉菜单传过来的
        /// </summary>
        public string HospitalName { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }

    /// <summary>
    /// 治疗预约明细Select
    /// </summary>
    public class ReportTreatSelect
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 预约医院ID
        /// </summary>
        public long? HospitalID { get; set; }
        /// <summary>
        /// 预约医生
        /// </summary>
        public long? UserID { get; set; }
        /// <summary>
        /// 预约状态0：未上门，1：已完成预约,99：所有
        /// </summary>
        public AppointmentStatus Status { get; set; }
        /// <summary>
        /// 顾客编号
        /// </summary>
        public long? CustomerID { get; set; }
        /// <summary>
        /// 当前分页
        /// </summary>
        public int PageNum { get; set; }

        /// <summary>
        /// 分页大小
        /// </summary>
        public int PageSize { get; set; }
    }
}
