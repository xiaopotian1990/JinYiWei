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
    /// 进货管理相关接口
    /// </summary>
  public  interface ISmartPurchaseService
    {

        /// <summary>
        /// 添加进货信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(SmartPurchaseAdd dto);

        /// <summary>
        /// 修改进货信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(SmartPurchaseUpdate dto);

        /// <summary>
        /// 查询所有进货信息
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<SmartPurchaseInfo>>> Get(SmartPurchaseSelect dto);


        /// <summary>
        ///根据医院id查询医院内的进货记录
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<SmartPurchaseInfo>> GetByHospitalID(SmartPurchaseSelect dto);

      

        /// <summary>
        /// 根据ID获取进货信息id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, SmartPurchaseInfo> GetByID(long id);

        /// <summary>
        /// 删除进货信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> SmartPurchaseDelete(SmartPurchaseDelete dto);

        /// <summary>
        /// 根据仓库进货id查询仓库进货详情拼接成字符串打印出来
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, string> SmartPurchasePrint(string purchaspID, long hospitalID);
    }
}
