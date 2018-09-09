using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    public interface ITreatService
    {
        /// <summary>
        /// 添加预约
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> Add(TreatAdd dto);

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
        Task<IFlyDogResult<IFlyDogResultType, int>> Update(TreatUpdate dto);

        /// <summary>
        /// 获取预约详细信息
        /// </summary>
        /// <param name="ID">预约记录ID</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, TreatDetail>> GetDetail(long ID);

        /// <summary>
        /// 获取预约记录
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<Treat>>> Get(long hospitalID, DateTime startTime, DateTime endTime);

        /// <summary>
        /// 获取预约记录
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <param name="startTime"></param>
        /// <param name="endTime"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<TreatDay>>> GetDayDetail(long hospitalID, DateTime date);
    }
}
