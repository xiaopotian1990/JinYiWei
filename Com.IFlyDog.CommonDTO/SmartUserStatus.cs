using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{
    public enum SmartUserStatus
    {
        [Description("停用")]
        OFF = 0,
        [Description("正常")]
        On = 1
    }
}
