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
    /// /库存查询接口
    /// </summary>
   public interface ISmartStockService
    {
        /// <summary>
        /// 查询所有库存信息 (这个有问题，需要调整)
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<SmartStockInfo>> Get(SmartStockSelect dto);

        /// <summary>
        /// 根据条件查询库存商品（名称，分类，拼音码）
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<SmartStockProductInfo>> GetByCondition(SmartStockSelect dto);

        /// <summary>
        /// 根据库存id查询库存详情
        /// </summary>
        /// <param name="no">根据进货单号来查询库存信息</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, SmartStockInfo> GetByID(long Id);

        /// <summary>
        ///根据医院id查询医院内的进货记录(查询出来后按照到期时间排序)
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<SmartStockInfo>>> GetByHospitalIDData(SmartStockSelect dto);


        /// <summary>
        /// 添加库存（每个批次一条记录）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(SmartStockAdd dto);

        /// <summary>
        /// 修改库存（修改当前批次）
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(SmartStockUpdate dto);
    }
}
