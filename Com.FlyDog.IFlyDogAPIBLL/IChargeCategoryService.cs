using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System.Collections.Generic;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    /// <summary>
    /// 项目分类
    /// </summary>
    public interface IChargeCategoryService
    {
        /// <summary>
        /// 添加项目分类
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(ChargeCategoryAdd dto);


        /// <summary>
        /// 修改项目分类
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(ChargeCategoryUpdate dto);

        /// <summary>
        /// 查询所有分类信息
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<ChargeCategoryInfo>> Get();

        /// <summary>
        /// 根据ID获取分类信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, ChargeCategoryInfo> GetByID(long id);

        /// <summary>
        /// 删除分类信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Delete(ChargeCategoryDelete dto);

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect();
    }
}
