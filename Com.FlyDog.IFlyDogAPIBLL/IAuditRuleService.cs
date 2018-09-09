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
    /// 审核规则设置接口
    /// </summary>
   public interface IAuditRuleService
    {
        /// <summary>
        /// 添加审核规则
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(AuditRuleAdd dto);

        /// <summary>
        /// 查询当前医院下的具体的某一种审核规则是否存在
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <param name="Type"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> GetByHtData(string hospitalID, int Type);

        /// <summary>
        /// 查询所有审核规则信息
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<AuditRuleInfo>> Get(long hospitalID);


        /// <summary>
        /// 修改审核规则
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(AuditRuleUpdate dto);

        /// <summary>
        /// 根据ID获取审核规则详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, AuditRuleInfo> GetByID(long id);

        /// <summary>
        /// 启用停用审核规则
        /// </summary>
        /// <param name="dto">参数集</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> StopOrUse(AuditRuleStopOrUse dto);

    }
}
