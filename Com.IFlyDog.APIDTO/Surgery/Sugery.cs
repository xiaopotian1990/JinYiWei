using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.APIDTO
{
    /// <summary>
    /// 手术排台
    /// </summary>
    public class Surgery
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Surgery()
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
        /// 顾客
        /// </summary>
        public string CustomerName { get; set; }
        /// <summary>
        /// 提交人
        /// </summary>
        public string CreateUserName { get; set; }
        /// <summary>
        /// 时间段，几点几分
        /// </summary>
        public TimeSpan AppointmentStartTime { get; set; }
        /// <summary>
        /// 时间段，几点几分
        /// </summary>
        public TimeSpan AppointmentEndTime { get; set; }
        /// <summary>
        /// 手术开始时间
        /// </summary>
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// 手术结束时间
        /// </summary>
        public DateTime? EndTime { get; set; }
        /// <summary>
        /// 预约医生
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 麻醉类型
        /// </summary>
        public AnesthesiaType AnesthesiaType { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public SurgeryStatus Status { get; set; }
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
