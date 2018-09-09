using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    public interface ICustomerService
    {
        /// <summary>
        /// 添加顾客
        /// </summary>
        /// <param name="dto">顾客信息</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, long>> AddAsync(CustomerAdd dto);

        /// <summary>
        /// 顾客识别
        /// </summary>
        /// <param name="name">各种识别码</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<CustomerIdentifyInfo>>> CustomerIdentifyAsync(string name);

        /// <summary>
        /// 查询今日登记顾客
        /// </summary>
        /// <param name="hospitalID">医院ID</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<CustomerCreateToday>>> CustomerCreateTodayAsync(long hospitalID,CustomerRegisterType type);

        /// <summary>
        /// 客户管理查询
        /// </summary>
        /// <param name="dto">筛选条件</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<CustomerManager>>>> GetCustomerManager(CustomerSelect dto);
    }
}
