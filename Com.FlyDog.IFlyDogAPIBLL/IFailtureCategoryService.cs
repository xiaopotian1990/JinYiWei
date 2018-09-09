using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System.Collections.Generic;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    public interface IFailtureCategoryService
    {
        /// <summary>
        /// 添加未成交类型
        /// </summary>
        /// <param name="dto">症状信息</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(FailtureCategoryAdd dto);

        /// <summary>
        /// 未成交类型修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(FailtureCategoryUpdate dto);

        /// <summary>
        /// 未成交类型使用停用
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> StopOrUse(FailtureCategoryStopOrUse dto);

        /// <summary>
        /// 查询所有未成交类型
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<FailtureCategory>> Get();

        /// <summary>
        /// 根据ID查询详细未成交类型
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, FailtureCategory> GetByID(long id);

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect();
    }
}
