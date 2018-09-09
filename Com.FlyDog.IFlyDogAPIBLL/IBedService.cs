using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    public interface IBedService
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(BedAdd dto);

        /// <summary>
        /// 部门修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(BedAdd dto);

        /// <summary>
        /// 查询当前医院下的所有直接部门
        /// </summary>
        /// <param name="hospitalID">所属医院ID</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<Bed>> Get(long hospitalID, CommonStatus status = CommonStatus.All);

        /// <summary>
        /// 根据ID获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, Bed> GetByID(long id);

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect(long hospitalID);

        /// <summary>
        /// 使用停用
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> StopOrUse(BedStop dto);
    }
}
