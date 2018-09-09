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
    public class CheckController : Controller
    {
        /// <summary>
        ///库存盘点详情页面（首页数据加载页）
        /// </summary>
        /// <returns></returns>
        // GET: Check
        public ActionResult CheckInfo()
        {
            return View();
        }

        /// <summary>
        /// 库存盘点详情详细页
        /// </summary>
        /// <returns></returns>
        public ActionResult CheckDetailIndex()
        {
            return View();
        }

        #region 查询所有数据
        public async Task<string> CheckGet(CheckSelect dto)
        {
            dto.CreateUserId = IDHelper.GetUserID().ToString();
            var result = await WebAPIHelper.Post("/api/Check/Get",
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
        public async Task<string> CheckGetByID(CheckSelect dto)
        {
            var d = new Dictionary<string, string>();
            d.Add("ID", dto.ID.ToString());
            var result = await WebAPIHelper.Get("/api/Check/GetByID", d);
            return result;
        }

        /// <summary>
        ///     添加数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CheckAdd(CheckAdd dto)
        {

            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Check/Add", dto);
            return result;
        }
    }
}