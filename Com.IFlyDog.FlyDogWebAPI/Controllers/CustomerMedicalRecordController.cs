using Com.FlyDog.IFlyDogAPIBLL;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.CommonDTO;
using Com.JinYiWei.WebAPI.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Com.IFlyDog.FlyDogWebAPI.Controllers
{
    /// <summary>
    /// 客户病例模板管理api
    /// </summary>
    public class CustomerMedicalRecordController : ApiController
    {
        private ICustomerMedicalRecordService _customerMedicalRecordService;

        #region 依赖注入类
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="customerMedicalRecordService"></param>
        public CustomerMedicalRecordController(ICustomerMedicalRecordService customerMedicalRecordService)
        {
            _customerMedicalRecordService = customerMedicalRecordService;
        }
        #endregion

        #region 添加客户病例模板
        /// <summary>
        /// 添加客户病例模板[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">添加客户病例模板</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add([FromBody]CustomerMedicalRecordAdd dto)
        {
            return _customerMedicalRecordService.Add(dto);
        }
        #endregion

        /// <summary>
        /// 根据病例模板详情查询客户病例模板详情(添加客户病例功能使用)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, CustomerMedicalRecordInfo> GetByID(long id)
        {
            return _customerMedicalRecordService.GetByID(id);
        }

        #region 列表查询客户病例信息
        /// <summary>
        /// 列表查询客户病例信息[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<CustomerMedicalRecordInfo>> Get(CustomerMedicalRecordSelect dto)
        {
            return _customerMedicalRecordService.Get(dto);
        }
        #endregion


        #region 根据id查询病例模板详情，客户病例模板列表使用
        /// <summary>
        /// 根据id查询病例模板详情，客户病例模板列表使用
        /// </summary>
        /// <param name="id">单位ID</param>
        /// <returns></returns>
        public IFlyDogResult<IFlyDogResultType, CustomerMedicalRecordInfo> GetByPKID(long id)
        {
            return _customerMedicalRecordService.GetByPKID(id);
        }
        #endregion


        #region 删除顾客病例模板
        /// <summary>
        /// 删除顾客病例模板[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">删除顾客病例模板</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Delete([FromBody]CustomerMedicalRecordDelete dto)
        {
            return _customerMedicalRecordService.Delete(dto);
        }
        #endregion

        #region 更新客户病例模板详情
        /// <summary>
        /// 更新客户病例模板详情[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">更新客户病例模板详情</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update([FromBody]CustomerMedicalRecordUpdate dto)
        {
            return _customerMedicalRecordService.Update(dto);
        }
        #endregion
    }
}
