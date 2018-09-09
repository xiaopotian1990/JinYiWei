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
    /// 我的审核申请接口
    /// </summary>
   public interface IAuditApplyService
    {
        /// <summary>
        /// 查询当前用户所有的审核申请
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<AuditApplyInfo>>> Get(AuditApplySelect dto);

        /// <summary>
        ///  根据操作i的，类型查询审核详情
        /// </summary>
        /// <param name="orderID">操作id，目前主要是咨询或者开发人员变更id</param>
        /// <param name="type"> 4 咨询人员变更 5 开发人员变更</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, OwnerShipOrderAudit> GetAuditDetail(long orderID,string type, long hospitalID, long userID);

        /// <summary>
        /// 取消/删除我的审核申请
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Delete(AuditApplyDelete dto);
    }
}
