using Com.IFlyDog.APIDTO;
using Com.IFlyDog.FlyDogWeb.Helper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Com.IFlyDog.FlyDogWeb.Controllers
{
    /// <summary>
    /// 回访类型管理
    /// </summary>
    public class CallbackCategoryController : Controller
    {
        // 回访类型管理 查询全部
        public ActionResult Index()
        {
            return View();
        }

        public async Task<string> CallbackCategoryGet()
        {
            var result = await WebAPIHelper.Get("/api/CallbackCategory/Get", new Dictionary<string, string>());
            return result;
        }

        /// <summary>
        ///  通过ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CallbackGetByID(SmartCallbackCategory dto)
        {
            var d = new Dictionary<string, string>();
            d.Add("id", dto.ID.ToString());
            var result = await WebAPIHelper.Get("/api/CallbackCategory/GetByID", d);
            return result;
        }

        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CallbackEdit(SmartCallbackCategoryUpdate dto)
        {
            var d = new Dictionary<string, string>();
            d.Add("id", dto.ID.ToString());
            d.Add("CreateUserID", IDHelper.GetUserID().ToString());
            d.Add("Name", dto.Name);
            d.Add("Remark", dto.Remark);
            var result = await WebAPIHelper.Post("/api/CallbackCategory/Update", d);
            return result;
        }

        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="id">POST</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CallbackAdd(SmartCallbackCategoryAdd dto)
        {
            var d = new Dictionary<string, string>();
            d.Add("CreateUserID", IDHelper.GetUserID().ToString());
            d.Add("Name", dto.Name);
            d.Add("Remark", dto.Remark);
            var result = await WebAPIHelper.Post("/api/CallbackCategory/Add", d);
            return result;
        }

        /// <summary>
        /// 停用数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<string> CallbackDisable(SmartCallbackCategoryStopOrUse dto)
        {
            var d = new Dictionary<string, string>();
            d.Add("CreateUserID", IDHelper.GetUserID().ToString());
            d.Add("CallbackID", dto.CallbackID.ToString());
            d.Add("Status", dto.Status.ToString());
            var result = WebAPIHelper.Post("/api/CallbackCategory/StopOrUse", d);
            return result;
        }
    }
}