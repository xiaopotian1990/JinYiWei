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
    /// 库存盘点接口
    /// </summary>
   public interface ICheckService
    {
        /// <summary>
        /// 添加盘点信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(CheckAdd dto);

        /// <summary>
        /// 修改盘点信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(CheckUpdate dto);

        /// <summary>
        /// 查询所有盘点信息
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<CheckInfo>>> Get(CheckSelect dto);

        /// <summary>
        /// 根据ID获取盘点信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, CheckInfo> GetByID(long id);
    }
}
