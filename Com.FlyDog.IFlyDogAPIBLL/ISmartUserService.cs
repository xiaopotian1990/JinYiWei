using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    /// <summary>
    /// 用户查询dto
    /// </summary>
   public interface ISmartUserService
    {
        /// <summary>
        /// 按条件查询用户
        /// </summary>
        /// <param name="dto">条件</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<SmartUserInfo>> Get(SmartUserSelect dto);

        /// <summary>
        /// 按条件查询用户（分页）
        /// </summary>
        /// <param name="dto">条件</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<SmartUserInfo>>> GetPages(SmartUserSelect dto);

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="dto">用户信息</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(UserAdd dto);

        /// <summary>
        /// 用户修改
        /// </summary>
        /// <param name="dto">用户信息</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(UserAdd dto);

        /// <summary>
        /// 用户使用停用
        /// </summary>
        /// <param name="dto">用户信息</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> StopOrUse(UserStopOrUse dto);

        /// <summary>
        /// 密码重置
        /// </summary>
        /// <param name="dto">重置信息</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> PasswordReset(UserPasswordReset dto);

        /// <summary>
        /// 设置顾客权限
        /// </summary>
        /// <param name="dto">访问权限</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> SetCustomerPermission(UserCustomerPermission dto);

        /// <summary>
        /// 设置访问权限
        /// </summary>
        /// <param name="dto">访问权限</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> SetCustomerCallBackPermission(UserCustomerPermission dto);

        /// <summary>
        /// 根据ID查询详细
        /// </summary>
        /// <param name="id">用户ID</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, SmartUserInfo> GetDetail(long id);

        /// <summary>
        /// 获取用户客户权限详细信息
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, UserCustomerPermissionDetail>> GetCustomerPermissionDetail(long userID);

        /// <summary>
        /// 获取用户回访权限详细信息
        /// </summary>
        /// <param name="userID">用户ID</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, UserCustomerPermissionDetail>> GetCallBackPermissionDetail(long userID);

        /// <summary>
        /// 获取分疹人员列表
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<FZUser>> GetFZUsers(string hospitalID);

        /// <summary>
        /// 获取当前医院参与排班的用户
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<CYPBUser>> GetCYPBUsers(SmartUserSelect dto);


        /// <summary>
        /// 缓存所有用户通用方法
        /// </summary>
        /// <returns></returns>
        IEnumerable<SmartUserInfo> GetAll();

        /// <summary>
        /// 获取参与预约用户
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<Select>>> GetSSYYUsers(long hospitalID, DateTime date);
    }
}
