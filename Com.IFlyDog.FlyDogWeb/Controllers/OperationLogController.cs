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
    /// <summary>
    /// 系统日志
    /// </summary>
    public class OperationLogController : Controller
    {
        /// <summary>
        /// 系统日志页面
        /// </summary>
        /// <returns></returns>
        // GET: OperationLog
        public ActionResult OperationLogInfo()
        {
            return View();
        }

        #region 查询所有系统日志
        /// <summary>
        /// 查询所有系统日志
        /// </summary>
        /// <returns></returns>
        public async Task<string> OperationLogGet(OperationLogSelect dto)
        {
            dto.HospitalID = IDHelper.GetHospitalID().ToString();
            var result = await WebAPIHelper.Post("/api/OperationLog/GetPages",
           dto);
            return result;
        }
        #endregion

        #region 根据id查询系统日志信息
        /// <summary>
        ///根据id查询系统日志信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> OperationLogEditGet(string id)
        {
            var d = new Dictionary<string, string>();
            d.Add("id", id);
            var result = await WebAPIHelper.Get("/api/OperationLog/GetByID", d);
            return result;
        }
        #endregion

        #region 获取日志类型下拉列表
        /// <summary>
        ///获取日志类型下拉列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> GetLogSelect()
        {
            var result = await WebAPIHelper.Get("/api/OperationLog/GetLogSelect", new Dictionary<string, string>());
            return result;
        }
        #endregion
    }
}