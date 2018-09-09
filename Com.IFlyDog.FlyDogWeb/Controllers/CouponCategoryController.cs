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
    public class CouponCategoryController : Controller
    {
        /// <summary>
        /// 代金券类型数据首页
        /// </summary>
        /// <returns></returns>
        // GET: CouponCategory
        public ActionResult CouponCategoryInfo()
        {
            return View();
        }

        #region 查询数据
        public async Task<string> CouponCategoryInfoGet()
        {
            var result = await WebAPIHelper.Get("/api/CouponCategory/Get",new Dictionary<string,string>());
            return result;
        }
        #endregion

        #region 根据医院id查询当前医院可以使用的卷类型
        public async Task<string> CouponCategoryInfoGetByHospitalID()
        {
            var d = new Dictionary<string, string>();
            d.Add("hospitalID",IDHelper.GetHospitalID().ToString());
            var result = await WebAPIHelper.Get("/api/CouponCategory/GetByHospitalID", d );
            return result;
        }
        #endregion

        #region 根据id获取信息
        /// <summary>
        /// 根据id获取信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> CouponCategoryGet(string id)
        {
            var d = new Dictionary<string, string>();
            d.Add("ID", id);
            var result = await WebAPIHelper.Get("/api/CouponCategory/GetByID", d);
            return result;
        }
        #endregion

        #region 修改数据
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <returns></returns>
        public async Task<string> SmartSupplierEdit(CouponCategoryUpdate dto)
        {
            // userID = Session["UserID"].ToString();
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/CouponCategory/Update", dto);
            return result;
        }
        #endregion

        /// <summary>
        /// 添加数据
        /// </summary>
        ///  /// <returns></returns>
        public async Task<string> SmartSupplierAdd(CouponCategoryAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/CouponCategory/Add", dto);
            return result;
        }


    }
}