using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System.Collections.Generic;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    /// <summary>
    /// 知识分类服务
    /// </summary>
    public interface IKnowledgeCategoryService
    {
        /// <summary>
        /// 添加知识分类
        /// </summary>
        /// <param name="dto">知识分类</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(KnowledgeCategoryAdd dto);

        /// <summary>
        /// 知识分类修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(KnowledgeCategoryUpdate dto);


        /// <summary>
        /// 查询所有知识分类
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<KnowledgeCategory>> Get();

        /// <summary>
        /// 根据id查询知识分类
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, KnowledgeCategory> GetByID(long id);

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect();


    }
}
