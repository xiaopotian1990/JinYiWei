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
    /// 标签
    /// </summary>
    public class SmartTagController : Controller
    {
        // GET: 显示
        public ActionResult Index()
        {
            return View();
        }


        ///查询全部
        public async Task<string> TagGet()
        {
            var result = await WebAPIHelper.Get("/api/Tag/Get", new Dictionary<string, string>());
            return result;
        }

        /// <summary>
        /// 查询全部可用的标签
        /// </summary>
        /// <returns></returns>
        public async Task<string> TagGetByIsOk()
        {
            var result = await WebAPIHelper.Get("/api/Tag/GetByIsOk", new Dictionary<string, string>());
            return result;
        }


        /// <summary>
        ///     工具添加
        /// </summary>
        /// <param name="SmartTagAdd"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> TagAdd(TagAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Tag/Add", dto);
            return result;
        }


        //设置停用
        [HttpPost]
        public async Task<string> TagDisable(TagStopOrUse dto)
        {
            var dic = new Dictionary<string, string>();
            dic.Add("CreateUserID", IDHelper.GetUserID().ToString());
            dic.Add("ID", dto.ID.ToString());
            dic.Add("Status", dto.Status.ToString());
            var result = await WebAPIHelper.Post("/api/Tag/StopOrUse", dic);
            return result;
        }

        /// <summary>
        ///     修改数据根据ID查询本条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> TagEditGetByID(TagUpdate dto)
        {
            var d = new Dictionary<string, string>();
            d.Add("ID", dto.ID.ToString());
            var result = await WebAPIHelper.Get("/api/Tag/GetByID", d);
            return result;
        }

        /// <summary>
        ///提交修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> TagEditSubmit(TagUpdate dto)
        {
            var dic = new Dictionary<string, string>();
            dic.Add("id", dto.ID.ToString());
            dic.Add("CreateUserID", IDHelper.GetUserID().ToString());
            dic.Add("Content", dto.Content);
            var result = await WebAPIHelper.Post("/api/Tag/Update", dic);
            return result;
        }
    }
}