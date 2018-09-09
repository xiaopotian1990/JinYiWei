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
    /// 用户通知
    /// </summary>
    public class UserTriggerController : ApiController
    {
        private IUserTriggerService _userTriggerService;

        #region 依赖注入类
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="userTriggerService"></param>
        public UserTriggerController(IUserTriggerService userTriggerService)
        {
            _userTriggerService = userTriggerService;
        }
        #endregion

        #region 添加用户通知
        /// <summary>
        /// 添加用户通知[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">添加用户通知</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add([FromBody]UserTriggerAdd dto)
        {
            return _userTriggerService.Add(dto);
        }
        #endregion

        #region 修改用户通知
        /// <summary>
        /// 修改用户通知[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">修改用户通知</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update([FromBody]UserTriggerUpdate dto)
        {
            return _userTriggerService.Update(dto);
        }
        #endregion

        #region 查询所有用户通知
        /// <summary>
        /// 查询所有用户通知[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<UserTriggerInfo>> Get()
        {
            return _userTriggerService.Get();
        }
        #endregion

        #region 根据ID获取用户通知
        /// <summary>
        /// 根据ID获取单位信息
        /// </summary>
        /// <param name="id">单位ID</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, UserTriggerInfo> GetByID(long id)
        {
            return _userTriggerService.GetByID(id);
        }
        #endregion

        #region 删除用户通知
        /// <summary>
        /// 删除用户通知[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">删除用户通知</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Delete([FromBody]UserTriggerDelete dto)
        {
            return _userTriggerService.Delete(dto);
        }
        #endregion


    }
}
