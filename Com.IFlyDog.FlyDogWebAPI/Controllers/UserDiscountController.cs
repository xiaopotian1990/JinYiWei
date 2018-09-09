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
    /// 用户折扣api
    /// </summary>
    public class UserDiscountController : ApiController
    {
        private IUserDiscountService _userDiscountService;

        #region 依赖注入类
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="userDiscountService"></param>
        public UserDiscountController(IUserDiscountService userDiscountService)
        {
            _userDiscountService = userDiscountService;
        }
        #endregion

        #region 添加用户折扣
        /// <summary>
        /// 添加用户折扣[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add([FromBody]UserDiscountAdd dto)
        {
            return _userDiscountService.Add(dto);
        }
        #endregion

        #region 修改用户折扣
        /// <summary>
        /// 修改用户折扣[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update([FromBody]UserDiscountUpdate dto)
        {
            return _userDiscountService.Update(dto);
        }
        #endregion

        #region 查询所有用户折扣信息
        /// <summary>
        /// 查询所有用户折扣信息[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<UserDiscountInfo>> Get()
        {
            return _userDiscountService.Get();
        }
        #endregion

        /// <summary>
        /// 获取全部用户折扣信息（分页）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<UserDiscountInfo>>> GetPage(UserDiscountSelect dto)
        {
            return _userDiscountService.GetPage(dto);
        }

        #region 根据id获取用户折扣信息
            /// <summary>
            /// 根据id获取用户折扣信息
            /// </summary>
            /// <param name="id">单位ID</param>
            /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, UserDiscountInfo> GetByID(long id)
        {
            return _userDiscountService.GetByID(id);
        }
        #endregion
    }
}
