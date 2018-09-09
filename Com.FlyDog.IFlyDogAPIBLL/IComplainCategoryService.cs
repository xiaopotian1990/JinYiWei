using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System.Collections.Generic;


namespace Com.FlyDog.IFlyDogAPIBLL
{
    /// <summary>
    /// 投诉类型
    /// </summary>
    public interface IComplainCategoryService
    {
        /// <summary>
        /// 添加投诉
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(SmartComplainCategoryAdd dto);

        /// <summary>
        /// 投诉修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(SmartComplainCategoryUpdate dto);

        /// <summary>
        /// 使用停用
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> StopOrUse(SmartComplainCategoryStopOrUse dto);

        /// <summary>
        /// 查询所有投诉
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<SmartComplainCategory>> Get();

        /// <summary>
        /// 根据ID获取投诉信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, SmartComplainCategory> GetByID(long id);

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect();
    }
}
