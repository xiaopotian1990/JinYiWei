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
    /// 用户通知接口
    /// </summary>
   public interface IUserTriggerService
    {
        /// <summary>
        /// 添加用户通知
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(UserTriggerAdd dto);

        /// <summary>
        /// 修改用户通知
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(UserTriggerUpdate dto);

        /// <summary>
        /// 查询所有用户通知
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<UserTriggerInfo>> Get();

        /// <summary>
        /// 根据ID获取用户通知
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, UserTriggerInfo> GetByID(long id);

        /// <summary>
        /// 删除用户通知
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Delete(UserTriggerDelete dto);
    }
}
