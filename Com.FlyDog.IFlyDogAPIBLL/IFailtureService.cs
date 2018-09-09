using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    public interface IFailtureService
    {
        /// <summary>
        /// 获取顾客未成交列表
        /// </summary>
        /// <param name="customerID">顾客ID</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<Failture>>> GetByCustomerID(long customerID);

        /// <summary>
        /// 获取未成交详细信息
        /// </summary>
        /// <param name="ID">咨记录ID</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, FailtureDetail>> GetDetail(long ID);

        /// <summary>
        /// 未成交修改
        /// </summary>
        /// <param name="dto">未成交信息</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> Update(FailtureAddUpdate dto);

        /// <summary>
        /// 添加未成交
        /// </summary>
        /// <param name="dto">未成交信息</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> Add(FailtureAddUpdate dto);

        /// <summary>
        /// 未成交删除
        /// </summary>
        /// <param name="dto">删除信息</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> Delete(FailtureDelete dto);
    }
}
