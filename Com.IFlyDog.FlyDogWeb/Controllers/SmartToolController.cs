using Com.IFlyDog.APIDTO;
using Com.IFlyDog.FlyDogWeb.Helper;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Com.IFlyDog.FlyDogWeb.Controllers
{
    public class SmartToolController : Controller
    {
        // 工具管理
        public ActionResult Index()
        {
            return View();
        }

        ///查询全部
        public async Task<string> ToolGet()
        {

            var result = await WebAPIHelper.Get("/api/SmartTool/Get", new Dictionary<string, string>());
            return result;
        }


        /// <summary>
        ///     工具添加
        /// </summary>
        /// <param name="SmartToolAdd"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ToolAdd(SmartToolAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/SmartTool/Add", dto);
            return result;
        }


        //设置停用
        [HttpPost]
        public async Task<string> ToolDisable(SmartToolStopOrUse dto)
        {
            var dic = new Dictionary<string, string>();
            dic.Add("CreateUserID", IDHelper.GetUserID().ToString());
            dic.Add("ID", dto.ID.ToString());
            dic.Add("Status", dto.Status.ToString());
            var result = await WebAPIHelper.Post("/api/SmartTool/StopOrUse", dic);
            return result;
        }

        /// <summary>
        ///     修改数据根据ID查询本条数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ToolEditGetByID(SmartToolUpdate dto)
        {
            var d = new Dictionary<string, string>();
            d.Add("ID", dto.ID.ToString());
            var result = await WebAPIHelper.Get("/api/SmartTool/GetByID", d);
            return result;
        }

        /// <summary>
        ///     提交修改
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ToolEditSubmit(SmartToolUpdate dto)
        {
            var dic = new Dictionary<string, string>();
            dic.Add("id", dto.ID.ToString());
            dic.Add("CreateUserID", IDHelper.GetUserID().ToString());
            dic.Add("Name", dto.Name);
            dic.Add("Remark", dto.Remark);
            var result = await WebAPIHelper.Post("/api/SmartTool/Update", dic);
            return result;
        }
    }
}