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
    /// 分享家控制器
    /// </summary>
    public class ShareCategoryController : Controller
    {
        /// <summary>
        /// 分享家页面
        /// </summary>
        /// <returns></returns>
        // GET: ShareCategory
        public ActionResult ShareCategoryInfo()
        {
            return View();
        }

        /// <summary>
        /// 查询全部
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ShareCategoryGet()
        {
            var result = await WebAPIHelper.Get("/api/ShareCategory/Get", new Dictionary<string, string>());

            return result;
        }

        /// <summary>
        ///添加分享家
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ShareCategoryAdd(ShareCategoryAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/ShareCategory/Add", dto);
            return result;
        }

       /// <summary>
       /// 删除分享家
       /// </summary>
       /// <param name="dto"></param>
       /// <returns></returns>
        [HttpPost]
        public async Task<string> ShareCategoryDelete(ShareCategoryDelete dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/ShareCategory/Delete", dto);
            return result;
        }

        /// <summary>
        /// 通过ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ShareCategoryGetByID(string ID)
        {

            var dic = new Dictionary<string, string>();
            dic.Add("ID", ID);
            var result = await WebAPIHelper.Get("/api/ShareCategory/GetByID", dic);
            return result;

        }

        /// <summary>
        /// 更新分享家信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ShareCategoryUpdate(ShareCategoryUpdate dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/ShareCategory/Update", dto);
            return result;

        }
    }
}