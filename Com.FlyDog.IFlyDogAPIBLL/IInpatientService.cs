using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    public interface IInpatientService
    {
        /// <summary>
        /// 住院
        /// </summary>
        /// <param name="dto">住院信息</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> In(InpatientAdd dto);

        /// <summary>
        /// 出院
        /// </summary>
        /// <param name="dto">出院信息</param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, int>> Out(Inpatientout dto);

        /// <summary>
        /// 住院工作台住院列表
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<InpatientIn>>> GetIn(long hospitalID);
    }
}
