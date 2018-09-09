using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    public interface IEquityService
    {
        /// <summary>
        /// 添加会员权益
        /// </summary>
        /// <param name="dto">会员权益信息</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(EquityAdd dto);

        /// <summary>
        /// 更新会员权益信息
        /// </summary>
        /// <param name="dto">会员权益信息</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(EquityUpdate dto);

        /// <summary>
        /// 会员权益停用
        /// </summary>
        /// <param name="dto">参数集</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> StopOrUse(EquityStopOrUse dto);

        /// <summary>
        /// 查询所有会员权益
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<Equity>> Get();

        /// <summary>
        /// 只查询可用的会员权益
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<Equity>> GetStatusIsTrue();

        /// <summary>
        /// 查询会员权益详细
        /// </summary>
        /// <param name="id">会员权益ID</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, Equity> GetByID(long id);
    }
}
