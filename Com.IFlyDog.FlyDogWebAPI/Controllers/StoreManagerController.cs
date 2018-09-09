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
    /// 店家负责人api
    /// </summary>
    public class StoreManagerController : ApiController
    {
        private IStoreManagerService _storeManagerService;

        #region 依赖注入类
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="storeManagerService"></param>
        public StoreManagerController(IStoreManagerService storeManagerService)
        {
            _storeManagerService = storeManagerService;
        }
        #endregion

        #region 添加店铺负责人
        /// <summary>
        /// 添加店铺负责人[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">单位信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add([FromBody]StoreManagerAdd dto)
        {
            return _storeManagerService.Add(dto);
        }
        #endregion

        /// <summary>
        /// 根据医院id查询所有店家负责人
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<StoreUserManager>> GetByHospitalID(string hospitalID, string userID)
        {
            return _storeManagerService.GetByHospitalID(hospitalID, userID);
        }

        /// <summary>
        /// 根据用户id查询用户所负责的店家信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<StoreInfoManager>> GetUserID(string userID)
        {
            return _storeManagerService.GetUserID(userID);
        }

        #region 删除店铺负责人
        /// <summary>
        /// 删除店铺负责人[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">删除店铺负责人</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Delete([FromBody]StoreManagerDelete dto)
        {
            return _storeManagerService.Delete(dto);
        }
        #endregion

        /// <summary>
        /// 删除当前负责人所负责的所有店铺
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> DeleteByUserID(StoreManagerDelete dto)
        {
            return _storeManagerService.DeleteByUserID(dto);
        }
    }
}
