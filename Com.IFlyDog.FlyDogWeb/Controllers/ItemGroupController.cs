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
    public class ItemGroupController : Controller
    {
        /// <summary>
        /// 报表项目组页
        /// </summary>
        /// <returns></returns>
        // GET: ItemGroup
        public ActionResult ItemGroupInfo()
        {
            return View();
        }

        #region 查询所有报表项目组
        /// <summary>
        /// 查询所有报表项目组
        /// </summary>
        /// <returns></returns>
        public async Task<string> ItemGroupGet()
        {
            var result = await WebAPIHelper.Get("/api/ItemGroup/Get", new Dictionary<string, string>());
            return result;
        }
        #endregion

        #region 根据id查询报表项目组详情
        /// <summary>
        ///根据id查询报表项目组详情
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ItemGroupEditGet(string id)
        {
            var d = new Dictionary<string, string>();
            d.Add("id", id);
            var result = await WebAPIHelper.Get("/api/ItemGroup/GetByID", d);
            return result;
        }
        #endregion

        #region 新增报表项目组详情
        /// <summary>
        /// 新增报表项目组详情
        /// </summary>
        /// <param name="smartUnitAdd"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ItemGroupAdd(ItemGroupAdd itemGroupAdd)
        {
            itemGroupAdd.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/ItemGroup/Add", itemGroupAdd);
            return result;
        }
        #endregion

        #region 更新项目组详情
        /// <summary>
        /// 更新项目组详情
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ItemGroupSubmit(ItemGroupUpdate itemGroupUpdate)
        {
            itemGroupUpdate.CreateUserID=IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/ItemGroup/Update", itemGroupUpdate);
            return result;
        }
        #endregion

        #region 删除项目组详情
        /// <summary>
        /// 删除项目组详情
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> ItemGroupDelete(ItemGroupDelete itemGroupDelete)
        {
            itemGroupDelete.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/ItemGroup/Delete", itemGroupDelete);
            return result;
        }
        #endregion
    }
}