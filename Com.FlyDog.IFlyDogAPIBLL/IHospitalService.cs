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
    /// 医院接口
    /// </summary>
    public interface IHospitalService
    {
        /// <summary>
        /// 查询所有医院信息
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<HospitalInfo>> Get(long id);
        /// <summary>
        /// 查询医院
        /// </summary>
        /// <param name="id">查询所有医院输入0，其他输入医院ID</param>
        /// <returns></returns>
        bool HasHospital(long userHositalID, long hositalID);

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect(long hospitalID);
    }
}
