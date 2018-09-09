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
    /// 系统设置接口
    /// </summary>
   public interface IOptionService
    {

        /// <summary>
        /// 修改客户自定义字段
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> UpdateCustomer(OptionUpdateCustomer dto);

        /// <summary>
        /// 修改咨询模板
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> UpdateContentTemplate(OptionUpdateContentTemplate dto);

        /// <summary>
        /// 修改预收款成交设置
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> UpdateAdvanceSettings(OptionUpdateAdvanceSettings dto);

        /// <summary>
        /// 修改预约设置
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> UpdateMakeTime(OptionUpdateMakeTime dto);

        /// <summary>
        /// 修改是否允许欠款
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> UpdateAllowArrears(OptionUpdateAllowArrears dto);

        /// <summary>
        /// 修改积分比例
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> UpdateIntegralNum(OptionUpdateIntegralNum dto);

        /// <summary>
        /// 修改隐私保护
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> UpdatePrivacyProtection(OptionUpdatePrivacyProtection dto);

        /// <summary>
        /// 修改挂号
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> UpdateRegistration(OptionUpdateRegistration dto);

        /// <summary>
        /// 修改等候是否开启
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> UpdateWaitingDiagnosis(OptionUpdateWaitingDiagnosis dto);

        /// <summary>
        /// 查询所有系统设置
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, OptionInfo> Get();

    }
}
