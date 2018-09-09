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
    /// 单项目管理api
    /// </summary>
    public class ClubController : ApiController
    {
        private IClubService _clubService;

        #region 依赖注入类
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="clubService"></param>
        public ClubController(IClubService clubService)
        {
            _clubService = clubService;
        }
        #endregion

        #region 添加单项目管理
        /// <summary>
        /// 添加单项目管理[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add([FromBody]ClubAdd dto)
        {
            return _clubService.Add(dto);
        }
        #endregion

        #region 查询所有单项目管理信息
        /// <summary>
        /// 查询所有单项目管理信息[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<ClubInfo>> Get(string hospitalID)
        {
            return _clubService.Get(hospitalID);
        }
        #endregion

        #region 删除单项目管理信息
        /// <summary>
        /// 删除单项目管理信息[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Delete([FromBody]ClubDelete dto)
        {
            return _clubService.Delete(dto);
        }
        #endregion
    }
}
