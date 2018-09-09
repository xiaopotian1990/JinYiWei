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
    /// 药物品设置接口
    /// </summary>
   public interface ISmartProductService
    {
        /// <summary>
        /// 添加药物品设置
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(SmartProductAdd dto);

        /// <summary>
        /// 修改药物品设置
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(SmartProductUpdate dto);

        /// <summary>
        /// 查询所有药物品设置
        /// </summary>SmartSupplierSelect
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<SmartProductInfo>>> Get(SmartProductSelect dto);

        /// <summary>
        /// 查询所有药物品信息（不分页，主要给列表使用） string hospitalID
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<SmartProductInfo>> GetAll(SmartProductSelect dto);

        /// <summary>
        /// 根据仓库id查询所有药物品信息（不分页，主要给列表使用） 
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<SmartProductInfo>> GetByWarehouseIDDataAll(SmartProductSelect dto);

        /// <summary>
        /// 根据ID获取药物品设置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, SmartProductInfo> GetByID(long id);

        /// <summary>
        /// 药物品设置使用停用
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> SmartProductDispose(SmartProductStopOrUse dto);


    }
}
