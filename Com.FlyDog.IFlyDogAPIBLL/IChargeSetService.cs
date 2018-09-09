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
    /// 套餐管理接口
    /// </summary>
  public  interface IChargeSetService
    {
        /// <summary>
        /// 添加套餐管理
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(ChargeSetAdd dto);

        /// <summary>
        /// 修改套餐管理
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(ChargeSetUpdate dto);

        /// <summary>
        /// 查询所有套餐信息
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<ChargeSetInfo>>> Get(ChargeSetSelect dto);

        /// <summary>
        /// 根据ID获取套餐信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, ChargeSetInfo> GetByID(long id);

    }
}
