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
    /// 单项目管理接口
    /// </summary>
   public interface IClubService
    {
        /// <summary>
        /// 添加单项目管理
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(ClubAdd dto);

        /// <summary>
        /// 查询所有单项目管理
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<ClubInfo>> Get(string hospitalID);

        /// <summary>
        /// 删除单项目管理
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Delete(ClubDelete dto);
    }
}
