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
    /// 分享家接口
    /// </summary>
    public interface IShareCategoryService
    {
        /// <summary>
        /// 添加分享家
        /// </summary>
        /// <param name="dto">添加分享家</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(ShareCategoryAdd dto);

        /// <summary>
        /// 更新分享家信息
        /// </summary>
        /// <param name="dto">更新分享家信息</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(ShareCategoryUpdate dto);

        /// <summary>
        /// 分享家删除
        /// </summary>
        /// <param name="dto">参数集</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Delete(ShareCategoryDelete dto);

        /// <summary>
        /// 查询所有分享家
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<ShareCategoryInfo>> Get();

        /// <summary>
        /// 根据分享家id查询详细
        /// </summary>
        /// <param name="id">分享家ID</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, ShareCategoryInfo> GetByID(long id);

        /// <summary>
        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect();
    }
}
