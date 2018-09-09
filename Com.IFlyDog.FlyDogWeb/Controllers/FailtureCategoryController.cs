using Com.IFlyDog.APIDTO;
using Com.IFlyDog.FlyDogWeb.Helper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Com.IFlyDog.FlyDogWeb.Controllers
{
    /// <summary>
    /// 未成交类型
    /// </summary>
    public class FailtureCategoryController : Controller
    {
        // GET: FailtureCategory
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        public async Task<string> FailtureGet()
        {
            var result = await WebAPIHelper.Get("/api/FailtureCategory/Get", new Dictionary<string, string>());
            return result;
        }

        /// <summary>
        /// 获取数据id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> FailtureCategoryGetByID(FailtureCategory dto)
        {
            var d = new Dictionary<string, string>();
            d.Add("id", dto.ID);
            var result = await WebAPIHelper.Get("/api/FailtureCategory/GetByID", d);
            return result;
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> FailtureCategoryEdit(FailtureCategoryUpdate dto)
        { 
            var d = new Dictionary<string, string>();
            d.Add("id", dto.ID.ToString());
            d.Add("CreateUserID", IDHelper.GetUserID().ToString());
            d.Add("Name", dto.Name);
            d.Add("Remark", dto.Remark);
            var result = await WebAPIHelper.Post("/api/FailtureCategory/Update", d);
            return result;
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> FailtureCategoryAdd(FailtureCategoryAdd dto)
        {
            
            var d = new Dictionary<string, string>();
            d.Add("CreateUserID", IDHelper.GetUserID().ToString());
            d.Add("Name", dto.Name);
            d.Add("Remark", dto.Remark);
        
            var result = await WebAPIHelper.Post("/api/FailtureCategory/Add", d);
            return result;
        }

        /// <summary>
        /// 停用数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<string> FailtureCategoryDisable(FailtureCategoryStopOrUse dto)
        {
            
            var d = new Dictionary<string, string>();
            d.Add("CreateUserID", IDHelper.GetUserID().ToString());
            d.Add("FailtureCategoryID", dto.FailtureCategoryID.ToString());
 
            d.Add("Status", dto.Status.ToString());
            var result = WebAPIHelper.Post("/api/FailtureCategory/StopOrUse", d);
            return result;
        }


    }
}