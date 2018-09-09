using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    public interface IBillService
    {
        /// <summary>
        /// 查询可开发票项目
        /// </summary>
        /// <param name="customerID">顾客ID</param>
        /// <param name="hospitalID">医院ID</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<CanBillCharges>>> GetCanBillCharges(long customerID, long hospitalID);

        /// <summary>
        /// 添加发票
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> Add(BillAdd dto);

        /// <summary>
        /// 删除发票
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> Delete(BillDelete dto);

        /// <summary>
        /// 获取今日发票记录
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<Bill>>> GetBillToday(long hospitalID);
    }
}
