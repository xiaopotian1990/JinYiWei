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
    /// 单个添加变更、开发人员申请
    /// </summary>
   public interface IOwnerShipOrderService
    {
        /// <summary>
        /// 添加/ 编辑开发人员变更申请
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> CustomerDeveloperAdd(CustomerDeveloperAdd dto);

        /// <summary>
        /// 添加/编辑咨询人员变更申请
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> CustomerConsultanAdd(CustomerConsultanAdd dto);

        /// <summary>
        ///  咨询/开发人员变更申请 加载
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, CustomerUserInfo> GetCustomerUserInfo(CustomerUserSelect dto);

    }
}
