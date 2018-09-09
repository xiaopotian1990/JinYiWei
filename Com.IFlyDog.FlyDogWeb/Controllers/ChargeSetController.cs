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
    public class ChargeSetController : Controller
    {
        /// <summary>
        /// 套餐管理页面
        /// </summary>
        /// <returns></returns>
        // GET: ChargeSet
        public ActionResult ChargeSetInfo()
        {
            return View();
        }

        /// <summary>
        /// 套餐管理添加详细页
        /// </summary>
        /// <returns></returns>
        public ActionResult ChargeSetIndex()
        {
            return View();
        }

        #region 查询所有数据
        [HttpPost]
        public async Task<string> ChargeSetGet(ChargeSetSelect dto)
        {
            var result = await WebAPIHelper.Post("/api/ChargeSet/Get",
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
        public async Task<string> ChargeSetGetByID(string id)
        {
            var d = new Dictionary<string, string>();
            d.Add("ID", id);
            var result = await WebAPIHelper.Get("/api/ChargeSet/GetByID", d);
            return result;
        }

        /// <summary>
        ///     修改数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ChargeSetEdit(ChargeSetUpdate dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/ChargeSet/Update", dto);
            return result;
        }

        /// <summary>
        ///     添加数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ChargeSetAdd(ChargeSetAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/ChargeSet/Add", dto);
            return result;
        }
    }
}