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
    /// 打印设置接口
    /// </summary>
   public interface IHospitalPrintService
    {

        /// <summary>
        /// 修改打印设置
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(HospitalPrintUpdate dto);


        /// <summary>
        /// 查询医院下所有打印设置
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<HospitalPrintInfo>> Get(string hospitalID);

        /// <summary>
        /// 根据id查询打印设置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, HospitalPrintInfo> GetByID(long id);

        /// <summary>
        /// 根据id查询打印设置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, HospitalPrintInfo> GetByHospitalAndType(long hospitalID,string type);
    }
}
