using Com.IFlyDog.APIDTO;
using Com.IFlyDog.FlyDogWeb.Helper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Com.IFlyDog.FlyDogWeb.Controllers
{
    public class KnowledgeCategoryController : Controller
    {
        // GET: 知识分类
        string userID = string.Empty;

        /// <summary>
        /// 知识分类页面
        /// </summary>
        /// <returns></returns>
        public ActionResult KnowledgeCategoryInfo()
        {
            return View();
        }

        /// <summary>
        /// 获取全部知识分类
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> KnowledgeCategoryIndexGet()
        {
            var result = await WebAPIHelper.Get("/api/KnowledgeCategory/Get", new Dictionary<string, string>());

            return result;
        }

        /// <summary>
        /// 获取数据id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> KnowledgeCategoryGetByID(KnowledgeCategory dto)
        {
            var d = new Dictionary<string, string>();
            d.Add("id", dto.ID.ToString());
            var result = await WebAPIHelper.Get("/api/KnowledgeCategory/GetByID", d);
            return result;
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> KnowledgeCategoryEdit(KnowledgeCategoryUpdate dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/KnowledgeCategory/Update", dto);
            return result;
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> KnowledgeCategoryAdd(KnowledgeCategoryAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/KnowledgeCategory/Add", dto);
            return result;
        }

    }
}