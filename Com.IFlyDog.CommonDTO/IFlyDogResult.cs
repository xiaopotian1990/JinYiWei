using Com.JinYiWei.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.IFlyDog.CommonDTO
{
    /// <summary>
    /// 返回结果
    /// </summary>
    /// <typeparam name="TResultType">状态类型</typeparam>
    public class IFlyDogResult<TResultType> : CommonResult<TResultType>
    {
    }
    /// <summary>
    /// 返回结果
    /// </summary>
    /// <typeparam name="TResultType">状态类型</typeparam>
    /// <typeparam name="TData">具体的返回信息</typeparam>
    public class IFlyDogResult<TResultType, TData> : CommonResult<TResultType, TData>
    {
    }
}
