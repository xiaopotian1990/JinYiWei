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
    /// 标签组管理接口
    /// </summary>
   public interface ITagGroupService
    {
        /// <summary>
        /// 添加标签组
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(TagGroupAdd dto);

        /// <summary>
        /// 修改标签组
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(TagGroupUpdate dto);

        /// <summary>
        /// 查询所有标签组
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<TagGroupInfo>> Get();

        /// <summary>
        /// 根据id获取标签组信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, TagGroupInfo> GetByID(long id);

        /// <summary>
        /// 删除标签组信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Delete(TagGroupDelete dto);
    }
}
