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
    /// 仓库调拨接口
    /// </summary>
   public interface IAllocateService
    {
        /// <summary>
        /// 添加调拨
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(AllocateAdd dto);

        /// <summary>
        /// 修改调拨信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(AllocateUpdate dto);

        /// <summary>
        /// 查询所有调拨信息
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<AllocateInfo>>> Get(AllocateSelect dto);

        /// <summary>
        /// 根据ID获取调拨信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, AllocateInfo> GetByID(long id);

        /// <summary>
        /// 删除调拨信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> SmartReturnDelete(AllocateDelete dto);

        /// <summary>
        /// 根据仓库退货id查询仓库退货详情拼接成字符串打印出来
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, string> SmartAllocatePrint(string allocateID, long hospitalID);
    }
}
