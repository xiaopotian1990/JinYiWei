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
    /// 客户归属权接口
    /// </summary>
    public interface IOwnerShipService
    {
        /// <summary>
        /// 单个添加开发人员客户归属权
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> SingleDeveLoperUserUpdateAdd(SingleDeveLoperUserUpdate dto);

        /// <summary>
        /// 单个添加咨询人员客户归属权
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> SingleConsultantUserUpdateAdd(SingleConsultantUserUpdate dto);

        /// <summary>
        /// 批量设置开发人员归属权
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> BatchDeveloperUserAdd(BatchDeveloperUser dto);

        /// <summary>
        /// 批量设置咨询人员归属权
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> BatchConsultantUserAdd(BatchConsultantUser dto);

        /// <summary>
        /// 查询当前医院客户归属权管理
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<OwnerShipInfo>> Get(long hospitalID);

        /// <summary>
        /// 根据条件查询客户信息
        /// </summary>
        /// <param name="type">1 查询开发类型 2 查询咨询类型</param>
        /// <param name="userID">查询开发或者咨询人员客户</param>
        /// <param name="hospitalID">当前医院</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<SingleCustormInfo>> GetByFiltrate(string type, long userID, long hospitalID);
    }
}
