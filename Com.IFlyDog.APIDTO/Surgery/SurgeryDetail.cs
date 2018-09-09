using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 手术详细
    /// </summary>
    public class SurgeryDetail
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public SurgeryDetail()
        {
            Charges = new List<ChargeTemp>();
        }
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
        /// 麻醉类型
        /// </summary>
        public AnesthesiaType AnesthesiaType { get; set; }
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
        public virtual List<ChargeTemp> Charges { get; set; }
    }
}
