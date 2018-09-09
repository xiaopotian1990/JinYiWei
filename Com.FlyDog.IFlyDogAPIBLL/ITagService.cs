using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System.Collections.Generic;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    public interface ITagService
    {
        /// <summary>
        /// 添加顾客标签
        /// </summary>
        /// <param name="dto">顾客标签信息</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(TagAdd dto);

        /// <summary>
        /// 更新顾客标签信息
        /// </summary>
        /// <param name="dto">顾客标签信息</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(TagUpdate dto);

        /// <summary>
        /// 顾客标签停用
        /// </summary>
        /// <param name="dto">参数集</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> StopOrUse(TagStopOrUse dto);

        /// <summary>
        /// 查询所有顾客标签
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<Tag>> Get();

        /// <summary>
        /// 查询顾客所有可用的标签
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<Tag>> GetByIsOk();

        /// <summary>
        /// 查询顾客标签详细
        /// </summary>
        /// <param name="id">顾客标签ID</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, Tag> GetByID(long id);

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect();
    }
}
