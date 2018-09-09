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
    /// 排班管理接口
    /// </summary>
    public interface IShiftService
    {
        /// <summary>
        /// 添加排班管理
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(ShiftAdd dto);

        /// <summary>
        /// 修改排班管理
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(ShiftUpdate dto);

        /// <summary>
        /// 查询所有排班信息
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, Shift> Get(long hospitalID, int number);

        /// <summary>
        /// 根据ID查询排班信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, ShiftInfo> GetByID(long id);

        /// <summary>
        /// 删除排班信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Delete(ShiftDelete dto);
    }
}
