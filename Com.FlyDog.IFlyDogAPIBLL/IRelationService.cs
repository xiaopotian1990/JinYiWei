using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System.Collections.Generic;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    /// <summary>
    /// 关系管理接口
    /// </summary>
    public  interface IRelationService
    {
        /// <summary>
        /// 添加关系
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(RelationAdd dto);

        /// <summary>
        /// 修改关系
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(RelationUpdate dto);

        /// <summary>
        /// 查询所有关系信息
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<RelationInfo>> Get();

        /// <summary>
        /// 根据ID获取关系信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, RelationInfo> GetByID(long id);

        /// <summary>
        /// 删除关系
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Delete(RelationDelete dto);

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect();
    }
}
