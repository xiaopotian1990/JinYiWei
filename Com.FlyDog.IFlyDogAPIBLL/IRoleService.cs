using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    public interface IRoleService
    {
        /// <summary>
        /// 查询所有菜单
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<MenuRole>> GetRoleMenu(long roleID = 0);

        /// <summary>
        /// 角色添加
        /// </summary>
        /// <param name="dto">角色信息</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(RoleAdd dto);
        /// <summary>
        /// 查询所以角色
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<Role>> GetAllRole(long userHositalID, long hositalID);
        /// <summary>
        /// 查询角色详细
        /// </summary>
        /// <param name="userHositalID">操作人所属ID</param>
        /// <param name="hositalID">角色所属ID</param>
        /// <param name="roleID">角色ID</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, RoleDetail> GetRoleDetail(long userHositalID, long hositalID, long roleID);
        /// <summary>
        /// 角色删除
        /// </summary>
        /// <param name="dto">角色信息</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Delete(RoleDelete dto);
        /// <summary>
        /// 角色更新
        /// </summary>
        /// <param name="dto">角色信息</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(RoleUpdate dto);
    }
}
