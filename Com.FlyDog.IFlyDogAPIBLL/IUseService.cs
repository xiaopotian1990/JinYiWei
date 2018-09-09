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
    /// 科室领用接口
    /// </summary>
   public interface IUseService
    {
        /// <summary>
        /// 添加科室领用
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(UseAdd dto);

        /// <summary>
        /// 修改科室领用
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(UseUpdate dto);

        /// <summary>
        /// 查询所有科室领用信息
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<UseInfo>>> Get(UseSelect dto);

        /// <summary>
        /// 根据ID获取科室领用信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, UseInfo> GetByID(long id);

        /// <summary>
        /// 根据科室领用id查询科室领用详情拼接成字符串打印出来
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, string> SmartUsePrint(string UseID, long hospitalID);
    }
}
