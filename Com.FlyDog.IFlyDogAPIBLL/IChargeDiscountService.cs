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
    /// 项目折扣接口
    /// </summary>
  public interface IChargeDiscountService
    {
        /// <summary>
        /// 添加项目折扣
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(ChargeDiscountAdd dto);

        /// <summary>
        /// 修改项目折扣
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(ChargeDiscountUpdate dto);

        /// <summary>
        /// 查询所有项目折扣
        /// </summary>SmartSupplierSelect
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<ChargeDiscountInfo>>> Get(ChargeDiscountSelect dto);

        /// <summary>
        /// 根据ID获取供应商信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, ChargeDiscountInfo> GetByID(long id);

        /// <summary>
        /// 获取项目折扣
        /// </summary>
        /// <param name="hospitalID"></param>
        /// <returns></returns>
        Task<IFlyDogResult<IFlyDogResultType, IEnumerable<ChargeDiscount>>> GetChargesDiscount(long hospitalID);
    }
}
