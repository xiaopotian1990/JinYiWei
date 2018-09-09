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
    /// 卷类型接口
    /// </summary>
    public interface ICouponCategoryService
    {
        /// <summary>
        /// 添加卷类型管理
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(CouponCategoryAdd dto);

        /// <summary>
        /// 修改卷类型管理
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(CouponCategoryUpdate dto);

        /// <summary>
        /// 查询所有卷类型管理
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<CouponCategoryInfo>> Get();

        /// <summary>
        /// 根据医院id查询当前医院可使用的全部卷类型
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<CouponCategoryInfo>> GetByHospitalID(long hospitalID);

        /// <summary>
        /// 根据ID获取卷类型信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, CouponCategoryInfo> GetByID(long id);

        /// <summary>
        /// 删除卷类型信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Delete(CouponCategoryDelete dto);

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect(long hospitalID);
    }
}
