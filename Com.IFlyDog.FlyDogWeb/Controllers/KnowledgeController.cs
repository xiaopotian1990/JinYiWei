using Com.IFlyDog.APIDTO.Knowledge;
using Com.IFlyDog.CommonDTO;
using Com.IFlyDog.FlyDogWeb.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Com.IFlyDog.FlyDogWeb.Controllers
{
    public class KnowledgeController : Controller
    {
        // GET: 知识管理
        string userID = string.Empty;


        /// <summary>
        /// 知识管理页面
        /// </summary>
        /// <returns></returns>
        public ActionResult KnowledgeInfo()
        {
            return View();
        }


        /// <summary>
        /// 获取全部知识信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> KnowledgeIndexGet(KnowledgeSelect dto)
        {
            var result = await WebAPIHelper.Post("/api/Knowledge/Get", dto);
            return result;
        }

        /// <summary>
        /// 获取数据id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> KnowledgeGet(string id)
        {
            var d = new Dictionary<string, string>();
            d.Add("id", id);
            var result = await WebAPIHelper.Get("/api/Knowledge/GetByID", d);
            return result;
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> KnowledgeEdit(KnowledgeUpdate dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Knowledge/Update", dto);
            return result;
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> KnowledgeAdd(KnowledgeAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/Knowledge/Add", dto);
            return result;
        }

    }
}