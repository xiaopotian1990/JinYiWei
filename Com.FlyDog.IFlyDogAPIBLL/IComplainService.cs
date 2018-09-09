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
    ///投诉处理接口
    /// </summary>
   public interface IComplainService
    {

        /// <summary>
        /// 处理投诉
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(ComplainUpdate dto);

        /// <summary>
        /// 查询当前医院所有投诉处理
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<ComplainInfo>>> Get(ComplainSelect dto);

        /// <summary>
        /// 根据id获取投诉信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, ComplainInfo> GetByID(long id);
    }
}
