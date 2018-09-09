using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.FlyDog.IFlyDogAPIBLL
{
   public interface ISmartSupplierService
    {
        /// <summary>
        /// 添加供应商管理
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(SmartSupplierAdd dto);

        /// <summary>
        /// 修改供应商管理
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(SmartSupplierUpdate dto);

        /// <summary>
        /// 查询所有供应商信息
        /// </summary>SmartSupplierSelect
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<SmartSupplierInfo>>> Get(SmartSupplierSelect dto);

        /// <summary>
        /// 查询所有供应商信息（不分页，主要给下拉列表使用） string hospitalID
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<SmartSupplierInfo>> GetAll(string hospitalIDD);
        /// <summary>
        /// 根据ID获取供应商信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, SmartSupplierInfo> GetByID(long id);

        /// <summary>
        /// 删除供应商信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Delete(SmartSupplierDelete dto);

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect(long hospitalID);
    }
}
