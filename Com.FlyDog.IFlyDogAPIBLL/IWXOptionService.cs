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
    /// 微信系统设置接口
    /// </summary>
  public  interface IWXOptionService
    {
        /// <summary>
        /// 微信系统设置修改不提点折扣小于
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> WXOptionUpdateNoDiscount(WXOptionUpdateNoDiscount dto);

        /// <summary>
        /// 微信系统设置佣金提成
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> WXOptionOpenCommission(WXOptionOpenCommission dto);

        /// <summary>
        /// 微信系统设置级别提点
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> WXOptionInsertPromoteLevel(WXOptionUpdatePromoteLevel dto);

        /// <summary>
        /// 微信系统设置修改推荐时限
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> WXOptionUpdateRecommenDay(WXOptionUpdateRecommenDay dto);

        /// <summary>
        /// 更新被推荐用户送券
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> WXOptionUpdateUserSendVolume(WXOptionUpdateUserSendVolume dto);

        /// <summary>
        /// 查询所有系统设置
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, WXOptionInfo> Get();

        /// <summary>
        /// 微信系统设置默认渠道
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> WXOptionDefaultChannel(WXOptionDefaultChannel dto);


        /// <summary>
        /// 微信系统设置特殊渠道
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> WXOptionSpecialChannel(WXOptionSpecialChannel dto);
    }
}
