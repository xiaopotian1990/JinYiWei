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
    /// 班次管理控制器
    /// </summary>
    public class SmartShiftCategoryController : Controller
    {
        #region 班次类型信息
        /// <summary>
        /// 班次类型信息
        /// </summary>
        /// <returns></returns>
        public ActionResult SmartShiftCategoryInfo()
        {
            return View();
        }
        #endregion

        #region 查询所有班次类型信息
        /// <summary>
        /// 查询所有班次信息
        /// </summary>
        /// <returns></returns>
        public async Task<string> SmartShiftCategoryGet()
        {
            var result = await WebAPIHelper.Get("/api/SmartShiftCategory/Get", new Dictionary<string, string>());
            return result;
        }
        #endregion

        #region 班次类型设置启用，停用
        /// <summary>
        /// 班次设置启用，停用
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SmartShiftEditStart(SmartShiftCategoryDispose dto)
        {
            var dic = new Dictionary<string, string>();
            dic.Add("CreateUserID", IDHelper.GetUserID().ToString());
            dic.Add("ID", dto.ID.ToString());
            dic.Add("Status", dto.Status);
            var result = await WebAPIHelper.Post("/api/SmartShiftCategory/SmartShiftCategoryDispose", dic);
            return result;
        }
        #endregion

        #region 根据班次信息id查询班次信息
        /// <summary>
        ///根据班次信息id查询班次信息
        /// </summary>
        /// <param name="smartShiftCategoryInfo">查询的班次信息实体</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SmartShiftCategoryEditGet(SmartShiftCategoryInfo smartShiftCategoryInfo)
        {
            var d = new Dictionary<string, string>();
            d.Add("id", smartShiftCategoryInfo.ID.ToString());
            var result = await WebAPIHelper.Get("/api/SmartShiftCategory/GetByID", d);
            return result;
        }
        #endregion

        #region 更新班次信息
        /// <summary>
        /// 更新班次信息
        /// </summary>
        /// <param name="smartShiftCategoryUpdate"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SmartShiftCategorySubmit(SmartShiftCategoryUpdate smartShiftCategoryUpdate)
        {
            var dic = new Dictionary<string, string>();
            dic.Add("ID", smartShiftCategoryUpdate.ID.ToString());
            dic.Add("CreateUserID", IDHelper.GetUserID().ToString());
            dic.Add("Name", smartShiftCategoryUpdate.Name);
            dic.Add("Status", smartShiftCategoryUpdate.Status.ToString());
            dic.Add("Type", smartShiftCategoryUpdate.Type.ToString());
            var result = await WebAPIHelper.Post("/api/SmartShiftCategory/Update", dic);
            return result;
        }
        #endregion

        #region 添加班次信息
        /// <summary>
        /// 添加班次信息
        /// </summary>
        /// <param name="smartShiftCategoryAdd"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> SmartShiftCategoryAdd(SmartShiftCategoryAdd smartShiftCategoryAdd)
        {
            smartShiftCategoryAdd.CreateUserID = IDHelper.GetUserID();
            var result = await WebAPIHelper.Post("/api/SmartShiftCategory/Add", smartShiftCategoryAdd);
            return result;
        }
        #endregion
    }
}