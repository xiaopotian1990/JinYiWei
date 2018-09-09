using Com.IFlyDog.APIDTO;
using Com.IFlyDog.FlyDogWeb.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Com.IFlyDog.FlyDogWeb.Controllers
{
    public class InvoiceController : Controller
    {
        /// <summary>
        /// 采购发票详情页面（首页数据加载页）
        /// </summary>
        /// <returns></returns>
        // GET: Invoice
        public ActionResult InvoiceInfo()
        {
            return View();
        }

        /// <summary>
        /// 采购发票详情详细页
        /// </summary>
        /// <returns></returns>
        public ActionResult InvoiceIndex()
        {
            return View();
        }

        #region 查询所有数据
        public async Task<string> InvoiceGet(InvoiceSelect dto)
        {
            dto.HospitalID = IDHelper.GetHospitalID().ToString();
            var result = await WebAPIHelper.Post("/api/Invoice/Get",
            dto);
            return result;
        }
        #endregion

        /// <summary>
        ///     通过ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> InvoiceGetByID(InvoiceSelect dto)
        {
            var d = new Dictionary<string, string>();
            d.Add("ID", dto.ID.ToString());

            var result = await WebAPIHelper.Get("/api/Invoice/GetByID", d);
            return result;
        }

        /// <summary>
        ///     添加数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> InvoiceAdd(InvoiceAdd dto)
        {

            dto.CreateUserID = IDHelper.GetUserID().ToString();
            dto.HospitalID = IDHelper.GetHospitalID().ToString();
            var result = await WebAPIHelper.Post("/api/Invoice/Add", dto);
            return result;
        }

        #region 删除数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="smartSupplierDelete"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> InvoiceDelete(InvoiceDelete dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Invoice/Delete", dto);
            return result;
        }
        #endregion
    }
}