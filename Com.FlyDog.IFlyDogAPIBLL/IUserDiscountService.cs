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
    /// 用户折扣接口
    /// </summary>
  public  interface IUserDiscountService
    {
        /// <summary>
        /// 添加用户折扣
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(UserDiscountAdd dto);

        /// <summary>
        /// 修改用户折扣
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(UserDiscountUpdate dto);

        /// <summary>
        /// 查询所有供应商信息
        /// </summary>SmartSupplierSelect
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<UserDiscountInfo>>> GetPage(UserDiscountSelect dto);


        /// <summary>
        /// 查询所有用户折扣信息
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<UserDiscountInfo>> Get();

        /// <summary>
        /// 根据ID获取用户折扣信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, UserDiscountInfo> GetByID(long id);
    }
}
