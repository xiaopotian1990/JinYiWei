using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 返回类型 0：成功；1：失败；2：系统异常；3：无权限
    /// </summary>
    public enum IFlyDogResultType
    {
        /// <summary>
        /// 成功结果类型
        /// </summary>
        Success = 0,

        /// <summary>
        /// 消息结果类型
        /// </summary>
        Failed = 1,

        /// <summary>
        /// 异常结果类型
        /// </summary>
        Error = 2,

        /// <summary>
        /// 权限结果类型
        /// </summary>
        NoAuth = 3,
    }
}
