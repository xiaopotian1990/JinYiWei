using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Com.IFlyDog.APIDTO.CallbackGroup;
using Com.IFlyDog.FlyDogWeb.Helper;

namespace Com.IFlyDog.FlyDogWeb.Controllers
{
    /// <summary>
    ///     回访组设置
    /// </summary>
    public class CallbackGroupController : Controller
    {
        // 回访组设置 查询全部
        public ActionResult Index()
        {
            return View();
        }

        //添加回访组详细
        public ActionResult CallbackGroupDetaiIndex()
        {
            return View();
        }

        // 回访组设置 查询全部
        public async Task<string> CallbackGroupGet()
        {
            var result = await WebAPIHelper.Get("/api/CallbackGroup/Get", new Dictionary<string, string>());
            return result;
        }

        /// <summary>
        ///     通过ID查询
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CallbackGroupGetByID(SmartCallbackGroup dto)
        {
            var d = new Dictionary<string, string>();
            d.Add("id", dto.ID);
            var result = await WebAPIHelper.Get("/api/CallbackGroup/GetByID", d);
            return result;
        }

  
        /// <summary>
        ///     修改数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CallbackGroupEdit(SmartCallbackGroupUpdate dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/CallbackGroup/Update", dto);
            return result;
        }

        /// <summary>
        ///     停用数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<string> CallbackGroupDisable(SmartCallbackGroupStopOrUse dto)
        {
            var d = new Dictionary<string, string>();
            d.Add("CreateUserID", IDHelper.GetUserID().ToString());
            d.Add("CallbackGroupID", dto.CallbackGroupID.ToString());
            d.Add("Status", dto.Status.ToString());
            var result = WebAPIHelper.Post("/api/CallbackGroup/StopOrUse", d);
            return result;
        }

        /// <summary>
        ///  添加数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> CallbackGroupAdd(SmartCallbackGroupAdd dto)
        {

            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/CallbackGroup/Add", dto);
            return result;
        }
    }
}