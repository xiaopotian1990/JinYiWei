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
    /// 仓库管理接口s
    /// </summary>
   public interface ISmartWarehouseService
    {
        /// <summary>
        /// 添加仓库管理
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(SmartWarehouseAdd dto);

        /// <summary>
        /// 修改仓库管理
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(SmartWarehouseUpdate dto);

        /// <summary>
        /// 查询所有仓库信息
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<SmartWarehouseInfo>> Get(string hospitalId);

        /// <summary>
        /// 查询所有仓库信息(根据用户id查询用户所能操作的仓库)
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<SmartWarehouseInfo>> GetByUserId(string userId,string hospitalId);

        /// <summary>
        /// 根据ID获取仓库信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, SmartWarehouseInfo> GetByID(long id);

        /// <summary>
        /// 仓库使用停用
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        //IFlyDogResult<IFlyDogResultType, int> SmartWarehouseDispose(SmartWarehouseStopOrUse dto);

        /// <summary>
        /// 删除仓库信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Delete(SmartWarehouseDelete dto);

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect(long hospitalID);

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelectByUserID(long hospitalID, long userID);
    }
}
