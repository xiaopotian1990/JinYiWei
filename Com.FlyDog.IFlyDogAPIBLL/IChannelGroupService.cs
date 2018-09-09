using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    /// <summary>
    /// 渠道组接口
    /// </summary>
   public interface IChannelGroupService
    {
        /// <summary>
        /// 添加渠道组
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(ChannelGroupAdd dto);

        /// <summary>
        /// 修改渠道组
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(ChannelGroupUpdate dto);

        /// <summary>
        /// 查询所有渠道组
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<ChannelGroupInfo>> Get();

        /// <summary>
        /// 根据id获取渠道组信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, ChannelGroupInfo> GetByID(long id);

        /// <summary>
        /// 删除渠道组信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Delete(ChannelGroupDelete dto);

        /// <summary>
        /// 检测所有渠道
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<ChannelGroupCheck>> GetChannelGroupCheck();
    }
}
