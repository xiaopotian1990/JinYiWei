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
    /// 供应商管理相关API
    /// </summary>
    public class SmartSupplierController : ApiController
    {
        private ISmartSupplierService _smartSupplierService;
        /// <summary>
        /// 依赖注入类
        /// </summary>
        /// <param name="smartSupplierService"></param>
        public SmartSupplierController(ISmartSupplierService smartSupplierService)
        {
            _smartSupplierService = smartSupplierService;
        }

        /// <summary>
        /// 添加供应商管理[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">供应商管理</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add([FromBody]SmartSupplierAdd dto)
        {
            return _smartSupplierService.Add(dto);
        }

        /// <summary>
        /// 供应商管理修改[所属角色("CRM")]
        /// </summary>
        /// <param name="dto">供应商管理</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Update([FromBody]SmartSupplierUpdate dto)
        {
            return _smartSupplierService.Update(dto);
        }

        /// <summary>
        /// 获取全部供应商信息，不分页，主要给下拉列表使用 string hospitalID
        /// </summary>
        /// <returns></returns>
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<SmartSupplierInfo>> GetAll(string hospitalIDD)
        {
            return _smartSupplierService.GetAll(hospitalIDD);
        }

        /// <summary>
        /// 查询所有供应商管理[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<SmartSupplierInfo>>> Get([FromBody]SmartSupplierSelect dto1)
        {
            return _smartSupplierService.Get(dto1);
        }



        /// <summary>
        /// 根据ID获取供应商管理
        /// </summary>
        /// <param name="id">供应商管理ID</param>
        /// <returns></returns>
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, SmartSupplierInfo> GetByID(long id)
        {
            return _smartSupplierService.GetByID(id);
        }

        /// <summary>
        /// 删除供应商管理[所属角色("CRM")]
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Delete([FromBody]SmartSupplierDelete dto)
        {
            return _smartSupplierService.Delete(dto);
        }

        /// <summary>
        /// 下拉菜单
        /// </summary>
        /// <param name="hospitalID">医院ID</param>
        /// <returns></returns>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, IEnumerable<Select>> GetSelect(long hospitalID)
        {
            return _smartSupplierService.GetSelect(hospitalID);
        }
    }
}
