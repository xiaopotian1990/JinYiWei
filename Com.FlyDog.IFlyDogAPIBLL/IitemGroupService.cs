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
    /// 报表项目组接口
    /// </summary>
   public interface IitemGroupService
    {
        /// <summary>
        /// 添加报表项目组
        /// </summary>
        /// <param name="dto">添加报表项目组</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(ItemGroupAdd dto);

        /// <summary>
        /// 更新报表项目组
        /// </summary>
        /// <param name="dto">更新报表项目组</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(ItemGroupUpdate dto);

        /// <summary>
        /// 查询所有报表项目组
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<ItemGroupInfo>> Get();

        /// <summary>
        /// 根据ID获取报表项目组
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, ItemGroupInfo> GetByID(long id);

        /// <summary>
        /// 删除报表项目组
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Delete(ItemGroupDelete dto);

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect();
    }
}
