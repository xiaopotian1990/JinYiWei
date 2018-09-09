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
    /// 店家负责人接口
    /// </summary>
    public interface IStoreManagerService
    {
        /// <summary>
        /// 添加店家负责人
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(StoreManagerAdd dto);

        /// <summary>
        /// 根据医院id,用户id,查询所有店家负责人
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<StoreUserManager>> GetByHospitalID(string hospitalID, string userID);

        /// <summary>
        /// 根据用户id查询用户所负责的店家信息
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<StoreInfoManager>> GetUserID(string userID);

        /// <summary>
        ///删除店家负责人下的店铺
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Delete(StoreManagerDelete dto);

        /// <summary>
        ///删除店家负责人及负责人下的店铺
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> DeleteByUserID(StoreManagerDelete dto);
    }
}
