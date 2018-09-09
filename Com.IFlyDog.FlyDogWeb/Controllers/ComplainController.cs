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
    /// 投诉处理控制器
    /// </summary>
    public class ComplainController : Controller
    {
        /// <summary>
        /// 投诉处理页面
        /// </summary>
        /// <returns></returns>
        // GET: Complain
        public ActionResult ComplainIndex()
        {
            return View();
        }

        #region 查询所有数据
        public async Task<string> ComplainGet(ComplainSelect dto)
        {
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/Complain/Get",
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
        public async Task<string> ComplainGetByID(string id)
        {
            var d = new Dictionary<string, string>();
            d.Add("id", id);

            var result = await WebAPIHelper.Get("/api/Complain/GetByID", d);
            return result;
        }

        /// <summary>
        ///     修改数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ComplainEdit(ComplainUpdate dto)
        {
            dto.FinishUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Complain/Update", dto);
            return result;
        }
    }
}