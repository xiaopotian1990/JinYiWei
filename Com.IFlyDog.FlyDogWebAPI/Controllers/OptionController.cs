using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.WebAPI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Com.IFlyDog.FlyDogWebAPI.Controllers
{
    /// <summary>
    /// 系统设置api
    /// </summary>
    public class OptionController : ApiController
    {
        private IOptionService _optionService;

        #region 依赖注入类
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="optionService"></param>
        public OptionController(IOptionService optionService)
        {
            _optionService = optionService;
        }
        #endregion

        /// <summary>
        /// 修改预收款成交设置
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> UpdateAdvanceSettings(OptionUpdateAdvanceSettings dto) {
            return _optionService.UpdateAdvanceSettings(dto);
        }


        /// <summary>
        /// 修改是否允许欠款
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> UpdateAllowArrears(OptionUpdateAllowArrears dto) {
            return _optionService.UpdateAllowArrears(dto);
        }

        /// <summary>
        /// 修改咨询模板
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> UpdateContentTemplate(OptionUpdateContentTemplate dto) {
            return _optionService.UpdateContentTemplate(dto);
        }

        /// <summary>
        /// 修改客户自定义字段
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> UpdateCustomer(OptionUpdateCustomer dto) {
            return _optionService.UpdateCustomer(dto);
        }

        /// <summary>
        /// 修改积分比例
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> UpdateIntegralNum(OptionUpdateIntegralNum dto) {
            return _optionService.UpdateIntegralNum(dto);
        }

        /// <summary>
        /// 修改预约设置
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> UpdateMakeTime(OptionUpdateMakeTime dto) {
            return _optionService.UpdateMakeTime(dto);
        }

        /// <summary>
        /// 修改隐私保护
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> UpdatePrivacyProtection(OptionUpdatePrivacyProtection dto) {
            return _optionService.UpdatePrivacyProtection(dto);
        }

        /// <summary>
        /// 修改挂号
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> UpdateRegistration(OptionUpdateRegistration dto) {
            return _optionService.UpdateRegistration(dto);
        }

        /// <summary>
        /// 修改等候是否开启
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> UpdateWaitingDiagnosis(OptionUpdateWaitingDiagnosis dto) {
            return _optionService.UpdateWaitingDiagnosis(dto);
        }

        #region 查询所有系统设置
        /// <summary>
        /// 查询所有系统设置[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, OptionInfo> Get()
        {
            return _optionService.Get();
        }
        #endregion
    }
}
