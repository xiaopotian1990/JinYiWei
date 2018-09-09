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
    /// 关系管理api
    /// </summary>
    public class RelationController : ApiController
    {
        private IRelationService _relationService;

        #region 依赖注入类
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="relationService"></param>
        public RelationController(IRelationService relationService)
        {
            _relationService = relationService;
        }
        #endregion

        #region 添加关系
        /// <summary>
        /// 添加关系
        /// </summary>
        /// <param name="dto">关系</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add([FromBody]RelationAdd dto)
        {
            return _relationService.Add(dto);
        }
        #endregion

        #region 修改关系
        /// <summary>
        /// 修改关系
        /// </summary>
        /// <param name="dto">关系信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update([FromBody]RelationUpdate dto)
        {
            return _relationService.Update(dto);
        }
        #endregion

        #region 查询所有关系信息
        /// <summary>
        /// 查询所有关系信息[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<RelationInfo>> Get()
        {
            return _relationService.Get();
        }
        #endregion

        #region 根据ID获取关系信息
        /// <summary>
        /// 根据ID获取关系信息
        /// </summary>
        /// <param name="id">关系id</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, RelationInfo> GetByID(long id)
        {
            return _relationService.GetByID(id);
        }
        #endregion

        #region 删除关系信息
        /// <summary>
        /// 删除关系信息
        /// </summary>
        /// <param name="dto">关系信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Delete([FromBody]RelationDelete dto)
        {
            return _relationService.Delete(dto);
        }
        #endregion

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect()
        {
            return _relationService.GetSelect();
        }
    }
}
