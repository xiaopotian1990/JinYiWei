using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using System.Collections.Generic;

namespace Com.FlyDog.IFlyDogAPIBLL
{
    public interface IProvinceService
    {
        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect();

        /// <summary>
        /// 根据省查询市
        /// </summary>
        /// <param name="provinceID">省ID</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetCity(int provinceID);

        /// <summary>
        /// 根据手机号自动识别省市
        /// </summary>
        /// <param name="phone">手机号</param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, ProvinceCity> GetProvinceCity(string phone);
    }
}
