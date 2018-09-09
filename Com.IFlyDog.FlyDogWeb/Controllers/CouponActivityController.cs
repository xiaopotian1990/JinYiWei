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
    public class CouponActivityController : Controller
    {
        /// <summary>
        /// 卷活动页面
        /// </summary>
        /// <returns></returns>
        // GET: CouponActivity
        public ActionResult CouponActivityInfo()
        {
            return View();
        }

        /// <summary>
        /// 管理活动
        /// </summary>
        /// <returns></returns>
        public ActionResult CouponActivityDetailInfo(string CategoryID)
        {
            return View();
        }

        /// <summary>
        ///     添加卷活动
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CouponActivityAdd(CouponActivityAdd dto)
        {
            dto.HospitalID = IDHelper.GetHospitalID().ToString();
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/CouponActivityService/Add", dto);
            return result;
        }

        #region 查询所有数据
        public async Task<string> CouponActivityGet(CouponActivitySelect dto)
        {
            dto.HospitalID = IDHelper.GetHospitalID().ToString();
            var result = await WebAPIHelper.Post("/api/CouponActivityService/Get",
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
        public async Task<string> CouponActivityGetByID(string id)
        {
            var d = new Dictionary<string, string>();
            d.Add("id", id);
            var result = await WebAPIHelper.Get("/api/CouponActivityService/GetByID", d);
            return result;
        }

        /// <summary>
        ///     修改数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CouponActivityEdit(CouponActivityUpdate dto)
        {
            dto.HospitalID = IDHelper.GetHospitalID().ToString();
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/CouponActivityService/Update", dto);
            return result;
        }

        #region 删除数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="smartSupplierDelete"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CouponActivityDelete(CouponActivityDelete dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/CouponActivityService/CouponActivityDelete", dto);
            return result;
        }
        #endregion


        /// <summary>
        ///     添加卷活动详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CouponActivityDetailAdd(CouponActivityDetailAdd dto)
        {
            dto.HospitalID = IDHelper.GetHospitalID();
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/CouponActivityDetail/Add", dto);
            return result;
        }

        #region 查询所有卷活动详情
        public async Task<string> CouponActivityDetailGet(CouponActivityDetailSelect dto)
        {
            var result = await WebAPIHelper.Post("/api/CouponActivityDetail/Get",
            dto);
            return result;
        }
        #endregion

        #region 删除单个卷活动详情
        /// <summary>
        /// 删除单个卷活动详情
        /// </summary>
        /// <param name="CouponActivityDetailDelete"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CouponActivityDetailDelete(CouponActivityDetailDelete dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/CouponActivityDetail/CouponActivityDetailDelete", dto);
            return result;
        }
        #endregion

        #region 删除全部卷活动详情
        /// <summary>
        /// 删除全部卷活动详情
        /// </summary>
        /// <param name="CouponActivityDetailDelete"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CouponActivityDetailDeleteAllDelete(CouponActivityDetailDelete dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/CouponActivityDetail/CouponActivityDetailDeleteAll", dto);
            return result;
        }
        #endregion

    }
}