using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.IFlyDogAPIBLL
{
   public interface ISmartUnitService
    {
        /// <summary>
        /// 添加单位
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(SmartUnitAdd dto);

        /// <summary>
        /// 修改单位
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(SmartUnitUpdate dto);


        /// <summary>
        /// 查询所有单位信息
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<SmartUnitInfo>> Get();

        /// <summary>
        /// 根据ID获取单位信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, SmartUnitInfo> GetByID(long id);

        /// <summary>
        /// 删除单位
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Delete(SmartUnitDelete dto);

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect();
    }
}
