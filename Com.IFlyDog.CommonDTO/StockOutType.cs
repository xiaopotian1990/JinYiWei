using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 出库类型
    /// </summary>
    public enum StockOutType
    {
        Operation = 1,
        Return = 2,
        Allocate = 3,
        Offset = 4,
        Check = 5,
        Use = 6
    }
}
