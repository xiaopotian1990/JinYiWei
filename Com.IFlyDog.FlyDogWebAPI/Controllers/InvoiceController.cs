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
    /// 采购发票管理api
    /// </summary>
    public class InvoiceController : ApiController
    {
        /// <summary>
        /// 依赖注入
        /// </summary>
        private IInvoiceService _invoiceService;

        /// <summary>
        /// 依赖注入方法重构
        /// </summary>
        /// <param name="invoiceService"></param>
        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        /// <summary>
        /// 查询所有退货信息[所属角色("CRM")]
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, Pages<IEnumerable<InvoiceInfo>>> Get([FromBody]InvoiceSelect dto1)
        {
            return _invoiceService.Get(dto1);
        }

        /// <summary>
        /// 根据ID查询一条数据
        /// </summary>
        [HttpGet]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, InvoiceInfo> GetByID(long id)
        {
            return _invoiceService.GetByID(id);
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Add(InvoiceAdd dto)
        {
            return _invoiceService.Add(dto);
        }

        /// <summary>
        /// 删除采购发票管理[所属角色("CRM")]
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleAuthorization("CRM")]
        public IFlyDogResult<IFlyDogResultType, int> Delete([FromBody]InvoiceDelete dto)
        {
            return _invoiceService.InvoiceDel(dto);
        }
    }
}
