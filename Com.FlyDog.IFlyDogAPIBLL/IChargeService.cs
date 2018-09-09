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
    /// 收费项目类型接口
    /// </summary>
   public interface IChargeService
    {
        /// <summary>
        /// 添加收费项目信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(ChargeAdd dto);

        /// <summary>
        /// 修改收费项目信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(ChargeUpdate dto);

        /// <summary>
        /// 查询所有收费项目信息
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<ChargeInfo>>> Get(ChargeSelect dto);

        /// <summary>
        /// 检测收费项目
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<ChargeCheckItem>>> GetCheckCharge(ChargeSelect dto);

        /// <summary>
        /// 查询所有收费项目（不分页给弹窗使用）
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<ChargeInfo>> GetData(ChargeSelect dto);

        /// <summary>
        ///根据id查询收费项目信息详情
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, ChargeInfo> GetByID(long id);
    }
}
