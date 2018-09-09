using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System.Collections.Generic;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    /// <summary>
    /// 渠道服务
    /// </summary>
    public interface IChannelService
    {
        /// <summary>
        /// 添加渠道
        /// </summary>
        /// <param name="dto">渠道信息</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(ChannelAdd dto);

        /// <summary>
        /// 渠道修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(ChannelUpdate dto);

        /// <summary>
        /// 渠道使用停用
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> StopOrUse(ChannelStopOrUse dto);

        /// <summary>
        /// 查询所有渠道
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<Channel>> Get();

        /// <summary>
        /// 查询所有可使用的渠道
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<Channel>> GetByIsOk();

        /// <summary>
        /// 查询所有渠道
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, Channel> GetByID(long id);

        /// <summary>
        /// 查询所有渠道
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect();
    }
}
