using System.Collections.Generic;
using Com.IFlyDog.CommonDTO;
using Com.IFlyDog.APIDTO;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    /// <summary>
    ///     工具接口
    /// </summary>
    public interface ISmartToolService
    {
        /// <summary>
        ///     添加工具
        /// </summary>
        /// <param name="dto">银行卡信息</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(SmartToolAdd dto);

        /// <summary>
        ///     更新工具信息
        /// </summary>
        /// <param name="dto">银行卡信息</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(SmartToolUpdate dto);

        /// <summary>
        ///     工具使用停用
        /// </summary>
        /// <param name="dto">参数集</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> StopOrUse(SmartToolStopOrUse dto);

        /// <summary>
        ///     查询所有工具
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<SmartToolInfo>> Get();

        /// <summary>
        ///     查询工具详细
        /// </summary>
        /// <param name="id">银行卡ID</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, SmartToolInfo> GetByID(long id);

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect();
    }
}