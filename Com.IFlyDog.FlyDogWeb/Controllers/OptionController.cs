using Com.IFlyDog.APIDTO;
using Com.IFlyDog.FlyDogWeb.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Com.IFlyDog.FlyDogWeb.Controllers
{
    /// <summary>
    ///系统设置控制器
    /// </summary>
    public class OptionController : Controller
    {
        /// <summary>
        /// 系统设置action
        /// </summary>
        /// <returns></returns>
        // GET: Option
        public ActionResult OptionInfo()
        {
            return View();
        }

        #region 查询所有系统设置
        /// <summary>
        /// 查询所有系统设置
        /// </summary>
        /// <returns></returns>
        public async Task<string> OptionGet()
        {
            var result = await WebAPIHelper.Get("/api/Option/Get", new Dictionary<string, string>());
            return result;
        }
        #endregion

        #region 修改预收款成交设置
        /// <summary>
        /// 修改预收款成交设置
        /// </summary>
        /// <param name="Option"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> UpdateAdvanceSettingsFun(OptionUpdateAdvanceSettings dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Option/UpdateAdvanceSettings", dto);
            return result;
        }
        #endregion

        #region 修改是否允许欠款
        /// <summary>
        /// 修改是否允许欠款
        /// </summary>
        /// <param name="Option"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> UpdateAllowArrearsFun(OptionUpdateAllowArrears dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Option/UpdateAllowArrears", dto);
            return result;
        }
        #endregion

        #region 修改咨询模板
        /// <summary>
        /// 修改咨询模板
        /// </summary>
        /// <param name="Option"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> UpdateContentTemplateFun(OptionUpdateContentTemplate dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Option/UpdateContentTemplate", dto);
            return result;
        }
        #endregion

        #region 修改客户自定义字段
        /// <summary>
        /// 修改客户自定义字段
        /// </summary>
        /// <param name="Option"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> UpdateCustomerFun(OptionUpdateCustomer dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Option/UpdateCustomer", dto);
            return result;
        }
        #endregion

        #region 修改积分比例
        /// <summary>
        /// 修改积分比例
        /// </summary>
        /// <param name="Option"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> UpdateIntegralNumFun(OptionUpdateIntegralNum dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Option/UpdateIntegralNum", dto);
            return result;
        }
        #endregion

        #region 修改预约设置
        /// <summary>
        /// 修改预约设置
        /// </summary>
        /// <param name="Option"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> UpdateMakeTimeFun(OptionUpdateMakeTime dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Option/UpdateMakeTime", dto);
            return result;
        }
        #endregion

        #region 修改隐私保护
        /// <summary>
        /// 修改隐私保护
        /// </summary>
        /// <param name="Option"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> UpdatePrivacyProtectionFun(OptionUpdatePrivacyProtection dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Option/UpdatePrivacyProtection", dto);
            return result;
        }
        #endregion

        #region 修改挂号
        /// <summary>
        /// 修改挂号
        /// </summary>
        /// <param name="Option"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> UpdateRegistrationFun(OptionUpdateRegistration dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Option/UpdateRegistration", dto);
            return result;
        }
        #endregion

        #region 修改等候是否开启
        /// <summary>
        /// 修改等候是否开启
        /// </summary>
        /// <param name="Option"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> UpdateWaitingDiagnosisFun(OptionUpdateWaitingDiagnosis dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Option/UpdateWaitingDiagnosis", dto);
            return result;
        }
        #endregion
    }
}