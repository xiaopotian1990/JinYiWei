using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System.Collections.Generic;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    /// <summary>
    /// 回访类型设置
    /// </summary>
    public interface ICallbackCategoryService
    {

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(SmartCallbackCategoryAdd dto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(SmartCallbackCategoryUpdate dto);

        /// <summary>
        /// 使用停用
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> StopOrUse(SmartCallbackCategoryStopOrUse dto);

        /// <summary>
        /// 查询所有
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<SmartCallbackCategory>> Get(); 

        /// <summary>
        /// 根据ID获取一条信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, SmartCallbackCategory> GetByID(long id);

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect();
    }
}
