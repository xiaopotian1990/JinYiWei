using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.WebAPI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Com.IFlyDog.FlyDogWebAPI.Controllers
{
    /// <summary>
    /// 查询用户数据API
    /// </summary>
    public class SmartUserController : ApiController
    {
        /// <summary>
        /// 依赖注入
        /// </summary>
        private ISmartUserService _smartUserService;

        /// <summary>
        /// 依赖注入方法重构
        /// </summary>
        /// <param name="smartUserService"></param>
        public SmartUserController(ISmartUserService smartUserService)
        {
            _smartUserService = smartUserService;
        }

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <param name="dto">查询条件</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<SmartUserInfo>> Get([FromBody]SmartUserSelect dto)
        {
            return _smartUserService.Get(dto); ;
        }

        /// <summary>
        /// 查询所有（分页）
        /// </summary>
        /// <param name="dto">查询条件</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<SmartUserInfo>>> GetPages([FromBody]SmartUserSelect dto)
        {
            return _smartUserService.GetPages(dto);
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="dto">用户信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add([FromBody]UserAdd dto)
        {
            return _smartUserService.Add(dto);
        }

        /// <summary>
        /// 用户修改
        /// </summary>
        /// <param name="dto">用户信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update([FromBody]UserAdd dto)
        {
            return _smartUserService.Update(dto);
        }

        /// <summary>
        /// 用户使用停用
        /// </summary>
        /// <param name="dto">用户信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> StopOrUse([FromBody]UserStopOrUse dto)
        {
            return _smartUserService.StopOrUse(dto);
        }

        /// <summary>
        /// 密码重置
        /// </summary>
        /// <param name="dto">重置信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> PasswordReset([FromBody]UserPasswordReset dto)
        {
            return _smartUserService.PasswordReset(dto);
        }

        /// <summary>
        /// 设置顾客权限
        /// </summary>
        /// <param name="dto">访问权限</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> SetCustomerPermission(UserCustomerPermission dto)
        {
            return _smartUserService.SetCustomerPermission(dto);
        }

        /// <summary>
        /// 设置访问权限
        /// </summary>
        /// <param name="dto">访问权限</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> SetCustomerCallBackPermission(UserCustomerPermission dto)
        {
            return _smartUserService.SetCustomerCallBackPermission(dto);
        }

        /// <summary>
        /// 根据ID查询详细
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, SmartUserInfo> GetDetail(long id)
        {
            return _smartUserService.GetDetail(id);
        }

        /// <summary>
        /// 获取用户客户权限详细信息
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, UserCustomerPermissionDetail>> GetCustomerPermissionDetail(long userID)
        {
            return await _smartUserService.GetCustomerPermissionDetail(userID);
        }

        /// <summary>
        /// 获取用户回访权限详细信息
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, UserCustomerPermissionDetail>> GetCallBackPermissionDetail(long userID)
        {
            return await _smartUserService.GetCallBackPermissionDetail(userID);
        }

        /// <summary>
        /// 获取分疹人员列表
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<FZUser>> GetFZUsers(string hospitalID)
        {
            return _smartUserService.GetFZUsers(hospitalID);
        }


        /// <summary>
        /// 获取参与排班用户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<CYPBUser>> GetCYPBUsers(SmartUserSelect dto)
        {
            return _smartUserService.GetCYPBUsers(dto);
        }

        /// <summary>
        /// 获取参与预约用户
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public async Task<IFlyDogResult<IFlyDogResultType, IEnumerable<Select>>> GetSSYYUsers(long hospitalID, DateTime date)
        {
            return await _smartUserService.GetSSYYUsers(hospitalID, date);
        }
    }
}
