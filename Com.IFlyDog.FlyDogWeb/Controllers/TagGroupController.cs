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
    public class TagGroupController : Controller
    {
        // GET: TagGroup
        /// <summary>
        /// 标签组页面
        /// </summary>
        /// <returns></returns>
        public ActionResult TagGroupInfo()
        {
            return View();
        }

        #region 查询所有标签组信息
        /// <summary>
        /// 查询所有标签组信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> TagGroupPost()
        {
            var result = await WebAPIHelper.Get("/api/TagGroup/Get", new Dictionary<string, string>());
            return result;
        }
        #endregion

        #region 根据id查询标签组信息
        /// <summary>
        ///根据id查询标签组信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> TagGroupEditGet(string id)
        {
            var d = new Dictionary<string, string>();
            d.Add("id", id);
            var result = await WebAPIHelper.Get("/api/TagGroup/GetByID", d);
            return result;
        }
        #endregion

        #region 新增标签组信息
        /// <summary>
        /// 新增标签组信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> TagGroupAdd(TagGroupAdd dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/TagGroup/Add", dto);
            return result;
        }
        #endregion

        #region 更新标签组信息
        /// <summary>
        /// 更新标签组信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> TagGroupSubmit(TagGroupUpdate dto)
        {
            dto.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/TagGroup/Update", dto);
            return result;
        }
        #endregion

        #region 删除标签组信息
        /// <summary>
        /// 删除标签组信息TagGroupDelete dto
        /// </summary>
        /// <returns></returns>
       
        public async Task<string> TagGroupDelete(string id)
        {
            TagGroupDelete td = new TagGroupDelete();
            td.ID = id;
            td.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/TagGroup/Delete", td);
            return result;
        }
        #endregion
    }
}