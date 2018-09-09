using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 客户事件类型
    /// </summary>
    public enum CustomerEventType
    {
        SmartCreate = 1,
        SmartConsult = 2,
        SmartCallback = 3,
        SmartAppointmentConsult = 4,
        SmartAppointmentTreat = 5,
        SmartAppointmentSurgery = 6,
        SmartVisit = 7,
        SmartTriage = 8,
        SmartFailture = 9,
        SmartComplain = 10,
        SmartBlacklist = 11,
        SmartGreylist = 12,
        SmartMember = 13,
        SmartOrder = 14,
        SmartDepositOrder = 15,
        SmartBackOrder = 16,
        SmartRefundOrder = 17,
        SmartInpatient = 18,
        SmartCashier = 19,
        SmartOperation = 20
    }
}
