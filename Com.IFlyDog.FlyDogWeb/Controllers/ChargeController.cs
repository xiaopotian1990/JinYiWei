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
    public class ChargeController : Controller
    {
        /// <summary>
        ///收费项目详情页面（首页数据加载页）
        /// </summary>
        /// <returns></returns>
        // GET: SmartReturn
        public ActionResult ChargeInfo()
        {
            return View();
        }

        /// <summary>
        /// 收费项目详情详细页
        /// </summary>
        /// <returns></returns>
        public ActionResult ChargeIndex()
        {
            return View();
        }

        #region 查询所有数据(不分页主要给弹窗使用)
        public async Task<string> ChargeGetData(ChargeSelect dto)
        {          
            var result = await WebAPIHelper.Post("/api/Charge/GetData",dto);
            return result;
        }
        #endregion


        #region 查询所有数据
        public async Task<string> ChargeGet(ChargeSelect dto)
        {
            var result = await WebAPIHelper.Post("/api/Charge/Get",
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
        public async Task<string> ChargeGetByID(string id)
        {
            var d = new Dictionary<string, string>();
            d.Add("ID", id);
            var result = await WebAPIHelper.Get("/api/Charge/GetByID", d);
            return result;
        }

        /// <summary>
        ///     修改数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ChargeEdit(ChargeUpdate dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Charge/Update", dto);
            return result;
        }

        /// <summary>
        ///     添加数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ChargeAdd(ChargeAdd dto)
        {

            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Charge/Add", dto);
            return result;
        }
    }
}