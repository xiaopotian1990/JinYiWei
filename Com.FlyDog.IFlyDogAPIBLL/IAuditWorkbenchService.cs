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
    /// 审核工作台
    /// </summary>
    public interface IAuditWorkbenchService
    {
        /// <summary>
        /// 查询所有待审核信息
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<AuditWorkbenchInfo>>> GetAllAudit(AuditWorkbenchSelect dto);

        /// <summary>
        /// 查询所有审核记录信息
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<AuditRecordInfo>>> GetAuditRecord(AuditRecordSelect dto);

        /// <summary>
        /// 点击查询查询此类型的审核用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, AuditUserInfo> GetByType(AuditUserSelect dto);


        /// <summary>
        /// 审核操作
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> AuditOperationAdd(AuditOperationAdd dto);

        /// <summary>
        /// 点击审核跳转到审核界面如果是开发人员或者咨询人员需要传递类型 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="type">4 开发人员  5咨询人员</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, AuditOrderInfo> GetAuditOrderInfo(long id,string type);
        
    }
}
