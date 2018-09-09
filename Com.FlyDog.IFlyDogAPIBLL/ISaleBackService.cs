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
    /// 回款记录接口
    /// </summary>
   public interface ISaleBackService
    {
        /// <summary>
        /// 添加回款记录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(SaleBackAdd dto);

        /// <summary>
        /// 修改回款记录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(SaleBackUpdate dto);

        /// <summary>
        /// 查询所有回款记录
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<SaleBackInfo>>> Get(SaleBackSelect dto);

        /// <summary>
        /// 根据ID获取回款记录
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, SaleBackInfo> GetByID(long id);

        /// <summary>
        /// 删除回款记录
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Delete(SaleBackDelete dto);
    }
}
