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
    /// 回款记录控制器
    /// </summary>
    public class SaleBackController : Controller
    {
        /// <summary>
        ///回款数据展示页
        /// </summary>
        /// <returns></returns>
        // GET: SaleBack
        public ActionResult SaleBackInfo()
        {
            return View();
        }

        #region 查询所有数据
        public async Task<string> SaleBackCGet(SaleBackSelect dto)
        {
            dto.HospitalID = IDHelper.GetHospitalID().ToString();
            dto.CreateUserID= IDHelper.GetUserID().ToString();
            var result = await WebAPIHelper.Post("/api/SaleBack/Get",
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
        public async Task<string> SaleBackGetByID(string id)
        {
            var d = new Dictionary<string, string>();
            d.Add("id", id);

            var result = await WebAPIHelper.Get("/api/SaleBack/GetByID", d);
            return result;
        }

        /// <summary>
        ///     修改数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SaleBackEdit(SaleBackUpdate dto)
        {
            dto.CreateUserID = IDHelper.GetUserID().ToString();
            var result = await WebAPIHelper.Post("/api/SaleBack/Update", dto);
            return result;
        }

        /// <summary>
        ///     添加数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SaleBackAdd(SaleBackAdd dto)
        {
            dto.HospitalID = IDHelper.GetHospitalID().ToString();
            dto.CreateTime = DateTime.Now.ToString();
            dto.CreateUserID = IDHelper.GetUserID().ToString();
            var result = await WebAPIHelper.Post("/api/SaleBack/Add", dto);
            return result;
        }

        #region 删除数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SaleBackDelete(SaleBackDelete dto)
        {
            dto.CreateUserID = IDHelper.GetUserID().ToString();
            var result = await WebAPIHelper.Post("/api/SaleBack/Delete", dto);
            return result;
        }
        #endregion

    }
}