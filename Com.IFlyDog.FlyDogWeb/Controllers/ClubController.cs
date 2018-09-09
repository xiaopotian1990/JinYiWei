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
    public class ClubController : Controller
    {
        /// <summary>
        /// 单项目管理页面
        /// </summary>
        /// <returns></returns>
        // GET: Club
        public ActionResult ClubInfo()
        {
            return View();
        }

        #region 查询所有单项目管理
        /// <summary>
        /// 查询所有单项目管理
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ClubtGet()
        {
            var d = new Dictionary<string, string>();
            d.Add("hospitalID",IDHelper.GetHospitalID().ToString());
            var result = await WebAPIHelper.Get("/api/Club/Get",d);
            return result;
        }
        #endregion

        #region 新增单项目管理
        /// <summary>
        /// 新增单项目管理
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ClubAdd(ClubAdd clubAdd)
        {
            clubAdd.UserID = IDHelper.GetUserID().ToString();
            clubAdd.HospitalID = IDHelper.GetHospitalID().ToString();
            var result = await WebAPIHelper.Post("/api/Club/Add", clubAdd);
            return result;
        }
        #endregion

        #region 删除单项目管理
        /// <summary>
        /// 删除单项目管理
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ClubDelete(ClubDelete clubDelete)
        {
            clubDelete.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Club/Delete", clubDelete);
            return result;
        }
        #endregion
    }
}