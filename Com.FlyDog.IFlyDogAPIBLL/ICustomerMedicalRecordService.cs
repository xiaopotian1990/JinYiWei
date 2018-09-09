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
    /// 顾客病例模板接口
    /// </summary>
  public  interface ICustomerMedicalRecordService
    {
        /// <summary>
        /// 添加顾客病例模板
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Add(CustomerMedicalRecordAdd dto);

        /// <summary>
        /// 修改顾客病例模板
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Update(CustomerMedicalRecordUpdate dto);

        /// <summary>
        /// 查询所有顾客病例模板
        /// </summary>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, IEnumerable<CustomerMedicalRecordInfo>> Get(CustomerMedicalRecordSelect dto);

        /// <summary>
        /// 根据ID获取顾客病例模板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, CustomerMedicalRecordInfo> GetByID(long id);


        /// <summary>
        /// 根据id获取客户病例模板详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, CustomerMedicalRecordInfo> GetByPKID(long id);


        /// <summary>
        /// 删除顾客病例模板
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        IFlyDogResult<IFlyDogResultType, int> Delete(CustomerMedicalRecordDelete dto);
    }
}
