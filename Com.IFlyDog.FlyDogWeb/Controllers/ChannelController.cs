using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using Com.IFlyDog.APIDTO;
using Com.IFlyDog.FlyDogWeb.Helper;

namespace Com.IFlyDog.FlyDogWeb.Controllers
{
    /// <summary>
    ///     渠道管理
    /// </summary>
    public class ChannelController : Controller
    {
        private string userID = string.Empty;
        // GET: Channel
        /// <summary>
        ///     获取数据
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<string> ChannelIndexGet()
        {
            var result = await WebAPIHelper.Get("/api/Channel/Get", new Dictionary<string, string>());
            return result;
        }

        /// <summary>
        ///     获取数据id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ChannelGetByID(Channel dto)
        {
            var dic = new Dictionary<string, string> {{"id", dto.ID}};
            var result = await WebAPIHelper.Get("/api/Channel/GetByID", dic);
            return result;
        }


        /// <summary>
        ///     查询所有可用的渠道
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ChannelGetIsOk()
        {
            var result = await WebAPIHelper.Get("/api/Channel/GetByIsOk", new Dictionary<string, string>());
            return result;
        }
        /// <summary>
        ///     修改数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ChannelEdit(ChannelUpdate dto)
        {
            var d = new Dictionary<string, string>
            {
                {"id", dto.ID.ToString()},
                {"CreateUserID", IDHelper.GetUserID().ToString()},
                {"Name", dto.Name},
                {"SortNo", dto.SortNo},
                {"Remark", dto.Remark}
            };
            var result = await WebAPIHelper.Post("/api/Channel/Update", d);
            return result;
        }

        /// <summary>
        ///     添加数据
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ChannelAdd(ChannelAdd dto)
        {
            var d = new Dictionary<string, string>();
            d.Add("CreateUserID", IDHelper.GetUserID().ToString());
            d.Add("Name", dto.Name);
            d.Add("SortNo", dto.SortNo);
            d.Add("Remark", dto.Remark);
            var result = await WebAPIHelper.Post("/api/Channel/Add", d);
            return result;
        }

        /// <summary>
        ///     停用数据
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        [HttpPost]
        public Task<string> ChannelDisable(ChannelStopOrUse dto)
        {
            var dic = new Dictionary<string, string>
            {
                {"CreateUserID", IDHelper.GetUserID().ToString()},
                {"ChannelID", dto.ChannelID.ToString()},
                {"Status", dto.Status.ToString()}
            };
            var result = WebAPIHelper.Post("/api/Channel/StopOrUse", dic);
            return result;
            //}
            //return "数据不存在,非法请求已记录";
        }
    }
}