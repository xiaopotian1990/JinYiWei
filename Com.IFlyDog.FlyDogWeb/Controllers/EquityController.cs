using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.FlyDogWeb.Helper;

namespace Com.IFlyDog.FlyDogWeb.Controllers
{
    public class EquityController : Controller
    {
        // 权益设置
        public ActionResult Index()
        {
            return View();
        }

        //查询全部权益
        [HttpPost]
        public async Task<string> EquityIndexGet()
        {
            var result = await WebAPIHelper.Get("/api/Equity/Get", new Dictionary<string, string>());
            return result;
        }

        /// <summary>
        ///查询全部可用 的会员权益
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> EquityStatusIsTrueGet()
        {
            var result = await WebAPIHelper.Get("/api/Equity/GetStatusIsTrue", new Dictionary<string, string>());
            return result;
        }

        /// <summary>
        /// 停用数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<string> EquityDisable(EquityStopOrUse dto)
        {
            var d = new Dictionary<string, string>();
            d.Add("CreateUserID", IDHelper.GetUserID().ToString());
            d.Add("ID", dto.ID.ToString());
            d.Add("Status", dto.Status.ToString());
            var result = WebAPIHelper.Post("/api/Equity/StopOrUse", d);
            return result;
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<string> EquityAdd(EquityAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = WebAPIHelper.Post("/api/Equity/Add", dto);
            return result;
        }

   

        /// <summary>
        /// 获取数据id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> EquityGetByID(Equity dto)
        {

            var d = new Dictionary<string, string>();
            d.Add("id", dto.ID.ToString());
            var result = await WebAPIHelper.Get("/api/Equity/GetByID", d);
            return result;

        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<string> EquityUpdate(EquityUpdate dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = WebAPIHelper.Post("/api/Equity/Update", dto);
            return result;
        }
    }
}