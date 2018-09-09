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
    public class ChargeDiscountController : Controller
    {
        /// <summary>
        /// 项目折扣页面
        /// </summary>
        /// <returns></returns>
        // GET: ChargeDiscount
        public ActionResult ChargeDiscountInfo()
        {
            return View();
        }

        #region 查询所有数据
        [HttpPost]
        public async Task<string> ChargeDiscountGet(ChargeDiscountSelect dto)
        {
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/ChargeDiscount/Get",
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
        public async Task<string> ChargeDiscountGetByID(ChargeDiscountInfo dto)
        {
            var d = new Dictionary<string, string>();
            d.Add("ID", dto.ID.ToString());
            var result = await WebAPIHelper.Get("/api/ChargeDiscount/GetByID", d);
            return result;
        }

        /// <summary>
        ///     修改数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ChargeDiscountEdit(ChargeDiscountUpdate dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/ChargeDiscount/Update", dto);
            return result;
        }

        /// <summary>
        ///     添加数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ChargeDiscountAdd(ChargeDiscountAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            dto.HospitalID = IDHelper.GetHospitalID();
            var result = await WebAPIHelper.Post("/api/ChargeDiscount/Add", dto);
            return result;
        }
    }
}