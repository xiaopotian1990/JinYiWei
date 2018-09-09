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
    /// 渠道组页面控制器
    /// </summary>
    public class ChannelGroupController : Controller
    {
        /// <summary>
        /// 渠道组信息
        /// </summary>
        /// <returns></returns>
        // GET: ChannelGroup
        public ActionResult ChannelGroupInfo()
        {
            return View();
        }

        #region 查询所有渠道组信息
        /// <summary>
        /// 查询所有渠道组信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ChannelGroupPost()
        {
            var result = await WebAPIHelper.Get("/api/ChannelGroup/Get", new Dictionary<string, string>());
            return result;
        }
        #endregion

        #region 根据id查询渠道组信息
        /// <summary>
        ///根据id查询渠道组信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ChannelGroupEditGet(string id)
        {
            var d = new Dictionary<string, string>();
            d.Add("id", id);
            var result = await WebAPIHelper.Get("/api/ChannelGroup/GetByID", d);
            return result;
        }
        #endregion

        #region 新增渠道组信息
        /// <summary>
        /// 新增渠道组信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ChannelGroupAdd(ChannelGroupAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/ChannelGroup/Add", dto);
            return result;
        }
        #endregion

        #region 更新渠道组信息
        /// <summary>
        /// 更新渠道组信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ChannelGroupSubmit(ChannelGroupUpdate dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/ChannelGroup/Update", dto);
            return result;
        }
        #endregion

        #region 删除渠道组信息
        /// <summary>
        /// 删除渠道组信息TagGroupDelete dto
        /// </summary>
        /// <returns></returns>

        public async Task<string> ChannelGroupDelete(ChannelGroupDelete dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/ChannelGroup/Delete", dto);
            return result;
        }
        #endregion

        #region 查询检测渠道信息
        /// <summary>
        /// 查询检测渠道信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> GetChannelGroupCheckPost()
        {
            var result = await WebAPIHelper.Get("/api/ChannelGroup/GetChannelGroupCheck", new Dictionary<string, string>());
            return result;
        }
        #endregion

    }
}