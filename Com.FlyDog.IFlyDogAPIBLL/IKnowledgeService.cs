using Com.IFlyDog.APIDTO.Knowledge;
using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    /// <summary>
    /// 知识管理
    /// </summary>
    public interface IKnowledgeService
    {
        /// <summary>
        /// 添加知识管理
        /// </summary>
        /// <param name="dto">知识管理</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(KnowledgeAdd dto);

        /// <summary>
        /// 知识管理修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(KnowledgeUpdate dto);


        /// <summary>
        /// 查询所有知识管理
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<KnowledgeInfo>>> Get(KnowledgeSelect dto);

        /// <summary>
        /// 查询所有知识管理
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, KnowledgeInfo> GetByID(long id);
    }
}
