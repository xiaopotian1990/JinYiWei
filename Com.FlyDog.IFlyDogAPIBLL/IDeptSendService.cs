using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    public interface IDeptSendService
    {
        /// <summary>
        /// 科室发料请求
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<DeptSendInfo>>> GetDeptSendInfo(long hospitalID, long userID);

        /// <summary>
        /// 发货
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> Send(DeptSendAdd dto);

        /// <summary>
        /// 今日发货记录
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <param name="userID"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<DeptSend>>> GetDeptSendToday(long hospitalID, long userID);
    }
}
