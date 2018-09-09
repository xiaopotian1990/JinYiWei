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
    /// 退货信息接口
    /// </summary>
   public interface ISmartReturnService
    {
        /// <summary>
        /// 添加退货信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(SmartReturnAdd dto);

        /// <summary>
        /// 修改退货信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(SmartReturnUpdate dto);

        /// <summary>
        /// 查询所有退货信息
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<SmartReturnInfo>>> Get(SmartReturnSelect dto);

        /// <summary>
        /// 根据ID获取退货信息id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, SmartReturnInfo> GetByID(long id);

        /// <summary>
        /// 删除退货信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> SmartReturnDelete(SmartReturnDelete dto);

        /// <summary>
        /// 根据仓库退货id查询仓库退货详情拼接成字符串打印出来
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, string> SmartReturnPrint(string returnID, long hospitalID);
    }
}
