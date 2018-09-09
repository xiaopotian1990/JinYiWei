using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    public interface ISurgeryService
    {
        /// <summary>
        /// 添加预约
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> Add(SurgeryAdd dto);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="dto">删除信息</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> Delete(SurgeryDelete dto);

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="dto">修改信息</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> Update(SurgeryUpdate dto);

        /// <summary>
        /// 获取预约详细信息
        /// </summary>
        /// <param name="ID">预约记录ID</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, SurgeryDetail>> GetDetail(long ID);

        /// <summary>
        /// 手术排台
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<Surgery>>> Get(long hospitalID, DateTime date);

        /// <summary>
        /// 开始结束手术
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> Done(SugeryDone dto);
    }
}
