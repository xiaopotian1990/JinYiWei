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
    /// 角色相关API
    /// </summary>
    public class RoleController : ApiController
    {
        /// <summary>
        /// 依赖注入
        /// </summary>
        private IRoleService _roleService;

        /// <summary>
        /// 依赖注入方法重构
        /// </summary>
        /// <param name="roleService"></param>
        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// 查询所有菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<MenuRole>> GetRoleMenu()
        {
            return _roleService.GetRoleMenu();
        }

        /// <summary>
        /// 角色添加
        /// </summary>
        /// <param name="dto">角色信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add(RoleAdd dto)
        {
            return _roleService.Add(dto);
        }

        /// <summary>
        /// 查询所以角色
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Role>> GetAllRole(long userHositalID, long hositalID)
        {
            return _roleService.GetAllRole(userHositalID, hositalID);
        }

        /// <summary>
        /// 查询角色详细
        /// </summary>
        /// <param name="userHositalID">操作人所属ID</param>
        /// <param name="hositalID">角色所属ID</param>
        /// <param name="roleID">角色ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, RoleDetail> GetRoleDetail(long userHositalID, long hositalID, long roleID)
        {
            return _roleService.GetRoleDetail(userHositalID, hositalID, roleID);
        }

        /// <summary>
        /// 角色删除
        /// </summary>
        /// <param name="dto">角色信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Delete(RoleDelete dto)
        {
            return _roleService.Delete(dto);
        }
        /// <summary>
        /// 角色更新
        /// </summary>
        /// <param name="dto">角色信息</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update(RoleUpdate dto)
        {
            return _roleService.Update(dto);
        }
    }
}
